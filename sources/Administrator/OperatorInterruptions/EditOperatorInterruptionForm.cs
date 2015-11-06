using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditOperatorInterruptionForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IUserTcpService> ServerUserChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly Guid operatorInterruptionId;
        private readonly TaskPool taskPool;
        private OperatorInterruption operatorInterruption;

        #endregion fields

        #region properties

        public OperatorInterruption OperatorInterruption
        {
            get { return operatorInterruption; }
            private set
            {
                operatorInterruption = value;

                operatorControl.Select(operatorInterruption.Operator);
                typeControl.Select(operatorInterruption.Type);
                dayOfWeekControl.Select(operatorInterruption.DayOfWeek);
                startTimePicker.Value = operatorInterruption.StartTime;
                finishTimePicker.Value = operatorInterruption.FinishTime;

                AdjustType();
            }
        }

        #endregion properties

        public EditOperatorInterruptionForm(Guid? operatorInterruptionId = null)
            : base()
        {
            InitializeComponent();

            this.operatorInterruptionId = operatorInterruptionId.HasValue ?
                operatorInterruptionId.Value : Guid.Empty;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void AdjustType()
        {
            var type = typeControl.Selected<OperatorInterruptionType>();

            dayOfWeekControl.Enabled = type == OperatorInterruptionType.Weekday;
            targetDatePicker.Enabled = type == OperatorInterruptionType.TargetDate;
        }

        private void dayOfWeekControl_Leave(object sender, EventArgs e)
        {
            operatorInterruption.DayOfWeek = dayOfWeekControl.Selected<DayOfWeek>();
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
            if (ChannelManager != null)
            {
                ChannelManager.Dispose();
            }
        }

        private async void EditOperatorInterruptionForm_Load(object sender, EventArgs e)
        {
            typeControl.Initialize<OperatorInterruptionType>();
            dayOfWeekControl.Initialize<DayOfWeek>();

            try
            {
                Enabled = false;

                using (var channel = ServerUserChannelManager.CreateChannel())
                {
                    operatorControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(UserRole.Operator)));
                }

                if (operatorInterruptionId != Guid.Empty)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        OperatorInterruption = await taskPool.AddTask(channel.Service.GetOperatorInterruption(operatorInterruptionId));
                    }
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

        private void finishTimePicker_Leave(object sender, EventArgs e)
        {
            operatorInterruption.FinishTime = finishTimePicker.Value;
        }

        private void operatorControl_Leave(object sender, EventArgs e)
        {
            operatorInterruption.Operator = operatorControl.Selected<QueueOperator>();
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = ChannelManager.CreateChannel())
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

        private void startTimePicker_Leave(object sender, EventArgs e)
        {
            operatorInterruption.StartTime = startTimePicker.Value;
        }

        private void targetDatePicker_Leave(object sender, EventArgs e)
        {
            operatorInterruption.TargetDate = targetDatePicker.Value;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void typeControl_Leave(object sender, EventArgs e)
        {
            operatorInterruption.Type = typeControl.Selected<OperatorInterruptionType>();
        }

        private void typeControl_SelectedChanged(object sender, EventArgs e)
        {
            AdjustType();
        }
    }
}