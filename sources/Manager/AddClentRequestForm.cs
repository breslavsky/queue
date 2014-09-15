using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SelectionMode = System.Windows.Forms.SelectionMode;

namespace Queue.Manager
{
    public partial class AddClentRequestForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        #region fields

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private string[] freeTimeReport;

        private Client currentClient;

        private Service selectedService;

        private string xpsCouponFile;

        #endregion fields

        #region properties

        private Client CurrentClient
        {
            get
            {
                return currentClient;
            }
            set
            {
                currentClient = value;

                if (currentClient != null)
                {
                    clientSurnameTextBox.Text = currentClient.Surname;
                    clientNameTextBox.Text = currentClient.Name;
                    clientPatronymicTextBox.Text = currentClient.Patronymic;
                    clientMobileTextBox.Text = currentClient.Mobile;

                    clientGroupBox.Enabled = false;
                }
                else
                {
                    clientSurnameTextBox.Text = string.Empty;
                    clientNameTextBox.Text = string.Empty;
                    clientPatronymicTextBox.Text = string.Empty;
                    clientMobileTextBox.Text = string.Empty;

                    clientsListBox.DataSource = null;
                    clientGroupBox.Enabled = true;
                }
            }
        }

        #endregion properties

        public AddClentRequestForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();

            clientsListBox.DisplayMember = string.Empty;
        }

        private void AddClientRequestForm_Load(object sender, EventArgs e)
        {
            foreach (var p in new PrintServer().GetPrintQueues(new[] {
                EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }))
            {
                printersComboBox.Items.Add(p.FullName);
            }

            printersComboBox.SelectedItem = string.IsNullOrWhiteSpace(settings.DefaultPrintQueue)
                    ? LocalPrintServer.GetDefaultPrintQueue().FullName
                    : settings.DefaultPrintQueue;

            earlyDatePicker.MinDate = earlyDatePicker.Value = ServerDateTime.Today;

            LoadServiceGroup(servicesTreeView.Nodes);
        }

        private async void LoadServiceGroup(TreeNodeCollection nodes, ServiceGroup serviceGroup = null)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var serviceGroups = serviceGroup != null
                        ? await taskPool.AddTask(channel.Service.GetServiceGroups(serviceGroup.Id))
                        : await taskPool.AddTask(channel.Service.GetRootServiceGroups());

                    foreach (var g in serviceGroups)
                    {
                        var node = new TreeNode()
                        {
                            Text = g.ToString(),
                            Checked = g.IsActive,
                            Tag = g
                        };

                        node.Nodes.Add(new TreeNode("загрузка...") { Tag = g });
                        nodes.Add(node);
                    }

                    var services = serviceGroup != null
                        ? await taskPool.AddTask(channel.Service.GetServices(serviceGroup.Id))
                        : await taskPool.AddTask(channel.Service.GetRootServices());

                    foreach (var s in services)
                    {
                        var node = new TreeNode()
                        {
                            Text = s.ToString(),
                            Tag = s
                        };
                        nodes.Add(node);
                    }

                    if (serviceGroup != null)
                    {
                        nodes.RemoveAt(0);
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
        }

        private void PrintCoupon()
        {
            if (!string.IsNullOrWhiteSpace(xpsCouponFile))
            {
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
                printQueue.AddJob(xpsCouponFile, xpsCouponFile, false);
            }
        }

        private async void LoadFreeTime()
        {
            addButton.Enabled = false;

            liveStatusLabel.Text = string.Empty;
            liveStatusLabel.Text = string.Empty;
            earlyStatusLabel.Text = string.Empty;
            earlyStatusLabel.Text = string.Empty;

            if (selectedService != null)
            {
                if (liveRadioButton.Checked)
                {
                    if (selectedService.LiveRegistrator.HasFlag(ClientRequestRegistrator.Manager))
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                var freeTime = await channel.Service.GetFreeTime(selectedService.Id, ServerDateTime.Now, ClientRequestType.Live);
                                var timeIntervals = freeTime.TimeIntervals;

                                liveStatusLabel.Text = timeIntervals.Length > 0
                                    ? string.Format("Доступно {0}", freeTime.FreeTimeIntervals)
                                    : "Нет свободного времени";

                                subjectsUpDown.Maximum = Math.Min(selectedService.MaxSubjects, timeIntervals.Length);
                                subjectsUpDown.Value = Math.Min(1, subjectsUpDown.Maximum);

                                if (timeIntervals.Length > 0)
                                {
                                    liveStatusLabel.Text = string.Format("Доступно {0}", freeTime.FreeTimeIntervals);
                                    addButton.Enabled = true;
                                }
                                else
                                {
                                    liveStatusLabel.Text = "Нет свободного времени";
                                }

                                freeTimeReport = freeTime.Report;
                            }
                            catch (FaultException exception)
                            {
                                liveStatusLabel.Text = exception.Reason.ToString();
                            }
                            catch (Exception exception)
                            {
                                UIHelper.Warning(exception.Message);
                            }
                        }
                    }
                    else
                    {
                        liveStatusLabel.Text = "Отключено администратором";
                    }
                }

                if (earlyRadioButton.Checked && earlyDatePicker.Value != null)
                {
                    var selectedDate = earlyDatePicker.Value;

                    if (selectedService.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Manager))
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                freeTimeComboBox.Enabled = false;

                                var freeTime = (await taskPool.AddTask(channel.Service
                                    .GetFreeTime(selectedService.Id, selectedDate, ClientRequestType.Early)));

                                freeTimeComboBox.Items.Clear();

                                var timeIntervals = freeTime.TimeIntervals;
                                if (timeIntervals.Length > 0)
                                {
                                    freeTimeComboBox.Enabled = true;

                                    Array.Sort(timeIntervals);

                                    foreach (var timeInterval in timeIntervals.Distinct())
                                    {
                                        freeTimeComboBox.Items.Add(timeInterval);
                                    }
                                    freeTimeComboBox.SelectedIndex = 0;

                                    earlyStatusLabel.Text = string.Format("Доступно {0}", freeTime.FreeTimeIntervals);
                                    addButton.Enabled = true;
                                }
                                else
                                {
                                    earlyStatusLabel.Text = "Нет свободного времени";
                                }

                                subjectsUpDown.Maximum = timeIntervals.Length;
                                subjectsUpDown.Value = Math.Min(1, subjectsUpDown.Maximum);

                                freeTimeReport = freeTime.Report;
                            }
                            catch (OperationCanceledException) { }
                            catch (CommunicationObjectAbortedException) { }
                            catch (ObjectDisposedException) { }
                            catch (InvalidOperationException) { }
                            catch (FaultException exception)
                            {
                                earlyStatusLabel.Text = exception.Reason.ToString();
                            }
                            catch (Exception exception)
                            {
                                UIHelper.Warning(exception.Message);
                            }
                        }
                    }
                    else
                    {
                        earlyStatusLabel.Text = "Отключено администратором";
                    }
                }
            }
        }

        private async void clientSurnameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var surname = clientSurnameTextBox.Text;

            if (!string.IsNullOrWhiteSpace(surname))
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                        var clients = await taskPool.AddTask(channel.Service.FindClients(0, 10, surname));

                        clientsListBox.SelectionMode = SelectionMode.None;
                        clientsListBox.DataSource = clients;
                        clientsListBox.SelectionMode = SelectionMode.One;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                clientsListBox.DataSource = null;
            }
        }

        private async void clientMobileTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var mobile = clientMobileTextBox.Text;

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                        var clients = await taskPool.AddTask(channel.Service.FindClients(0, 10, mobile));

                        clientsListBox.SelectionMode = SelectionMode.None;
                        clientsListBox.DataSource = clients;
                        clientsListBox.SelectionMode = SelectionMode.One;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                clientsListBox.DataSource = null;
            }
        }

        private void clientSurnameTextBox_Leave(object sender, EventArgs e)
        {
            clientSurnameTextBox.Text = clientSurnameTextBox.Text.ToInOffer();
        }

        private void clientNameTextBox_Leave(object sender, EventArgs e)
        {
            clientNameTextBox.Text = clientNameTextBox.Text.ToInOffer();
        }

        private void clientPatronymicTextBox_Leave(object sender, EventArgs e)
        {
            clientPatronymicTextBox.Text = clientPatronymicTextBox.Text.ToInOffer();
        }

        private void clientsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            CurrentClient = (Client)clientsListBox.SelectedItem;
        }

        private void clearCurrentClientButton_Click(object sender, EventArgs e)
        {
            CurrentClient = null;
        }

        private void servicesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node;
            if (expandedNode != null && expandedNode.Tag is ServiceGroup)
            {
                var serviceGroup = (ServiceGroup)expandedNode.Tag;

                if (expandedNode.Nodes.Cast<TreeNode>()
                    .Any(x => x.Tag.Equals(serviceGroup)))
                {
                    LoadServiceGroup(expandedNode.Nodes, serviceGroup);
                }
            }
        }

        private void servicesTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is ServiceGroup)
            {
                if (!e.Node.IsExpanded)
                {
                    e.Node.Expand();
                }
            }
        }

        private void servicesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            selectedService = selectedNode != null && selectedNode.Tag is Service ? (Service)selectedNode.Tag : null;

            LoadFreeTime();
        }

        private async void addButton_Click(object clickSender, EventArgs eventArg)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addButton.Enabled = false;

                    await channel.Service.OpenUserSession(currentUser.SessionId);

                    var surname = clientSurnameTextBox.Text.Trim();

                    if (CurrentClient == null && !string.IsNullOrWhiteSpace(surname))
                    {
                        var name = clientNameTextBox.Text.Trim();
                        var patronymic = clientPatronymicTextBox.Text.Trim();
                        var mobile = clientMobileTextBox.Text.Trim();

                        CurrentClient = await taskPool.AddTask(channel.Service.AddClient(surname, name, patronymic, string.Empty, mobile, string.Empty, string.Empty));
                    }

                    var selectedItem = servicesTreeView.SelectedNode;
                    if (selectedItem != null)
                    {
                        var selectedQueueService = (Service)selectedItem.Tag;
                        if (selectedQueueService != null)
                        {
                            ClientRequest clientRequest;

                            int subjects = (int)subjectsUpDown.Value;

                            var currentClientId = CurrentClient != null ? CurrentClient.Id : Guid.Empty;

                            if (liveRadioButton.Checked)
                            {
                                clientRequest = await taskPool.AddTask(channel.Service
                                    .AddLiveClientRequest(currentClientId, selectedQueueService.Id, priorityCheckBox.Checked,
                                    new Dictionary<Guid, object>(), subjects));
                            }
                            else
                            {
                                object selectedTime = freeTimeComboBox.SelectedItem;
                                if (selectedTime != null)
                                {
                                    clientRequest = await taskPool.AddTask(channel.Service
                                        .AddEarlyClientRequest(currentClientId, selectedQueueService.Id, earlyDatePicker.Value, (TimeSpan)selectedTime,
                                        new Dictionary<Guid, object>(), subjects));
                                }
                                else
                                {
                                    UIHelper.Warning("Не указано время предварительной записи");
                                    return;
                                }
                            }

                            LoadFreeTime();

                            CurrentClient = null;
                            subjectsUpDown.Value = Math.Min(1, subjectsUpDown.Maximum);
                            priorityCheckBox.Checked = false;

                            string coupon = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
                            var xmlReader = new XmlTextReader(new StringReader(coupon));
                            var grid = (Grid)XamlReader.Load(xmlReader);

                            xpsCouponFile = Path.GetTempFileName() + ".xps";

                            using (var container = Package.Open(xpsCouponFile, FileMode.Create))
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

                            if (couponAutoPrintCheckBox.Checked)
                            {
                                PrintCoupon();
                            }
                            else
                            {
                                Process.Start(xpsCouponFile);
                            }
                        }
                    }
                    else
                    {
                        UIHelper.Warning("Услуга не выбрана");
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
                    addButton.Enabled = true;
                }
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            PrintCoupon();
        }

        private void liveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (liveRadioButton.Checked)
            {
                queueTypeLiveGroupBox.Enabled = true;
                queueTypeEarlyGroupBox.Enabled = false;
                priorityCheckBox.Enabled = true;

                LoadFreeTime();
            }
        }

        private void earlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (earlyRadioButton.Checked)
            {
                queueTypeLiveGroupBox.Enabled = false;
                queueTypeEarlyGroupBox.Enabled = true;
                priorityCheckBox.Enabled = false;

                LoadFreeTime();
            }
        }

        private void earlyDatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadFreeTime();
        }

        private void printersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            settings.DefaultPrintQueue = (string)printersComboBox.SelectedItem;
            settings.Save();
        }

        private void AddClentRequestForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q)
            {
                if (freeTimeReport != null)
                {
                    string file = Path.GetTempFileName() + ".txt";
                    File.WriteAllLines(file, freeTimeReport);
                    Process.Start(file);
                }
            }
        }

        private void AddClentRequestForm_FormClosing(object sender, FormClosingEventArgs e)
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