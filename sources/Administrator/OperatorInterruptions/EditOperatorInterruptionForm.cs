using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditOperatorInterruptionForm : RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private OperatorInterruption operatorInterruption;
        private Guid operatorInterruptionId;
        private TaskPool taskPool;

        public OperatorInterruption OperatorInterruption
        {
            get { return operatorInterruption; }
            private set
            {
                operatorInterruption = value;

                operatorControl.Select(operatorInterruption.Operator);
                dayOfWeekControl.Select(operatorInterruption.DayOfWeek);
                startTimePicker.Value = operatorInterruption.StartTime;
                finishTimePicker.Value = operatorInterruption.FinishTime;
            }
        }

        public EditOperatorInterruptionForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? operatorInterruptionId = null)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            this.operatorInterruptionId = operatorInterruptionId.HasValue ?
                operatorInterruptionId.Value : Guid.Empty;

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

        private void EditAdditionalServiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskPool.Cancel();

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

        private async void EditOperatorInterruptionForm_Load(object sender, EventArgs e)
        {
            dayOfWeekControl.Initialize<DayOfWeek>();

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    Enabled = false;

                    operatorControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(UserRole.Operator)));

                    if (operatorInterruptionId != Guid.Empty)
                    {
                        OperatorInterruption = await taskPool.AddTask(channel.Service.GetOperatorInterruption(operatorInterruptionId));
                    }
                    else
                    {
                        OperatorInterruption = new OperatorInterruption()
                        {
                            StartTime = new TimeSpan(12, 0, 0),
                            FinishTime = new TimeSpan(13, 0, 0)
                        };
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
                    Enabled = true;
                }
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    OperatorInterruption = await taskPool.AddTask(channel.Service.EditOperatorInterruption(operatorInterruption));

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

        private void operatorControl_Leave(object sender, EventArgs e)
        {
            operatorInterruption.Operator = operatorControl.Selected<QueueOperator>();
        }

        private void dayOfWeekControl_Leave(object sender, EventArgs e)
        {
            operatorInterruption.DayOfWeek = dayOfWeekControl.Selected<DayOfWeek>();
        }

        private void startTimePicker_Leave(object sender, EventArgs e)
        {
            operatorInterruption.StartTime = startTimePicker.Value;
        }

        private void finishTimePicker_Leave(object sender, EventArgs e)
        {
            operatorInterruption.FinishTime = finishTimePicker.Value;
        }
    }
}