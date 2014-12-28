using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class OfficesForm : Queue.UI.WinForms.RichForm
    {
        private const string LoginColumn = "loginColumn";
        private const string ManageColumn = "manageColumn";

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public OfficesForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            InitializeComponent();
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

        private void addOfficeButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditOfficeForm(channelBuilder, currentUser))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = officesGridView.Rows[officesGridView.Rows.Add()];
                    }

                    OfficesGridViewRenderRow(row, f.Office);
                };

                f.ShowDialog();
            }
        }

        private async void officeGridView_CellClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            int rowIndex = eventArgs.RowIndex,
                columnIndex = eventArgs.ColumnIndex;
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = officesGridView.Rows[rowIndex];
                var cell = row.Cells[columnIndex];

                var office = row.Tag as Office;

                switch (cell.OwningColumn.Name)
                {
                    case LoginColumn:

                        using (var loginForm = new LoginForm(UserRole.Administrator))
                        {
                            loginForm.Endpoint = office.Endpoint;

                            if (loginForm.ShowDialog() == DialogResult.OK)
                            {
                                var user = loginForm.User;

                                office.Endpoint = loginForm.Endpoint;
                                office.SessionId = user.SessionId;

                                using (var channel = channelManager.CreateChannel())
                                {
                                    try
                                    {
                                        row.Tag = await taskPool.AddTask(channel.Service.EditOffice(office));

                                        UIHelper.Information("Вход успешно выполнен. Управление доступно.");
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
                        break;

                    case ManageColumn:

                        if (office.SessionId != Guid.Empty)
                        {
                            var officeChannelManager = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(),
                                Bindings.NetTcpBinding, new EndpointAddress(office.Endpoint));

                            using (var officeChannel = officeChannelManager.CreateChannel())
                            {
                                try
                                {
                                    var user = await taskPool.AddTask(officeChannel.Service.OpenUserSession(office.SessionId));
                                    var form = new AdministratorForm(officeChannelManager, user);
                                    FormClosing += (s, e) =>
                                    {
                                        form.Close();
                                    };
                                    form.Show();
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
                        else
                        {
                            UIHelper.Warning("Вход не выполнен");
                        }
                        break;
                }
            }
        }

        private void OfficesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void OfficesForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    foreach (var o in await taskPool.AddTask(channel.Service.GetOffices()))
                    {
                        var row = officesGridView.Rows[officesGridView.Rows.Add()];
                        OfficesGridViewRenderRow(row, o);
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

        private void officesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = officesGridView.Rows[rowIndex];
                Office office = row.Tag as Office;

                using (var f = new EditOfficeForm(channelBuilder, currentUser, office.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        OfficesGridViewRenderRow(row, f.Office);
                    };

                    f.ShowDialog();
                }
            }
        }

        private async void officesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить филиал?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Office office = e.Row.Tag as Office;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteOffice(office.Id));
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
            else
            {
                e.Cancel = true;
            }
        }

        private void OfficesGridViewRenderRow(DataGridViewRow row, Office office)
        {
            row.Cells["nameColumn"].Value = office.Name;
            row.Tag = office;
        }
    }
}