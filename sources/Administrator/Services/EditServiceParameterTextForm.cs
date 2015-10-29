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

namespace Queue.Administrator
{
    public partial class EditServiceParameterTextForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly TaskPool taskPool;
        private readonly Guid serviceId;
        private readonly Guid serviceParameterTextId;
        private Service service;
        private ServiceParameterText serviceParameterText;

        #endregion fields

        #region properties

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

        #endregion properties

        public EditServiceParameterTextForm(Guid? serviceId, Guid? serviceParameterTextId = null)
        {
            InitializeComponent();

            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;
            this.serviceParameterTextId = serviceParameterTextId.HasValue
                ? serviceParameterTextId.Value : Guid.Empty;

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

        private void EditServiceParameterTextForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            ChannelManager.Dispose();
        }

        private async void EditServiceParameterTextForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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