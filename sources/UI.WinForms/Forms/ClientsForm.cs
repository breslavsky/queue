using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class ClientsForm : Queue.UI.WinForms.RichForm
    {
        private const byte PageSize = 50;
        private int startIndex = 0;

        private bool loaded = false;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public ClientsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            RefreshGridView();

            loaded = true;
        }

        private async void RefreshGridView()
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await Task.Delay(1000);
                    await channel.Service.OpenUserSession(currentUser.SessionId);
                    var clients = await taskPool.AddTask(channel.Service.FindClients(startIndex, PageSize, queryTextBox.Text.Trim()));

                    clientsGridView.Rows.Clear();
                    foreach (var c in clients)
                    {
                        int index = clientsGridView.Rows.Add();
                        var row = clientsGridView.Rows[index];

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

        private void resetButton_Click(object sender, EventArgs e)
        {
            startIndex = 0;
            RefreshGridView();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            startIndex -= PageSize;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            RefreshGridView();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (clientsGridView.Rows.Count == PageSize)
            {
                startIndex += PageSize;
            }
            RefreshGridView();
        }

        private void filterChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                startIndex = 0;
                RefreshGridView();
            }
        }

        private void clientsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = clientsGridView.Rows[rowIndex];
                Client client = row.Tag as Client;

                using (var f = new EditClientForm(channelBuilder, currentUser, client.Id))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        ClientsGridViewRenderRow(row, f.Client);
                    }
                }
            }
        }

        private void ClientsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
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
    }
}