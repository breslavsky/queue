using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class UsersForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public UsersForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
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

        private void addUserButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (usersTabs.SelectedTab.Equals(operatorsTabPage))
            {
                using (var f = new EditOperatorForm(channelBuilder, currentUser))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        if (row == null)
                        {
                            row = usersGridView.Rows[usersGridView.Rows.Add()];
                        }
                        RenderUsersGridRow(row, f.Operator);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
            else if (usersTabs.SelectedTab.Equals(administratorsTabPage))
            {
                using (var f = new EditAdministratorForm(channelBuilder, currentUser))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        if (row == null)
                        {
                            row = usersGridView.Rows[usersGridView.Rows.Add()];
                            row.Selected = true;
                        }
                        RenderUsersGridRow(row, f.Administrator);
                        f.Close();
                    };

                    f.ShowDialog();
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
            row.Cells["isActiveColumn"].Value = user.IsActive;

            var queueOperator = user as QueueOperator;

            if (queueOperator != null)
            {
                var workplace = queueOperator.Workplace;
                if (workplace != null)
                {
                    row.Cells["workplaceColumn"].Value = workplace;
                }
            }
            row.Tag = user;
        }

        private void UsersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void UsersForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    foreach (var user in await taskPool.AddTask(channel.Service.GetUsers()))
                    {
                        var row = usersGridView.Rows[usersGridView.Rows.Add()];
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

        private void usersGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = usersGridView.Rows[rowIndex];
                User user = row.Tag as User;

                if (user is QueueOperator)
                {
                    using (var f = new EditOperatorForm(channelBuilder, currentUser, user.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderUsersGridRow(row, f.Operator);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                }
                else if (user is QueueAdministrator)
                {
                    using (var f = new EditAdministratorForm(channelBuilder, currentUser, user.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderUsersGridRow(row, f.Administrator);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                }
            }
        }

        private async void usersGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "Подтвердите удаление",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                User user = e.Row.Tag as User;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteUser(user.Id));
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

        private void UsersGridViewRefresh()
        {
            if (usersTabs.SelectedTab.Equals(operatorsTabPage))
            {
                workplaceColumn.Visible = true;
            }
            else if (usersTabs.SelectedTab.Equals(administratorsTabPage))
            {
                workplaceColumn.Visible = false;
            }

            foreach (DataGridViewRow row in usersGridView.Rows)
            {
                row.Visible = false;

                User user = row.Tag as User;

                if (usersTabs.SelectedTab.Equals(operatorsTabPage)
                    && user is QueueOperator)
                {
                    row.Visible = true;
                }
                else if (usersTabs.SelectedTab.Equals(administratorsTabPage)
                    && user is QueueAdministrator)
                {
                    row.Visible = true;
                }
            }
        }

        private void usersTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            usersTableLayoutPanel.Parent = usersTabs.SelectedTab;
            UsersGridViewRefresh();
        }
    }
}