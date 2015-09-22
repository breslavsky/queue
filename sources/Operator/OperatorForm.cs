using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using DTO = Queue.Services.DTO;

using Icons = Queue.UI.Common.Icons;
using QueueOperator = Queue.Services.DTO.Operator;
using Timer = System.Timers.Timer;

namespace Queue.Operator
{
    public partial class OperatorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueOperator CurrentOperator { get; set; }

        [Dependency]
        public HubQualityService HubQualityService { get; set; }

        [Dependency]
        public HubSettings HubQualitySettings { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region fields

        private const int PingInterval = 10000;

        private const int NoClient = 0;
        private const int ClientWaiting = 1;
        private const int ClientCalling = 2;
        private const int ClientRendering = 3;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Timer pingQualityTimer;
        private readonly Timer pingServerTimer;
        private readonly HubQualityCallback qualityCallback;
        private readonly DuplexChannelManager<IHubQualityTcpService> qualityChannelManager;
        private readonly ServerCallback serverCallback;
        private readonly DuplexChannelManager<IServerTcpService> serverChannelManager;
        private readonly TaskPool taskPool;
        private readonly TaskPool pingTaskPool;
        private BindingList<ClientRequestAdditionalService> additionalServices;
        private ClientRequestPlan currentClientRequestPlan;
        private BindingList<ClientRequestParameter> parameters;
        private Channel<IServerTcpService> pingServerChannel;
        private Channel<IHubQualityTcpService> pingQualityChannel;
        private byte qualityPanelDeviceId;
        private bool qualityPanelEnabled;
        private byte step = byte.MaxValue;

        #endregion fields

        #region properties

        public bool IsLogout { get; private set; }

        private ClientRequestPlan CurrentClientRequestPlan
        {
            set
            {
                Invoke(new MethodInvoker(async () =>
                {
                    if (value != null)
                    {
                        var clientRequest = value.ClientRequest;

                        if (currentClientRequestPlan == null
                            || !clientRequest.Equals(currentClientRequestPlan.ClientRequest)
                            || clientRequest.IsRecent(currentClientRequestPlan.ClientRequest))
                        {
                            numberTextBlock.Text = clientRequest.Number.ToString();
                            isPriorityCheckBox.Checked = clientRequest.IsPriority;
                            requestTimeTextBlock.Text = clientRequest.RequestTime.ToString("hh\\:mm\\:ss");

                            typeTextBlock.Text = Translater.Enum(clientRequest.Type);
                            subjectsUpDown.Value = clientRequest.Subjects;

                            var client = clientRequest.Client;
                            clientTextBlock.Text = client != null ? client.ToString() : string.Empty;

                            var service = clientRequest.Service;
                            serviceTextBlock.Text = service.ToString();

                            serviceTypeControl.Select<ServiceType>(clientRequest.ServiceType);

                            using (var channel = serverChannelManager.CreateChannel())
                            {
                                try
                                {
                                    serviceStepControl.Initialize(await taskPool.AddTask(channel.Service.GetServiceStepLinks(service.Id)));
                                    serviceStepControl.Select<ServiceStep>(clientRequest.ServiceStep);
                                }
                                catch (Exception exception)
                                {
                                    logger.Warn(exception);
                                }
                            }

                            serviceStepControl.Enabled = true;

                            stateTextBlock.Text = Translater.Enum(clientRequest.State);
                            stateTextBlock.BackColor = ColorTranslator.FromHtml(clientRequest.Color);

                            commentTextBox.Text = clientRequest.Comment;

                            using (var channel = serverChannelManager.CreateChannel())
                            {
                                try
                                {
                                    parametersGridView.Rows.Clear();
                                    parameters = new BindingList<ClientRequestParameter>(new List<ClientRequestParameter>
                                        (await taskPool.AddTask(channel.Service.GetClientRequestParameters(clientRequest.Id))));
                                    parametersBindingSource.DataSource = additionalServices;

                                    additionalServicesGridView.Rows.Clear();
                                    additionalServices = new BindingList<ClientRequestAdditionalService>(new List<ClientRequestAdditionalService>
                                        (await taskPool.AddTask(channel.Service.GetClientRequestAdditionalServices(clientRequest.Id))));
                                    additionalServicesBindingSource.DataSource = additionalServices;
                                }
                                catch (Exception exception)
                                {
                                    logger.Warn(exception);
                                }
                            }

                            versionLabel.Text = string.Format("[{0}]", clientRequest.Version);

                            switch (clientRequest.State)
                            {
                                case ClientRequestState.Waiting:
                                case ClientRequestState.Redirected:
                                case ClientRequestState.Postponed:
                                    Step = ClientWaiting;
                                    break;

                                case ClientRequestState.Calling:
                                    Step = ClientCalling;
                                    break;

                                case ClientRequestState.Rendering:
                                    Step = ClientRendering;
                                    break;
                            }
                        }

                        currentClientRequestPlan = value;

                        switch (clientRequest.State)
                        {
                            case ClientRequestState.Waiting:
                            case ClientRequestState.Redirected:
                            case ClientRequestState.Postponed:

                                if (IsAutocall && currentClientRequestPlan.StartTime <= ServerDateTime.Now.TimeOfDay)
                                {
                                    using (var channel = serverChannelManager.CreateChannel())
                                    {
                                        try
                                        {
                                            await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Calling));
                                            await taskPool.AddTask(channel.Service.CallCurrentClient());
                                        }
                                        catch (Exception exception)
                                        {
                                            logger.Warn(exception);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        currentClientRequestPlan = null;

                        numberTextBlock.Text = string.Empty;
                        requestTimeTextBlock.Text = string.Empty;
                        typeTextBlock.Text = string.Empty;
                        subjectsUpDown.Value = 0;
                        clientTextBlock.Text = string.Empty;
                        serviceTextBlock.Text = string.Empty;
                        serviceTypeControl.Reset();
                        serviceStepControl.Reset();
                        redirectOperatorControl.Reset();
                        stateTextBlock.Text = string.Empty;
                        stateTextBlock.BackColor = Color.White;
                        commentTextBox.Text = string.Empty;
                        parametersGridView.Rows.Clear();
                        additionalServicesGridView.Rows.Clear();

                        versionLabel.Text = string.Empty;
                        ratingLabel.Text = string.Empty;

                        Step = 0;
                    }
                }));
            }
        }

        private bool IsAutocall
        {
            get
            {
                return isAutocallCheckBox.Checked;
            }

            set
            {
                isAutocallCheckBox.Checked = value;
            }
        }

        private byte Step
        {
            set
            {
                if (value == step)
                {
                    return;
                }

                step = value;

                step1Panel.Visible =
                    step2Panel.Visible =
                    step3Panel.Visible =
                    subjectsPanel.Enabled =
                    serviceChangeLink.Enabled =
                    commentTextBox.Enabled =
                    commentSaveLink.Enabled =
                    serviceTypeControl.Enabled =
                    serviceStepControl.Enabled =
                    clientRequestTabControl.Enabled = false;

                if (HubQualitySettings.Enabled && qualityPanelEnabled)
                {
                    Invoke(new MethodInvoker(QualityPanelDisable));
                }

                switch (step)
                {
                    case NoClient:
                        break;

                    case ClientWaiting:
                        step1Panel.Visible = true;
                        break;

                    case ClientCalling:
                        step2Panel.Visible = true;
                        break;

                    case ClientRendering:
                        step3Panel.Visible =
                        subjectsPanel.Enabled =
                            serviceTypeControl.Enabled =
                            serviceChangeLink.Enabled =
                            commentTextBox.Enabled =
                            commentSaveLink.Enabled =
                            clientRequestTabControl.Enabled = true;

                        if (HubQualitySettings.Enabled && !qualityPanelEnabled)
                        {
                            Invoke(new MethodInvoker(QualityPanelEnable));
                        }

                        Invoke(new MethodInvoker(LoadRedirectOperators));

                        break;
                }
            }
        }

        #endregion properties

        public OperatorForm()
            : base()
        {
            InitializeComponent();

            serverCallback = new ServerCallback();
            serverCallback.OnCurrentClientRequestPlanUpdated += serverCallback_OnCurrentClientRequestPlanUpdated;
            serverCallback.OnOperatorPlanMetricsUpdated += serverCallback_OnOperatorPlanMetricsUpdated;

            serverChannelManager = ServerService.CreateChannelManager(CurrentOperator.SessionId);

            qualityCallback = new HubQualityCallback();
            qualityCallback.OnAccepted += qualityCallback_OnAccepted;

            qualityChannelManager = HubQualityService.CreateChannelManager();

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            pingTaskPool = new TaskPool();

            pingServerTimer = new Timer();
            pingServerTimer.Elapsed += pingServerTimer_Elapsed;

            pingQualityTimer = new Timer();
            pingQualityTimer.Elapsed += pingQualityTimer_Elapsed;

            qualityPanelDeviceId = CurrentOperator.Workplace.QualityPanelDeviceId;
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

                if (pingTaskPool != null)
                {
                    pingTaskPool.Dispose();
                }

                #region timers

                if (pingServerTimer != null)
                {
                    pingServerTimer.Dispose();
                }
                if (pingQualityTimer != null)
                {
                    pingQualityTimer.Dispose();
                }

                if (serverChannelManager != null)
                {
                    serverChannelManager.Dispose();
                }
                if (qualityChannelManager != null)
                {
                    qualityChannelManager.Dispose();
                }

                #endregion timers

                #region channels

                if (pingServerChannel != null)
                {
                    pingServerChannel.Dispose();
                }
                if (serverChannelManager != null)
                {
                    serverChannelManager.Dispose();
                }
                if (pingQualityChannel != null)
                {
                    pingQualityChannel.Dispose();
                }
                if (qualityChannelManager != null)
                {
                    qualityChannelManager.Dispose();
                }

                #endregion channels
            }
            base.Dispose(disposing);
        }

        #region form events

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pingServerTimer.Stop();
            pingServerTimer.Dispose();

            pingQualityTimer.Stop();
            pingQualityTimer.Dispose();

            pingServerChannel.Close();
            if (pingQualityChannel != null)
            {
                pingQualityChannel.Close();
            }

            taskPool.Cancel();
            pingTaskPool.Cancel();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            serviceTypeControl.Initialize<ServiceType>();

            Step = 0;

            foreach (Control control in stepPanel.Controls)
            {
                control.Location = new Point(0, 0);
            }

            Text = string.Format("{0} | {1}", CurrentOperator, CurrentOperator.Workplace);

            pingServerTimer.Start();

            if (HubQualitySettings.Enabled)
            {
                pingQualityTimer.Start();
            }
        }

        #endregion form events

        #region timers

        private void pingServerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingServerTimer.Stop();
            if (pingServerTimer.Interval < PingInterval)
            {
                pingServerTimer.Interval = PingInterval;
            }

            Invoke((MethodInvoker)async delegate
            {
                if (pingServerChannel == null)
                {
                    pingServerChannel = serverChannelManager.CreateChannel(serverCallback);
                }

                try
                {
                    serverStateLabel.Image = Icons.connecting16x16;

                    if (!pingServerChannel.IsConnected)
                    {
                        pingServerChannel.Service.Subscribe(ServerServiceEventType.CurrentClientRequestPlanUpdated,
                            new ServerSubscribtionArgs { Operators = new DTO.Operator[] { CurrentOperator } });
                        pingServerChannel.Service.Subscribe(ServerServiceEventType.OperatorPlanMetricsUpdated,
                            new ServerSubscribtionArgs { Operators = new DTO.Operator[] { CurrentOperator } });
                        CurrentClientRequestPlan = await pingTaskPool.AddTask(pingServerChannel.Service.GetCurrentClientRequestPlan());
                    }

                    ServerDateTime.Sync(await pingTaskPool.AddTask(pingServerChannel.Service.GetDateTime()));
                    currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

                    await pingTaskPool.AddTask(pingServerChannel.Service.UserHeartbeat());

                    serverStateLabel.Image = Icons.online16x16;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (Exception exception)
                {
                    logger.Warn(exception);

                    currentDateTimeLabel.Text = exception.Message;
                    serverStateLabel.Image = Icons.offline16x16;

                    pingServerChannel.Dispose();
                    pingServerChannel = null;
                }
                finally
                {
                    if (!IsDisposed)
                    {
                        pingServerTimer.Start();
                    }
                }
            });
        }

        private void pingQualityTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingQualityTimer.Stop();
            if (pingQualityTimer.Interval < PingInterval)
            {
                pingQualityTimer.Interval = PingInterval;
            }

            Invoke((MethodInvoker)async delegate
            {
                if (pingQualityChannel == null)
                {
                    pingQualityChannel = qualityChannelManager.CreateChannel(qualityCallback);
                }

                try
                {
                    qualityStateLabel.Image = Icons.connecting16x16;

                    if (!pingQualityChannel.IsConnected)
                    {
                        pingQualityChannel.Service.Subscribe(HubQualityServiceEventType.RatingAccepted,
                            new HubQualityServiceSubscribtionArgs { DeviceId = qualityPanelDeviceId });
                    }

                    await pingTaskPool.AddTask(pingQualityChannel.Service.Heartbeat());

                    qualityStateLabel.Image = Icons.online16x16;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (Exception exception)
                {
                    logger.Warn(exception);

                    qualityStateLabel.Image = Icons.offline16x16;

                    pingQualityChannel.Dispose();
                    pingQualityChannel = null;
                }
                finally
                {
                    if (!IsDisposed)
                    {
                        pingQualityTimer.Start();
                    }
                }
            });
        }

        #endregion timers

        #region callbacks

        private void qualityCallback_OnAccepted(object sender, HubQualityEventArgs e)
        {
            Invoke(new MethodInvoker(async () =>
            {
                ratingLabel.Text = string.Format("[{0}]", e.Rating);

                if (currentClientRequestPlan != null)
                {
                    var clientRequest = currentClientRequestPlan.ClientRequest;
                    clientRequest.Rating = e.Rating;

                    using (var channel = serverChannelManager.CreateChannel())
                    {
                        try
                        {
                            await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
                        }
                        catch (OperationCanceledException) { }
                        catch (CommunicationObjectAbortedException) { }
                        catch (ObjectDisposedException) { }
                        catch (InvalidOperationException) { }
                        catch (FaultException exception)
                        {
                            logger.Warn(exception.Reason);
                        }
                        catch (Exception exception)
                        {
                            logger.Error(exception);
                        }
                    }
                }
            }));
        }

        private async void serverCallback_OnCurrentClientRequestPlanUpdated(object sender, ServerEventArgs e)
        {
            await Task.Run(() =>
            {
                CurrentClientRequestPlan = e.ClientRequestPlan;
            });
        }

        private async void serverCallback_OnOperatorPlanMetricsUpdated(object sender, ServerEventArgs e)
        {
            await Task.Run(() =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    standingLabel.Text = e.OperatorPlanMetrics.Standing.ToString();
                }));
            });
        }

        #endregion callbacks

        private async void LoadRedirectOperators()
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    redirectOperatorControl.Initialize(await taskPool.AddTask(channel.Service.GetRedirectOperatorsLinks()));
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

        private async void QualityPanelEnable()
        {
            using (var channel = qualityChannelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.Enable(qualityPanelDeviceId));
                    qualityPanelEnabled = true;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    logger.Warn(exception.Reason);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        private async void QualityPanelDisable()
        {
            using (var channel = qualityChannelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.Disable(qualityPanelDeviceId));
                    qualityPanelEnabled = false;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    logger.Warn(exception.Reason);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        private async void mainTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    Width = MinimumSize.Width;
                    break;

                case 1:
                    Width = MaximumSize.Width;

                    using (var channel = serverChannelManager.CreateChannel())
                    {
                        try
                        {
                            clientRequestsGridView.Rows.Clear();

                            var clientRequestPlans = await taskPool.AddTask(channel.Service.GetOperatorClientRequestPlans());

                            foreach (var clientRequestPlan in clientRequestPlans)
                            {
                                var clientRequest = clientRequestPlan.ClientRequest;

                                int index = clientRequestsGridView.Rows.Add();
                                var row = clientRequestsGridView.Rows[index];

                                row.Cells["numberColumn"].Value = clientRequest.Number;
                                row.Cells["subjectsColumn"].Value = clientRequest.Subjects;
                                row.Cells["startTimeColumn"].Value = clientRequestPlan.StartTime.ToString("hh\\:mm\\:ss");
                                row.Cells["timeIntervalColumn"].Value = (clientRequestPlan.FinishTime - clientRequestPlan.StartTime).Minutes;
                                row.Cells["clientColumn"].Value = clientRequest.Client;
                                row.Cells["serviceColumn"].Value = clientRequest.Service;
                                row.Cells["stateColumn"].Value = Translater.Enum(clientRequest.State);
                                row.Tag = clientRequest;
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
                    break;
            }
        }

        private async void serviceChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;

                using (var f = new SelectServiceForm())
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        clientRequest.Service = f.Service;

                        using (var channel = serverChannelManager.CreateChannel())
                        {
                            try
                            {
                                serviceChangeLink.Enabled = false;

                                await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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

        private async void serviceStepControl_SelectedChanged(object sender, EventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.ServiceStep = serviceStepControl.Selected<ServiceStep>();

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        serviceStepControl.Enabled = false;

                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                        serviceStepControl.Enabled = true;
                    }
                }
            }
        }

        private async void serviceTypeControl_SelectedChanged(object sender, EventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                ClientRequest clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.ServiceType = serviceTypeControl.Selected<ServiceType>();

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        serviceTypeControl.Enabled = false;

                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                        serviceTypeControl.Enabled = true;
                    }
                }
            }
        }

        private async void subjectsChangeButton_Click(object sender, EventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.Subjects = (int)subjectsUpDown.Value;

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        subjectsChangeButton.Enabled = false;

                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
        }

        private async void commentSaveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.Comment = commentTextBox.Text;

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        commentSaveLink.Enabled = false;

                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                        commentSaveLink.Enabled = true;
                    }
                }
            }
        }

        private void settingsButton_ButtonClick(object sender, EventArgs e)
        {
            using (var f = new SettingsForm())
            {
                f.ShowDialog();
            }
        }

        #region task pool

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #endregion task pool

        #region step 1

        private async void callClientButton_Click(object sender, EventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var startTime = currentClientRequestPlan.StartTime;

                if (startTime > ServerDateTime.Now.TimeOfDay
                    && MessageBox.Show(string.Format(@"Время запроса клиента {0:hh\:mm\:ss} еще не наступило.
Вы подтверждаете вызов клиента в текущее время?", startTime), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                bool completed = false;

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        callClientButton.Enabled = false;
                        await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Calling));
                        completed = true;
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
                        if (completed)
                        {
                            digitalTimer.Reset();

                            Task.Run(() =>
                            {
                                Thread.Sleep(2000);
                                Invoke(new MethodInvoker(() => callClientButton.Enabled = true));
                            });
                        }
                        else
                        {
                            callClientButton.Enabled = true;
                        }
                    }
                }
            }
        }

        #endregion step 1

        #region step 2

        private async void absenceButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    absenceButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Absence));
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
                    absenceButton.Enabled = true;

                    digitalTimer.Reset();
                }
            }
        }

        private async void recallingButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    recallingButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.CallCurrentClient());
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
                    recallingButton.Enabled = true;

                    digitalTimer.Reset();
                }
            }
        }

        private async void renderingButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    renderingButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Rendering));
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
                    renderingButton.Enabled = true;

                    digitalTimer.Reset();
                }
            }
        }

        #endregion step 2

        #region step 3

        private async void postponeButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    postponeButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.PostponeCurrentClientRequest(new TimeSpan(0, (int)postponeMinutesUpDown.Value, 0)));
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

                    digitalTimer.Reset();
                }
            }
        }

        private async void renderedButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    renderedButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Rendered));
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
                    renderedButton.Enabled = true;

                    digitalTimer.Reset();
                }
            }
        }

        private async void returnButton_Click(object sender, EventArgs e)
        {
            using (var channel = serverChannelManager.CreateChannel())
            {
                try
                {
                    returnButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.ReturnCurrentClientRequest());
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
                    returnButton.Enabled = true;

                    digitalTimer.Reset();
                }
            }
        }

        private async void targetOperatorControl_SelectedChanged(object sender, EventArgs e)
        {
            var redirectOperator = redirectOperatorControl.Selected<QueueOperator>();
            if (redirectOperator != null)
            {
                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        redirectOperatorControl.Enabled = false;

                        await taskPool.AddTask(channel.Service.RedirectCurrentClientRequest(redirectOperator.Id));
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
                        redirectOperatorControl.Enabled = true;

                        digitalTimer.Reset();
                    }
                }
            }
        }

        private void reloadRedirectOperator_Click(object sender, EventArgs e)
        {
            LoadRedirectOperators();
        }

        #endregion step 3

        #region buttons

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        #endregion buttons

        #region additional services

        private void addAdditionalServiceButton_Click(object sender, EventArgs e)
        {
            if (currentClientRequestPlan.ClientRequest != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;

                using (var f = new EditClientRequestAdditionalServiceForm(clientRequest.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        additionalServices.Add(f.ClientRequestAdditionalService);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private void additionalServicesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var currentRow = additionalServicesGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            ClientRequestAdditionalService additionalService = additionalServices[currentRow.Index];

            using (var f = new EditClientRequestAdditionalServiceForm(null, additionalService.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    additionalService.Update(f.ClientRequestAdditionalService);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void additionalServicesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var currentRow = additionalServicesGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить дополнительную услугу?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var additionalService = additionalServices[currentRow.Index];

                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteClientRequestAdditionalService(additionalService.Id));
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
            else
            {
                e.Cancel = true;
            }
        }

        #endregion additional services
    }
}