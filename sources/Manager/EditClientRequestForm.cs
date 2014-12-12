using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Manager
{
    public partial class EditClientRequestForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

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

            typeControl.Initialize<ClientRequestType>();
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
                    requestDateTextBlock.Text = clientRequest.RequestDate.ToShortDateString();
                    requestTimeTextBlock.Text = clientRequest.RequestTime.ToString("hh\\:mm\\:ss");
                    subjectsUpDown.Value = clientRequest.Subjects;
                    typeControl.Select<ClientRequestType>(clientRequest.Type);
                    clientTextBlock.Text = clientRequest.Client != null ? clientRequest.Client.ToString() : string.Empty;

                    var service = clientRequest.Service;
                    serviceTextBlock.Text = service.ToString();

                    serviceTypeTextBlock.Text = clientRequest.ServiceType.Translate();
                    stateTextBlock.Text = clientRequest.State.Translate();
                    stateTextBlock.BackColor = ColorTranslator.FromHtml(clientRequest.Color);

                    operatorControl.Select<QueueOperator>(clientRequest.Operator);

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            serviceStepControl.Initialize<ServiceStep>(await channel.Service.GetServiceStepList(service.Id));
                            serviceStepControl.Select<ServiceStep>(clientRequest.ServiceStep);

                            eventsGridView.Rows.Clear();

                            var events = await taskPool.AddTask(channel.Service.GetClientRequestEvents(clientRequestId));
                            foreach (var e in events.Reverse())
                            {
                                int index = eventsGridView.Rows.Add();
                                var row = eventsGridView.Rows[index];
                                row.Cells["createDateColumn"].Value = e.CreateDate.ToString();
                                row.Cells["messageColumn"].Value = e.Message;
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
                    }

                    foreach (var parameter in clientRequest.Parameters)
                    {
                        var index = parametersGridView.Rows.Add();
                        var row = parametersGridView.Rows[index];
                        row.Cells["parameterNameColumn"].Value = parameter.Name;
                        row.Cells["parameterValueColumn"].Value = parameter.Value;
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
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorControl.Initialize<QueueOperator>(await taskPool.AddTask(channel.Service.GetOperators()));

                    ClientRequest = await taskPool.AddTask(channel.Service.GetClientRequest(clientRequestId));
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

                    var coupon = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequestId));
                    var xmlReader = new XmlTextReader(new StringReader(coupon));
                    var grid = (Grid)XamlReader.Load(xmlReader);

                    var xpsFile = Path.GetTempFileName() + ".xps";

                    using (var container = Package.Open(xpsFile, FileMode.Create))
                    {
                        using (var document = new XpsDocument(container, CompressionOption.SuperFast))
                        {
                            var fixedPage = new FixedPage();
                            fixedPage.Children.Add(grid);

                            var pageConent = new PageContent();
                            ((IAddChild)pageConent).AddChild(fixedPage);

                            var fixedDocument = new FixedDocument();
                            fixedDocument.Pages.Add(pageConent);

                            var xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(document);
                            xpsDocumentWriter.Write(fixedDocument);
                        }
                    }

                    PrintQueue printQueue;
                    try
                    {
                        printQueue = new PrintServer().GetPrintQueue(settings.DefaultPrintQueue);
                    }
                    catch
                    {
                        UIHelper.Warning("Ошибка получения принтера, будет использован принтер по умолчанию");
                        printQueue = LocalPrintServer.GetDefaultPrintQueue();
                    }
                    printQueue.AddJob(xpsFile, xpsFile, false);
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

                    clientRequest = await taskPool.AddTask(channel.Service.EditClientRequest(clientRequest));

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
    }
}