using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class EditAdministratorForm : RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private QueueAdministrator administrator;
        private Guid administratorId;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public EditAdministratorForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? administratorId = null)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.administratorId = administratorId.HasValue
                ? administratorId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            permissionsFlagsControl.Initialize<AdministratorPermissions>();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        public QueueAdministrator Administrator
        {
            get { return administrator; }
            private set
            {
                administrator = value;

                surnameTextBox.Text = administrator.Surname;
                nameTextBox.Text = administrator.Name;
                patronymicTextBox.Text = administrator.Patronymic;
                emailTextBox.Text = administrator.Email;
                mobileTextBox.Text = administrator.Mobile;
                isActiveCheckBox.Checked = administrator.IsActive;
                permissionsFlagsControl.Select<AdministratorPermissions>(administrator.Permissions);
            }
        }

        private void EditAdministratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditAdministratorForm_Load(object sender, EventArgs e)
        {
            if (administratorId != Guid.Empty)
            {
                Enabled = false;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        Administrator = await taskPool.AddTask(channel.Service.GetUser(administratorId)) as QueueAdministrator;

                        Enabled = true;
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
                Administrator = new QueueAdministrator()
                {
                    Surname = "Новый пользователь",
                    IsActive = true
                };
            }
        }

        #region bindings

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Surname = surnameTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Name = nameTextBox.Text;
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Patronymic = patronymicTextBox.Text;
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Email = emailTextBox.Text;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Mobile = mobileTextBox.Text;
        }

        private void isActiveCheckBox_Leave(object sender, EventArgs e)
        {
            administrator.IsActive = isActiveCheckBox.Checked;
        }

        private void permissionsFlagsControl_Leave(object sender, EventArgs e)
        {
            administrator.Permissions = permissionsFlagsControl.Selected<AdministratorPermissions>();
        }

        #endregion bindings

        private async void passwordButton_Click(object sender, EventArgs e)
        {
            using (var f = new PasswordForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            passwordButton.Enabled = false;

                            await taskPool.AddTask(channel.Service.ChangeUserPassword(administrator.Id, f.Password));
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
                            passwordButton.Enabled = true;
                        }
                    }
                }
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    Administrator = await taskPool.AddTask(channel.Service.EditAdministrator(administrator));

                    if (Saved != null)
                    {
                        Saved(this, EventArgs.Empty);
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
                finally
                {
                    saveButton.Enabled = true;
                }
            }
        }
    }
}