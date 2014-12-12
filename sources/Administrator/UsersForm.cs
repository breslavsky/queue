using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueManager = Queue.Services.DTO.Manager;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class UsersForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public UsersForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
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

        private async void UsersForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    workplaceColumn.DisplayMember = "Key";
                    workplaceColumn.ValueMember = "Value";
                    workplaceColumn.DataSource = (await taskPool.AddTask(channel.Service.GetWorkplaces()))
                                                .Select(w => new KeyValuePair<string, Workplace>(w.ToString(), w))
                                                .ToList();

                    foreach (var user in await taskPool.AddTask(channel.Service.GetUsers()))
                    {
                        int index = usersGridView.Rows.Add();
                        var row = usersGridView.Rows[index];

                        RenderUsersGridRow(row, user);
                    }

                    UsersGridViewRefresh();
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

        private void RenderUsersGridRow(DataGridViewRow row, User user)
        {
            row.Cells["surnameColumn"].Value = user.Surname;
            row.Cells["nameColumn"].Value = user.Name;
            row.Cells["patronymicColumn"].Value = user.Patronymic;
            row.Cells["emailColumn"].Value = user.Email;
            row.Cells["mobileColumn"].Value = user.Mobile;
            if (user is QueueOperator)
            {
                var queueOperator = (QueueOperator)user;

                var workplace = queueOperator.Workplace;
                if (workplace != null)
                {
                    row.Cells["workplaceColumn"].Value = workplace;
                }

                row.Cells["isInterruptionColumn"].Value = queueOperator.IsInterruption;
                row.Cells["InterruptionStartTimeColumn"].Value = queueOperator.InterruptionStartTime.ToString("hh\\:mm\\:ss");
                row.Cells["InterruptionFinishTimeColumn"].Value = queueOperator.InterruptionFinishTime.ToString("hh\\:mm\\:ss");
            }
            row.Tag = user;
        }

        private void UsersGridViewRefresh()
        {
            int userRole = Convert.ToInt32(usersTabs.SelectedTab.Tag);
            switch ((UserRole)userRole)
            {
                case UserRole.Operator:
                    workplaceColumn.Visible = true;
                    isInterruptionColumn.Visible = true;
                    InterruptionStartTimeColumn.Visible = true;
                    InterruptionFinishTimeColumn.Visible = true;
                    break;

                default:
                    workplaceColumn.Visible = false;
                    isInterruptionColumn.Visible = false;
                    InterruptionStartTimeColumn.Visible = false;
                    InterruptionFinishTimeColumn.Visible = false;
                    break;
            }

            foreach (DataGridViewRow row in usersGridView.Rows)
            {
                row.Visible = false;
                switch ((UserRole)userRole)
                {
                    case UserRole.Administrator:
                        if (typeof(QueueAdministrator).IsInstanceOfType(row.Tag))
                        {
                            row.Visible = true;
                        }
                        break;

                    case UserRole.Manager:
                        if (typeof(QueueManager).IsInstanceOfType(row.Tag))
                        {
                            row.Visible = true;
                        }
                        break;

                    case UserRole.Operator:
                        if (typeof(QueueOperator).IsInstanceOfType(row.Tag))
                        {
                            row.Visible = true;
                        }
                        break;
                }
            }
        }

        private void usersTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            usersTableLayoutPanel.Parent = usersTabs.SelectedTab;
            UsersGridViewRefresh();
        }

        private async void usersGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = usersGridView.Rows[rowIndex];
                    var user = row.Tag as User;

                    string surname = row.Cells["surnameColumn"].Value as string;
                    if (surname != null)
                    {
                        user.Surname = surname;
                    }

                    string name = row.Cells["nameColumn"].Value as string;
                    if (name != null)
                    {
                        user.Name = name;
                    }

                    string patronymic = row.Cells["patronymicColumn"].Value as string;
                    if (patronymic != null)
                    {
                        user.Patronymic = patronymic;
                    }

                    string email = row.Cells["emailColumn"].Value as string;
                    if (email != null)
                    {
                        user.Email = email;
                    }

                    string mobile = row.Cells["mobileColumn"].Value as string;
                    if (mobile != null)
                    {
                        user.Mobile = mobile;
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            if (user is QueueOperator)
                            {
                                var queueOperator = user as QueueOperator;

                                var workplace = row.Cells["workplaceColumn"].Value as Workplace;
                                if (workplace != null)
                                {
                                    queueOperator.Workplace = workplace;
                                }

                                queueOperator.IsInterruption = (bool)row.Cells["isInterruptionColumn"].Value;

                                string interruptionStartTime = row.Cells["InterruptionStartTimeColumn"].Value as string;
                                if (interruptionStartTime != null)
                                {
                                    try
                                    {
                                        queueOperator.InterruptionStartTime = TimeSpan.Parse(interruptionStartTime);
                                    }
                                    catch
                                    {
                                        UIHelper.Warning("Время начала перерыва указано не верно");
                                        return;
                                    }
                                }

                                string interruptionFinishTime = row.Cells["InterruptionFinishTimeColumn"].Value as string;
                                if (interruptionFinishTime != null)
                                {
                                    try
                                    {
                                        queueOperator.InterruptionFinishTime = TimeSpan.Parse(interruptionFinishTime);
                                    }
                                    catch
                                    {
                                        UIHelper.Warning("Время окончания перерыва указано не верно");
                                        return;
                                    }
                                }

                                await taskPool.AddTask(channel.Service.EditOperator(queueOperator));
                            }
                            else
                            {
                                await taskPool.AddTask(channel.Service.EditUser(user));
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
            }
        }

        private async void usersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = usersGridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    User user = row.Tag as User;

                    switch (cell.OwningColumn.Name)
                    {
                        case "passwordColumn":
                            using (PasswordForm passwordForm = new PasswordForm())
                            {
                                if (passwordForm.ShowDialog() == DialogResult.OK)
                                {
                                    using (var channel = channelManager.CreateChannel())
                                    {
                                        try
                                        {
                                            row.ReadOnly = true;

                                            await taskPool.AddTask(channel.Service.ChangeUserPassword(user.Id, passwordForm.Password));
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
                            break;

                        case "deleteColumn":

                            if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "Подтвердите удаление",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var channel = channelManager.CreateChannel())
                                {
                                    try
                                    {
                                        await taskPool.AddTask(channel.Service.DeleteUser(user.Id));

                                        usersGridView.Rows.Remove(row);
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

        private async void addUserButton_Click(object sender, EventArgs e)
        {
            int userRole = Convert.ToInt32(usersTabs.SelectedTab.Tag);

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var user = await taskPool.AddTask(channel.Service.AddUser((UserRole)userRole));

                    int index = usersGridView.Rows.Add();
                    var row = usersGridView.Rows[index];

                    RenderUsersGridRow(row, user);
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

        private void UsersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }
    }
}