using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.Common;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class EditClientRequestForm : UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private IConfigurationManager configuration;
        private AdministratorSettings settings;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private ClientRequest clientRequest;
        private Guid clientRequestId;
        private User currentUser;
        private TaskPool taskPool;

        public EditClientRequestForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid clientRequestId)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.clientRequestId = clientRequestId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            typeControl.Initialize<ClientRequestType>();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        public ClientRequest ClientRequest
        {
            get
            {
                return clientRequest;
            }
            private set
            {
                Invoke(new MethodInvoker(async () =>
                {
                    clientRequest = value;

                    numberTextBlock.Text = clientRequest.Number.ToString();
                    isPriorityCheckBox.Checked = clientRequest.IsPriority;
                    requestDatePicker.Value = clientRequest.RequestDate;
                    requestTimePicker.Value = clientRequest.RequestTime;
                    subjectsUpDown.Value = clientRequest.Subjects;
                    typeControl.Select(clientRequest.Type);
                    clientTextBlock.Text = clientRequest.Client != null ? clientRequest.Client.ToString() : string.Empty;

                    var service = clientRequest.Service;
                    serviceTextBlock.Text = service.ToString();

                    serviceTypeTextBlock.Text = Translater.Enum(clientRequest.ServiceType);
                    stateTextBlock.Text = Translater.Enum(clientRequest.State);
                    stateTextBlock.BackColor = ColorTranslator.FromHtml(clientRequest.Color);

                    operatorControl.Select(clientRequest.Operator);

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            serviceStepControl.Initialize(await channel.Service.GetServiceStepLinks(service.Id));
                            serviceStepControl.Select(clientRequest.ServiceStep);

                            eventsGridView.Rows.Clear();

                            var events = await taskPool.AddTask(channel.Service.GetClientRequestEvents(clientRequestId));
                            eventsBindingSource.DataSource = events.Reverse();

                            var parameters = await taskPool.AddTask(channel.Service.GetClientRequestParameters(clientRequestId));
                            parametersBindingSource.DataSource = parameters;

                            var additionalServices = await taskPool.AddTask(channel.Service.GetClientRequestAdditionalServices(clientRequestId));
                            additionalServicesBindingSource.DataSource = additionalServices;
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

                    clientEditLink.Enabled = clientRequest.Client != null;
                    editPanel.Enabled = clientRequest.IsEditable;

                    restoreMenuItem.Enabled = clientRequest.IsRestorable;
                }));
            }
        }

        private void EditClientRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void EditClientRequestForm_Load(object sender, EventArgs e)
        {
            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            settings = configuration.GetSection<AdministratorSettings>(AdministratorSettings.SectionKey);

            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(UserRole.Operator)));

                    ClientRequest = await taskPool.AddTask(channel.Service.GetClientRequest(clientRequestId));

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

        private void editPanel_EnabledChanged(object sender, EventArgs e)
        {
            cancelMenuItem.Enabled = editPanel.Enabled;
        }

        #region top menu

        private async void cancelMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    cancelMenuItem.Enabled = false;

                    ClientRequest = await taskPool.AddTask(channel.Service.CancelClientRequest(clientRequestId));
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
                    cancelMenuItem.Enabled = true;
                }
            }
        }

        private async void couponMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    couponMenuItem.Enabled = false;

                    ClientRequestCoupon data = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
                    CouponConfig config = await taskPool.AddTask(channel.Service.GetCouponConfig());
                    XPSUtils.PrintXaml(config.Template, data, settings.CouponPrinter);
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
                    couponMenuItem.Enabled = true;
                }
            }
        }

        private async void reportMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    reportMenuItem.Enabled = false;

                    byte[] report = await taskPool.AddTask(channel.Service.GetClientRequestReport(clientRequestId));
                    var path = Path.GetTempPath() + Path.GetRandomFileName() + ".xls";

                    var file = File.OpenWrite(path);
                    file.Write(report, 0, report.Length);
                    file.Close();

                    Process.Start(path);
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
                    reportMenuItem.Enabled = true;
                }
            }
        }

        private async void restoreMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    restoreMenuItem.Enabled = false;

                    ClientRequest = await taskPool.AddTask(channel.Service.RestoreClientRequest(clientRequestId));
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
                    restoreMenuItem.Enabled = true;
                }
            }
        }

        #endregion top menu

        #region bindings

        private void clientEditLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new EditClientForm(channelBuilder, currentUser, clientRequest.Client.Id))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    clientRequest.Client = f.Client;
                    clientTextBlock.Text = f.Client.ToString();
                }
            }
        }

        private void isPriorityCheckBox_Leave(object sender, EventArgs e)
        {
            clientRequest.IsPriority = isPriorityCheckBox.Checked;
        }

        private void operatorsControl_Leave(object sender, EventArgs e)
        {
            clientRequest.Operator = operatorControl.Selected<QueueOperator>();
        }

        private void requestDatePicker_Leave(object sender, EventArgs e)
        {
            clientRequest.RequestDate = requestDatePicker.Value;
        }

        private void requestTimePicker_Leave(object sender, EventArgs e)
        {
            clientRequest.RequestTime = requestTimePicker.Value;
        }

        private void serviceChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new SelectServiceForm(channelBuilder, currentUser))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    clientRequest.Service = f.Service;
                    serviceTextBlock.Text = clientRequest.Service.ToString();
                }
            }
        }

        private void serviceStepControl_Leave(object sender, EventArgs e)
        {
            clientRequest.ServiceStep = serviceStepControl.Selected<ServiceStep>();
        }

        private void subjectsUpDown_Leave(object sender, EventArgs e)
        {
            clientRequest.Subjects = (int)subjectsUpDown.Value;
        }

        private void typeControl_Leave(object sender, EventArgs e)
        {
            clientRequest.Type = typeControl.Selected<ClientRequestType>();
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    ClientRequest = await taskPool.AddTask(channel.Service.EditClientRequest(clientRequest));

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