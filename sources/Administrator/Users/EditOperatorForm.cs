using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditOperatorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly Guid operatorId;
        private readonly TaskPool taskPool;
        private QueueOperator queueOperator;

        #endregion fields

        #region properties

        public QueueOperator Operator
        {
            get { return queueOperator; }
            private set
            {
                queueOperator = value;

                surnameTextBox.Text = queueOperator.Surname;
                nameTextBox.Text = queueOperator.Name;
                patronymicTextBox.Text = queueOperator.Patronymic;
                emailTextBox.Text = queueOperator.Email;
                mobileTextBox.Text = queueOperator.Mobile;
                isActiveCheckBox.Checked = queueOperator.IsActive;
                workplaceControl.Select<Workplace>(queueOperator.Workplace);
            }
        }

        #endregion properties

        public EditOperatorForm(Guid? operatorId = null)
        {
            InitializeComponent();

            this.operatorId = operatorId.HasValue
                ? operatorId.Value : Guid.Empty;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void EditOperatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditOperatorForm_Load(object sender, EventArgs e)
        {
            if (operatorId != Guid.Empty)
            {
                Enabled = false;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        workplaceControl.Initialize(await taskPool.AddTask(channel.Service.GetWorkplacesLinks()));

                        Operator = await taskPool.AddTask(channel.Service.GetUser(operatorId)) as QueueOperator;

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
                Operator = new QueueOperator()
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
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            passwordButton.Enabled = false;

                            await taskPool.AddTask(channel.Service.ChangeUserPassword(queueOperator.Id, f.Password));
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

                    Operator = await taskPool.AddTask(channel.Service.EditOperator(queueOperator));

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

        private void workplaceControl_Leave(object sender, EventArgs e)
        {
            queueOperator.Workplace = workplaceControl.Selected<Workplace>();
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Email = emailTextBox.Text;
        }

        private void isActiveCheckBox_Leave(object sender, EventArgs e)
        {
            queueOperator.IsActive = isActiveCheckBox.Checked;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Mobile = mobileTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Name = nameTextBox.Text;
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Patronymic = patronymicTextBox.Text;
        }

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Surname = surnameTextBox.Text;
        }

        #endregion bindings
    }
}