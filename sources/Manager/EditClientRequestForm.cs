﻿using Junte.Parallel.Common;
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
using Translation = Queue.Model.Common.Translation;

namespace Queue.Manager
{
    public partial class EditClientRequestForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;
        private Guid clientRequestId;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private ClientRequest clientRequest;

        public EditClientRequestForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser, Guid clientRequestId)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.clientRequestId = clientRequestId;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
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

                    var translation1 = Translation.ClientRequestType.ResourceManager;
                    typeTextBlock.Text = translation1.GetString(clientRequest.Type.ToString());

                    clientTextBlock.Text = clientRequest.Client != null ? clientRequest.Client.ToString() : string.Empty;

                    foreach (var parameter in clientRequest.Parameters)
                    {
                        var index = parametersGridView.Rows.Add();
                        var row = parametersGridView.Rows[index];
                        row.Cells["parameterNameColumn"].Value = parameter.Name;
                        row.Cells["parameterValueColumn"].Value = parameter.Value;
                    }

                    var service = clientRequest.Service;
                    serviceTextBlock.Text = service.ToString();

                    var translation2 = Translation.ClientRequestState.ResourceManager;
                    stateTextBlock.Text = translation2.GetString(clientRequest.State.ToString());
                    stateTextBlock.BackColor = ColorTranslator.FromHtml(clientRequest.Color);

                    operatorsComboBox.SelectedValueChanged -= operatorsComboBox_SelectedValueChanged;
                    var queueOperator = clientRequest.Operator;
                    operatorsComboBox.SelectedValue = queueOperator != null ? queueOperator.Id : Guid.Empty;
                    operatorsComboBox.SelectedValueChanged += operatorsComboBox_SelectedValueChanged;

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
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

                    if (clientRequest.IsEditable)
                    {
                        isPriorityCheckBox.Enabled = true;
                        subjectsPanel.Enabled = true;
                        clientEditLink.Enabled = clientRequest.Client != null;
                        operatorsComboBox.Enabled = true;
                        serviceChangeLink.Enabled = true;
                        postponeMinutesUpDown.Enabled = true;
                        postponeButton.Enabled = true;
                        cancelMenuItem.Enabled = true;
                    }
                    else
                    {
                        isPriorityCheckBox.Enabled = false;
                        subjectsPanel.Enabled = false;
                        clientEditLink.Enabled = false;
                        operatorsComboBox.Enabled = false;
                        serviceChangeLink.Enabled = false;
                        postponeMinutesUpDown.Enabled = false;
                        postponeButton.Enabled = false;
                        cancelMenuItem.Enabled = false;
                    }

                    restoreMenuItem.Enabled = clientRequest.IsRestorable;
                }));
            }
        }

        private async void EditClientRequestForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    var operators = await taskPool.AddTask(channel.Service.GetUserList(UserRole.Operator));
                    operators.Add(Guid.Empty, "[не назначен]");

                    operatorsComboBox.DisplayMember = DataListItem.Value;
                    operatorsComboBox.ValueMember = DataListItem.Key;
                    operatorsComboBox.DataSource = new BindingSource(operators.Reverse(), null);

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

        private async void isPriorityCheckBox_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    isPriorityCheckBox.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    ClientRequest = await taskPool.AddTask(channel.Service.ChangeClientRequestPriority(clientRequestId, isPriorityCheckBox.Checked));
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
                    isPriorityCheckBox.Enabled = true;
                }
            }
        }

        private async void subjectsChangeButton_Click(object sender, EventArgs e)
        {
            var subjects = (int)subjectsUpDown.Value;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    subjectsChangeButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    ClientRequest = await taskPool.AddTask(channel.Service.ChangeClientRequestSubjects(clientRequestId, subjects));
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
                    subjectsChangeButton.Enabled = true;
                }
            }
        }

        private void clientEditLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new EditClientForm(channelBuilder, currentUser, clientRequest.Client))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    clientTextBlock.Text = f.Client.ToString();
                }
            }
        }

        private async void operatorsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var operatorId = (Guid)operatorsComboBox.SelectedValue;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorsComboBox.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    ClientRequest = await taskPool.AddTask(channel.Service.ChangeClientRequestOperator(clientRequestId, operatorId));
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
                    operatorsComboBox.Enabled = true;
                }
            }
        }

        private async void serviceChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new SelectServiceForm(channelBuilder, currentUser))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    if (f.SelectedService != null)
                    {
                        var selectedServiceId = f.SelectedService.Id;

                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                serviceChangeLink.Enabled = false;

                                await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                                ClientRequest = await taskPool.AddTask(channel.Service.ChangeClientRequestService(clientRequestId, selectedServiceId));
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
                                serviceChangeLink.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private async void postponeButton_Click(object sender, EventArgs e)
        {
            var postponeTime = TimeSpan.FromMinutes((int)postponeMinutesUpDown.Value);

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    postponeButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    ClientRequest = await taskPool.AddTask(channel.Service.PostponeClientRequest(clientRequestId, postponeTime));
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
                    postponeButton.Enabled = true;
                }
            }
        }

        private async void cancelMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    cancelMenuItem.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

        private async void restoreMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    restoreMenuItem.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

        private void EditClientRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }
    }
}