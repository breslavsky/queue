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
    public partial class EditServiceParameterNumberForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private Guid serviceId;
        private ServiceParameterNumber serviceParameterNumber;
        private Guid serviceParameterNumberId;
        private TaskPool taskPool;

        public EditServiceParameterNumberForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser,
            Guid? serviceId, Guid? serviceParameterNumberId = null)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;
            this.serviceParameterNumberId = serviceParameterNumberId.HasValue
                ? serviceParameterNumberId.Value : Guid.Empty;

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

        public ServiceParameterNumber ServiceParameterNumber
        {
            get { return serviceParameterNumber; }
            private set
            {
                serviceParameterNumber = value;

                nameTextBox.Text = serviceParameterNumber.Name;
                toolTipTextBox.Text = serviceParameterNumber.ToolTip;
                isRequireCheckBox.Checked = serviceParameterNumber.IsRequire;
            }
        }

        private void EditServiceParameterNumberForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditServiceParameterNumberForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    if (serviceId != Guid.Empty)
                    {
                        service = await taskPool.AddTask(channel.Service.GetService(serviceId));
                    }

                    ServiceParameterNumber = serviceParameterNumberId != Guid.Empty ?
                        await taskPool.AddTask(channel.Service.GetServiceParameter(serviceParameterNumberId)) as ServiceParameterNumber
                        : new ServiceParameterNumber()
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

                    ServiceParameterNumber = await taskPool.AddTask(channel.Service.EditServiceParameterNumber(serviceParameterNumber));

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
            serviceParameterNumber.IsRequire = isRequireCheckBox.Checked;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterNumber.Name = nameTextBox.Text;
        }

        private void toolTipTextBox_Leave(object sender, EventArgs e)
        {
            serviceParameterNumber.ToolTip = toolTipTextBox.Text;
        }

        #endregion bindings
    }
}