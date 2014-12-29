﻿using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ClientsForm : Queue.UI.WinForms.RichForm
    {
        private const byte PageSize = 50;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private int startIndex = 0;
        private TaskPool taskPool;

        public ClientsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void ClientsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            RefreshClientsGridView();
        }

        private void clientsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = clientsGridView.Rows[e.RowIndex];
                Client client = row.Tag as Client;

                using (var f = new EditClientForm(channelBuilder, currentUser, client.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        ClientsGridViewRenderRow(row, f.Client);
                    };

                    f.ShowDialog();
                }
            }
        }

        private void ClientsGridViewRenderRow(DataGridViewRow row, Client client)
        {
            row.Cells["registerDateColumn"].Value = client.RegisterDate;
            row.Cells["surnameColumn"].Value = client.Surname;
            row.Cells["nameColumn"].Value = client.Name;
            row.Cells["patronymicColumn"].Value = client.Patronymic;
            row.Cells["emailColumn"].Value = client.Email;
            row.Cells["mobileColumn"].Value = client.Mobile;
            row.Tag = client;
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            RefreshClientsGridView();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (clientsGridView.Rows.Count == PageSize)
            {
                startIndex += PageSize;
            }
            RefreshClientsGridView();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            startIndex -= PageSize;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            RefreshClientsGridView();
        }

        private async void RefreshClientsGridView()
        {
            string query = queryTextBox.Text.Trim();

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var clients = await taskPool.AddTask(channel.Service.FindClients(startIndex, PageSize, query));

                    clientsGridView.Rows.Clear();
                    foreach (var c in clients)
                    {
                        var row = clientsGridView.Rows[clientsGridView.Rows.Add()];
                        ClientsGridViewRenderRow(row, c);
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            startIndex = 0;
            RefreshClientsGridView();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditClientForm(channelBuilder, currentUser))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = clientsGridView.Rows[clientsGridView.Rows.Add()];
                        row.Selected = true;
                    }
                    ClientsGridViewRenderRow(row, f.Client);
                };

                f.ShowDialog();
            }
        }

        private async void clientsGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить клиента?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Client client = e.Row.Tag as Client;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteClient(client.Id));
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException exception)
                    {
                        UIHelper.Warning(exception.Reason.ToString());
                    }
                    catch (Exception exception)
                    {
                        UIHelper.Warning(exception.Message);
                    }
                }
            }
        }
    }
}