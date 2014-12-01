using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class OfficesForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public OfficesForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void GridViewRenderRow(DataGridViewRow row, Office office)
        {
            row.Cells["nameColumn"].Value = office.Name;
            row.Tag = office;
        }

        private async void OfficesForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    foreach (var o in await taskPool.AddTask(channel.Service.GetOffices()))
                    {
                        GridViewRenderRow(gridView.Rows[gridView.Rows.Add()], o);
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

        private async void addOfficeButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addOfficeButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    var office = await taskPool.AddTask(channel.Service.AddOffice());

                    int index = gridView.Rows.Add();
                    var row = gridView.Rows[index];

                    row.Cells["nameColumn"].Value = office.Name;
                    row.Tag = office;
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
                finally
                {
                    addOfficeButton.Enabled = true;
                }
            }
        }

        private async void gridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = gridView.Rows[rowIndex];
                    var office = row.Tag as Office;

                    string name = row.Cells["nameColumn"].Value as string;
                    if (name != null)
                    {
                        office.Name = name;
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            row.ReadOnly = true;

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.EditOffice(office));
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
                        finally
                        {
                            row.ReadOnly = false;
                        }
                    }
                }
            }
        }

        private async void gridView_CellClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            int rowIndex = eventArgs.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = eventArgs.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = gridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    var office = row.Tag as Office;

                    var officeChannelManager = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(office.Endpoint));

                    switch (cell.OwningColumn.Name)
                    {
                        case "loginColumn":

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
                                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

                        case "manageColumn":

                            if (office.SessionId != Guid.Empty)
                            {
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

                        case "deleteColumn":

                            if (MessageBox.Show("Вы действительно хотите удалить филиал?",
                                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var channel = channelManager.CreateChannel())
                                {
                                    try
                                    {
                                        await channel.Service.OpenUserSession(currentUser.SessionId);
                                        await channel.Service.DeleteOffice(office.Id);

                                        gridView.Rows.Remove(row);
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
                            break;
                    }
                }
            }
        }

        private void OfficesForm_FormClosing(object sender, FormClosingEventArgs e)
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