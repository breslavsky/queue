using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
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
    public partial class EditAdministratorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly Guid administratorId;

        private readonly TaskPool taskPool;
        private QueueAdministrator administrator;

        #endregion fields

        #region properties

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

        #endregion properties

        public EditAdministratorForm(Guid? administratorId = null)
        {
            InitializeComponent();

            this.administratorId = administratorId.HasValue
                ? administratorId.Value : Guid.Empty;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            permissionsFlagsControl.Initialize<AdministratorPermissions>();
        }

        private void EditAdministratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            ChannelManager.Dispose();
        }

        private async void EditAdministratorForm_Load(object sender, EventArgs e)
        {
            if (administratorId != Guid.Empty)
            {
                Enabled = false;

                using (var channel = ChannelManager.CreateChannel())
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

        private async void passwordButton_Click(object sender, EventArgs e)
        {
            using (var f = new PasswordForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #region bindings

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Email = emailTextBox.Text;
        }

        private void isActiveCheckBox_Leave(object sender, EventArgs e)
        {
            administrator.IsActive = isActiveCheckBox.Checked;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Mobile = mobileTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Name = nameTextBox.Text;
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Patronymic = patronymicTextBox.Text;
        }

        private void permissionsFlagsControl_Leave(object sender, EventArgs e)
        {
            administrator.Permissions = permissionsFlagsControl.Selected<AdministratorPermissions>();
        }

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            administrator.Surname = surnameTextBox.Text;
        }

        #endregion bindings
    }
}