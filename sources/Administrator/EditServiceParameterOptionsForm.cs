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
    public partial class EditServiceParameterOptionsForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private Guid serviceId;
        private ServiceParameterOptions serviceParameterOptions;
        private Guid serviceParameterOptionsId;
        private TaskPool taskPool;

        public EditServiceParameterOptionsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser,
            Guid? serviceId, Guid? serviceParameterOptionsId = null)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;
            this.serviceParameterOptionsId = serviceParameterOptionsId.HasValue
                ? serviceParameterOptionsId.Value : Guid.Empty;

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

        private void EditServiceParameterOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditServiceParameterOptionsForm_Load(object sender, EventArgs e)
        {
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