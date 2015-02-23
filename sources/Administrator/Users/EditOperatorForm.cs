using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditOperatorForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private QueueOperator queueOperator;
        private Guid operatorId;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public EditOperatorForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? operatorId = null)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.operatorId = operatorId.HasValue
                ? operatorId.Value : Guid.Empty;

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
                isInterruptionCheckBox.Checked = queueOperator.IsInterruption;
                interruptionStartTimeTextBox.Text = queueOperator.InterruptionStartTime.ToString("hh\\:mm");
                interruptionFinishTimeTextBox.Text = queueOperator.InterruptionFinishTime.ToString("hh\\:mm");
            }
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

        #region bindings

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Surname = surnameTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Name = nameTextBox.Text;
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Patronymic = patronymicTextBox.Text;
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Email = emailTextBox.Text;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            queueOperator.Mobile = mobileTextBox.Text;
        }

        private void isActiveCheckBox_Leave(object sender, EventArgs e)
        {
            queueOperator.IsActive = isActiveCheckBox.Checked;
        }

        private void isInterruptionCheckBox_Leave(object sender, EventArgs e)
        {
            queueOperator.IsInterruption = isInterruptionCheckBox.Checked;
        }

        private void interruptionStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                queueOperator.InterruptionStartTime = TimeSpan.Parse(interruptionStartTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void interruptionFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                queueOperator.InterruptionFinishTime = TimeSpan.Parse(interruptionFinishTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
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

        private void workplaceControl_Leave(object sender, EventArgs e)
        {
            queueOperator.Workplace = workplaceControl.Selected<Workplace>();
        }
    }
}