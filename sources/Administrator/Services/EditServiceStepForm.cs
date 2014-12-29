using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditServiceStepForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private Guid serviceId;
        private ServiceStep serviceStep;
        private Guid serviceStepId;
        private TaskPool taskPool;

        public EditServiceStepForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid serviceId, Guid? serviceStepId = null)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceId = serviceId;
            this.serviceStepId = serviceStepId.HasValue ? serviceStepId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            InitializeComponent();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        public ServiceStep ServiceStep
        {
            get { return serviceStep; }
            private set
            {
                serviceStep = value;

                nameTextBox.Text = serviceStep.Name;
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

        private void EditServiceStepForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void EditServiceStepForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    service = await taskPool.AddTask(channel.Service.GetService(serviceId));

                    ServiceStep = serviceStepId != Guid.Empty
                        ? await taskPool.AddTask(channel.Service.GetServiceStep(serviceStepId))
                        : new ServiceStep()
                        {
                            Service = service,
                            Name = "Новый этап"
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
            }
        }

        #region bindings

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceStep.Name = nameTextBox.Text;
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    ServiceStep = await taskPool.AddTask(channel.Service.EditServiceStep(serviceStep));

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