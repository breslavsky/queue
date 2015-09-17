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
    public partial class EditServiceParameterOptionsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly Guid serviceId;
        private readonly Guid serviceParameterOptionsId;
        private readonly TaskPool taskPool;
        private Service service;
        private ServiceParameterOptions serviceParameterOptions;

        #endregion fields

        #region properties

        public ServiceParameterOptions ServiceParameterOptions
        {
            get { return serviceParameterOptions; }
            private set
            {
                serviceParameterOptions = value;

                nameTextBox.Text = serviceParameterOptions.Name;
                toolTipTextBox.Text = serviceParameterOptions.ToolTip;
                isRequireCheckBox.Checked = serviceParameterOptions.IsRequire;
                optionsTextBox.Text = serviceParameterOptions.Options;
                isMultipleCheckBox.Checked = serviceParameterOptions.IsMultiple;
            }
        }

        #endregion properties

        public EditServiceParameterOptionsForm(Guid? serviceId, Guid? serviceParameterOptionsId = null)
        {
            InitializeComponent();

            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;
            this.serviceParameterOptionsId = serviceParameterOptionsId.HasValue
                ? serviceParameterOptionsId.Value : Guid.Empty;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void EditServiceParameterOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditServiceParameterOptionsForm_Load(object sender, EventArgs e)
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

                    ServiceParameterOptions = serviceParameterOptionsId != Guid.Empty ?
                        await taskPool.AddTask(channel.Service.GetServiceParameter(serviceParameterOptionsId)) as ServiceParameterOptions
                        : new ServiceParameterOptions()
                        {
                            Service = service,
                            Name = "Новый параметр"
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

                    ServiceParameterOptions = await taskPool.AddTask(channel.Service.EditServiceParameterOptions(serviceParameterOptions));

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

        private void isMultipleCheckBox_Leave(object sender, EventArgs e)
        {
            serviceParameterOptions.IsMultiple = isMultipleCheckBox.Checked;
        }

        private void isRequireCheckBox_Leave(object sender, EventArgs e)
        {
            serviceParameterOptions.IsRequire = isRequireCheckBox.Checked;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterOptions.Name = nameTextBox.Text;
        }

        private void optionsTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterOptions.Options = optionsTextBox.Text;
        }

        private void toolTipTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterOptions.ToolTip = toolTipTextBox.Text;
        }

        #endregion bindings
    }
}