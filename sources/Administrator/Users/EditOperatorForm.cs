using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditOperatorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<IWorkplaceTcpService> WorkplaceChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IUserTcpService> UserChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

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
                identityTextBox.Text = queueOperator.Identity;
                isMultisessionCheckBox.Checked = queueOperator.IsMultisession;
            }
        }

        #endregion properties

        public EditOperatorForm(Guid? operatorId = null)
        {
            InitializeComponent();

            this.operatorId = operatorId.HasValue
                ? operatorId.Value : Guid.Empty;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void EditOperatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
        }

        private async void EditOperatorForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            try
            {
                using (var channel = WorkplaceChannelManager.CreateChannel())
                {
                    workplaceControl.Initialize(await taskPool.AddTask(channel.Service.GetWorkplacesLinks()));
                }

                if (operatorId != Guid.Empty)
                {
                    using (var channel = UserChannelManager.CreateChannel())
                    {
                        Operator = await taskPool.AddTask(channel.Service.GetUser(operatorId)) as QueueOperator;
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

        private async void passwordButton_Click(object sender, EventArgs e)
        {
            using (var f = new PasswordForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        passwordButton.Enabled = false;

                        using (var channel = UserChannelManager.CreateChannel())
                        {
                            await taskPool.AddTask(channel.Service.ChangeUserPassword(queueOperator.Id, f.Password));
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
                        passwordButton.Enabled = true;
                    }
                }
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                saveButton.Enabled = false;

                using (var channel = UserChannelManager.CreateChannel())
                {
                    Operator = await taskPool.AddTask(channel.Service.EditOperator(queueOperator));
                }

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

        private void identityTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Identity = identityTextBox.Text;
        }

        private void isMultisessionCheckBox_Leave(object sender, EventArgs e)
        {
            queueOperator.IsMultisession = isMultisessionCheckBox.Checked;
        }

        #endregion bindings
    }
}