using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditServiceParameterTextForm : RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private Guid serviceId;
        private ServiceParameterText serviceParameterText;
        private Guid serviceParameterTextId;
        private TaskPool taskPool;

        public EditServiceParameterTextForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser,
            Guid? serviceId, Guid? serviceParameterTextId = null)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;
            this.serviceParameterTextId = serviceParameterTextId.HasValue
                ? serviceParameterTextId.Value : Guid.Empty;

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

        public ServiceParameterText ServiceParameterText
        {
            get { return serviceParameterText; }
            private set
            {
                serviceParameterText = value;

                nameTextBox.Text = serviceParameterText.Name;
                toolTipTextBox.Text = serviceParameterText.ToolTip;
                isRequireCheckBox.Checked = serviceParameterText.IsRequire;
                parameterMinLengthUpDown.Value = serviceParameterText.MinLength;
                parameterMaxLengthUpDown.Value = serviceParameterText.MaxLength;
            }
        }

        private void EditServiceParameterTextForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditServiceParameterTextForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    if (serviceId != Guid.Empty)
                    {
                        service = await taskPool.AddTask(channel.Service.GetService(serviceId));
                    }

                    ServiceParameterText = serviceParameterTextId != Guid.Empty ?
                        await taskPool.AddTask(channel.Service.GetServiceParameter(serviceParameterTextId)) as ServiceParameterText
                        : new ServiceParameterText()
                        {
                            Service = service,
                            Name = "Новый параметр",
                            MinLength = 1,
                            MaxLength = 30
                        };

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

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    ServiceParameterText = await taskPool.AddTask(channel.Service.EditServiceParameterText(serviceParameterText));

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

        #region bindings

        private void isRequireCheckBox_Leave(object sender, EventArgs e)
        {
            serviceParameterText.IsRequire = isRequireCheckBox.Checked;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterText.Name = nameTextBox.Text;
        }

        private void toolTipTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterText.ToolTip = toolTipTextBox.Text;
        }

        private void parameterMinLengthUpDown_Leave(object sender, EventArgs e)
        {
            serviceParameterText.MinLength = (int)parameterMinLengthUpDown.Value;
        }

        private void parameterMaxLengthUpDown_Leave(object sender, EventArgs e)
        {
            serviceParameterText.MaxLength = (int)parameterMaxLengthUpDown.Value;
        }

        #endregion bindings
    }
}