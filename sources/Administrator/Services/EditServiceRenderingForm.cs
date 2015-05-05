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
    public partial class EditServiceRenderingForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Schedule schedule;
        private Guid scheduleId;
        private ServiceRendering serviceRendering;
        private Guid serviceRenderingId;
        private TaskPool taskPool;

        public EditServiceRenderingForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser,
            Guid scheduleId, Guid? serviceRenderingId = null)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.scheduleId = scheduleId;
            this.serviceRenderingId = serviceRenderingId.HasValue
                ? serviceRenderingId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            InitializeComponent();

            modeСontrol.Initialize<ServiceRenderingMode>();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        public ServiceRendering ServiceRendering
        {
            get { return serviceRendering; }
            private set
            {
                serviceRendering = value;

                operatorControl.Select<QueueOperator>(serviceRendering.Operator);
                serviceStepControl.Select<ServiceStep>(serviceRendering.ServiceStep);
                modeСontrol.Select<ServiceRenderingMode>(serviceRendering.Mode);
                priorityUpDown.Value = serviceRendering.Priority;
            }
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

        private void EditServiceRenderingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void EditServiceRenderingForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    schedule = await taskPool.AddTask(channel.Service.GetSchedule(scheduleId));
                    if (schedule is ServiceSchedule)
                    {
                        Service service = (schedule as ServiceSchedule).Service;
                        serviceStepControl.Initialize(await taskPool.AddTask(channel.Service.GetServiceStepLinks(service.Id)));
                    }

                    operatorControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(UserRole.Operator)));

                    ServiceRendering = serviceRenderingId != Guid.Empty ?
                        await taskPool.AddTask(channel.Service.GetServiceRendering(serviceRenderingId))
                        : new ServiceRendering()
                        {
                            Schedule = schedule
                        };
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

        #region bindings

        private void operatorControl_Leave(object sender, EventArgs e)
        {
            serviceRendering.Operator = operatorControl.Selected<QueueOperator>();
        }

        private void priorityUpDown_Leave(object sender, EventArgs e)
        {
            serviceRendering.Priority = (byte)priorityUpDown.Value;
        }

        private void serviceStepControl_Leave(object sender, EventArgs e)
        {
            serviceRendering.ServiceStep = serviceStepControl.Selected<ServiceStep>();
        }

        private void modeСontrol_Leave(object sender, EventArgs e)
        {
            serviceRendering.Mode = modeСontrol.Selected<ServiceRenderingMode>();
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    ServiceRendering = await taskPool.AddTask(channel.Service.EditServiceRendering(serviceRendering));

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