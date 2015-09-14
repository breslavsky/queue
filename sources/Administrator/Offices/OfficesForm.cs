using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class OfficesForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        [Dependency]
        public LoginSettings LoginSettings { get; set; }

        #endregion dependency

        #region fields

        private const string LoginColumn = "loginColumn";
        private const string ManageColumn = "manageColumn";

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public OfficesForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
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
            using (var f = new EditOfficeForm())
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = officesGridView.Rows[officesGridView.Rows.Add()];
                    }

                    OfficesGridViewRenderRow(row, f.Office);
                    f.Close();
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

                        using (var f = new OfficeLoginForm(office.Id))
                        {
                            if (f.ShowDialog() == DialogResult.OK)
                            {
                                office.Endpoint = f.Settings.Endpoint;
                                office.SessionId = f.SessionId;

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
                                    var administrator = await taskPool.AddTask(officeChannel.Service.OpenUserSession(office.SessionId));

                                    Process.Start(new ProcessStartInfo()
                                    {
                                        UseShellExecute = true,
                                        FileName = "Queue.Administrator.exe",
                                        Arguments = string.Format("--AutoLogin --Endpoint=\"{0}\" --SessionId={1}",
                                            LoginSettings.Endpoint, administrator.SessionId),
                                        WorkingDirectory = Environment.CurrentDirectory
                                    });
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

                using (var f = new EditOfficeForm(office.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        OfficesGridViewRenderRow(row, f.Office);
                        f.Close();
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}