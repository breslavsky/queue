using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class EditServiceRenderingForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Guid serviceRenderingId { get; set; }

        private ServiceRendering serviceRendering { get; set; }

        public ServiceRendering ServiceRendering
        {
            get { return serviceRendering; }
            private set
            {
                serviceRendering = value;

                operatorComboBox.SelectedItem = serviceRendering.Operator;
                serviceStepСomboBox.SelectedItem = serviceRendering.ServiceStep;
                modeСomboBox.SelectedItem = new EnumItem<ServiceRenderingMode>(serviceRendering.Mode);
                priorityUpDown.Value = serviceRendering.Priority;
            }
        }

        public EditServiceRenderingForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid serviceRenderingId)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceRenderingId = serviceRenderingId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            InitializeComponent();

            modeСomboBox.Items.AddRange(EnumItem<ServiceRenderingMode>.GetItems());
        }

        private async void EditServiceRenderingForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    var serviceRendering = await taskPool.AddTask(channel.Service.GetServiceRendering(serviceRenderingId));

                    var operators = await taskPool.AddTask(channel.Service.GetOperators());
                    operatorComboBox.Items.AddRange(operators);

                    //var serviceSteps = await taskPool.AddTask(channel.Service.GetServiceSteps(serviceId));
                    //operatorComboBox.Items.AddRange(operators);

                    ServiceRendering = serviceRendering;
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

        private void operatorComboBox_Leave(object sender, EventArgs e)
        {
            var selectedItem = operatorComboBox.SelectedItem as Operator;
            if (selectedItem != null)
            {
                ServiceRendering.Operator = selectedItem;
            }
        }

        private void serviceStepСomboBox_Leave(object sender, EventArgs e)
        {
            var selectedItem = serviceStepСomboBox.SelectedItem as ServiceStep;
            if (selectedItem != null)
            {
                ServiceRendering.ServiceStep = selectedItem;
            }
        }

        private void modeСomboBox_Leave(object sender, EventArgs e)
        {
            var selectedItem = modeСomboBox.SelectedItem as EnumItem<ServiceRenderingMode>;
            if (selectedItem != null)
            {
                ServiceRendering.Mode = selectedItem.Value;
            }
        }

        private void priorityUpDown_Leave(object sender, EventArgs e)
        {
            ServiceRendering.Priority = (byte)priorityUpDown.Value;
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    ServiceRendering = await taskPool.AddTask(channel.Service.EditServiceRendering(ServiceRendering));

                    DialogResult = DialogResult.OK;
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

        private void EditServiceRenderingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
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
    }
}