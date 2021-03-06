﻿using Junte.Parallel;
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
using Queue.Services.Contracts.Hub;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
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
        public HubSettings HubQualitySettings { get; set; }

        [Dependency]
        public DuplexChannelManager<IQualityTcpService> QualityChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ServerChannelManager { get; set; }

        [Dependency]
        public DuplexChannelManager<IQueuePlanTcpService> QueuePlanChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IUserTcpService> ServerUserChannelManager { get; set; }

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
        private readonly QualityCallback qualityCallback;
        private readonly QueuePlanCallback queuePlanCallback;
        private readonly TaskPool taskPool;
        private readonly TaskPool pingTaskPool;
        private BindingList<ClientRequestAdditionalService> additionalServices;
        private ClientRequestPlan currentClientRequestPlan;
        private BindingList<ClientRequestParameter> parameters;
        private Channel<IQueuePlanTcpService> queuePlanChannel;
        private Channel<IQualityTcpService> qualityChannel;
        private byte qualityPanelDeviceId;
        private bool qualityPanelEnabled;
        private byte step = byte.MaxValue;

        #endregion fields

        #region properties

        public bool IsLogout { get; private set; }

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

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

                            using (var channel = ServerChannelManager.CreateChannel())
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

                            using (var channel = ServerChannelManager.CreateChannel())
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

                                if (currentClientRequestPlan.StartTime <= ServerDateTime.Now.TimeOfDay)
                                {
                                    using (var channel = QueuePlanChannelManager.CreateChannel())
                                    {
                                        try
                                        {
                                            await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Calling));
                                            await taskPool.AddTask(channel.Service.CallCurrentClient());
                                            if (WindowState != FormWindowState.Normal)
                                            {
                                                WindowState = FormWindowState.Normal;
                                            }

                                            SetForegroundWindow(Handle.ToInt32());
                                            Console.Beep();
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
                    clientRequestTabControl.Enabled =
                    redirectToOperatorMenuItem.Enabled = false;

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
                            clientRequestTabControl.Enabled =
                            redirectToOperatorMenuItem.Enabled = true;

                        if (HubQualitySettings.Enabled && !qualityPanelEnabled)
                        {
                            Invoke(new MethodInvoker(QualityPanelEnable));
                        }

                        break;
                }
            }
        }

        #endregion properties

        public OperatorForm()
            : base()
        {
            InitializeComponent();

            queuePlanCallback = new QueuePlanCallback();
            queuePlanCallback.OnCurrentClientRequestPlanUpdated += queuePlanCallback_OnCurrentClientRequestPlanUpdated;
            queuePlanCallback.OnOperatorPlanMetricsUpdated += queuePlanCallback_OnOperatorPlanMetricsUpdated;

            qualityCallback = new QualityCallback();

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            taskPool.OnAddTask += taskPool_OnAddTask;

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

                if (ServerChannelManager != null)
                {
                    ServerChannelManager.Dispose();
                }
                if (QualityChannelManager != null)
                {
                    QualityChannelManager.Dispose();
                }

                #endregion timers

                #region channels

                if (queuePlanChannel != null)
                {
                    queuePlanChannel.Dispose();
                }
                if (ServerChannelManager != null)
                {
                    ServerChannelManager.Dispose();
                }
                if (qualityChannel != null)
                {
                    qualityChannel.Dispose();
                }
                if (QualityChannelManager != null)
                {
                    QualityChannelManager.Dispose();
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

            if (queuePlanChannel != null)
            {
                queuePlanChannel.Close();
            }

            if (qualityChannel != null)
            {
                qualityChannel.Close();
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
                if (queuePlanChannel == null)
                {
                    queuePlanChannel = QueuePlanChannelManager.CreateChannel(queuePlanCallback);
                }

                try
                {
                    serverStateLabel.Image = Icons.connecting16x16;

                    if (!queuePlanChannel.IsConnected)
                    {
                        queuePlanChannel.Service.Subscribe(QueuePlanEventType.CurrentClientRequestPlanUpdated,
                            new QueuePlanSubscribtionArgs { Operators = new DTO.Operator[] { CurrentOperator } });
                        queuePlanChannel.Service.Subscribe(QueuePlanEventType.OperatorPlanMetricsUpdated,
                            new QueuePlanSubscribtionArgs { Operators = new DTO.Operator[] { CurrentOperator } });
                        CurrentClientRequestPlan = await pingTaskPool.AddTask(queuePlanChannel.Service.GetCurrentClientRequestPlan());
                    }

                    using (var channel = ServerChannelManager.CreateChannel())
                    {
                        ServerDateTime.Sync(await taskPool.AddTask(channel.Service.GetDateTime()));
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();
                    }

                    using (var channel = ServerUserChannelManager.CreateChannel())
                    {
                        await pingTaskPool.AddTask(channel.Service.UserHeartbeat());
                    }

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

                    queuePlanChannel.Dispose();
                    queuePlanChannel = null;
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
                if (qualityChannel == null)
                {
                    qualityChannel = QualityChannelManager.CreateChannel(qualityCallback);
                }

                try
                {
                    qualityStateLabel.Image = Icons.connecting16x16;

                    if (!qualityChannel.IsConnected)
                    {
                        qualityChannel.Service.Subscribe(QualityServiceEventType.RatingAccepted,
                            new QualityServiceSubscribtionArgs { DeviceId = qualityPanelDeviceId });
                    }

                    await pingTaskPool.AddTask(qualityChannel.Service.Heartbeat());

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

                    qualityChannel.Dispose();
                    qualityChannel = null;
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

        private void qualityCallback_OnAccepted(object sender, QualityEventArgs e)
        {
            Invoke(new MethodInvoker(async () =>
            {
                ratingLabel.Text = string.Format("[{0}]", e.Rating);

                if (currentClientRequestPlan != null)
                {
                    var clientRequest = currentClientRequestPlan.ClientRequest;
                    clientRequest.Rating = e.Rating;

                    try
                    {
                        using (var channel = QueuePlanChannelManager.CreateChannel())
                        {
                            await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
                        }

                        qualityCallback.OnAccepted -= qualityCallback_OnAccepted;
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
                    catch (Exception exception)
                    {
                        logger.Error(exception);
                    }
                }
            }));
        }

        private async void queuePlanCallback_OnCurrentClientRequestPlanUpdated(object sender, QueuePlanEventArgs e)
        {
            await Task.Run(() =>
            {
                CurrentClientRequestPlan = e.ClientRequestPlan;
            });
        }

        private async void queuePlanCallback_OnOperatorPlanMetricsUpdated(object sender, QueuePlanEventArgs e)
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

        private async void QualityPanelEnable()
        {
            using (var channel = QualityChannelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.Enable(qualityPanelDeviceId));

                    qualityCallback.OnAccepted += qualityCallback_OnAccepted;
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
            using (var channel = QualityChannelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.Disable(qualityPanelDeviceId));

                    qualityCallback.OnAccepted -= qualityCallback_OnAccepted;
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

                        try
                        {
                            serviceChangeLink.Enabled = false;

                            using (var channel = QueuePlanChannelManager.CreateChannel())
                            {
                                await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                            serviceChangeLink.Enabled = true;
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

                try
                {
                    serviceStepControl.Enabled = false;

                    using (var channel = QueuePlanChannelManager.CreateChannel())
                    {
                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                    serviceStepControl.Enabled = true;
                }
            }
        }

        private async void serviceTypeControl_SelectedChanged(object sender, EventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.ServiceType = serviceTypeControl.Selected<ServiceType>();

                try
                {
                    serviceTypeControl.Enabled = false;

                    using (var channel = QueuePlanChannelManager.CreateChannel())
                    {
                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                    serviceTypeControl.Enabled = true;
                }
            }
        }

        private async void subjectsChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.Subjects = (int)subjectsUpDown.Value;

                try
                {
                    subjectsChangeLink.Enabled = false;

                    using (var channel = QueuePlanChannelManager.CreateChannel())
                    {
                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                    subjectsChangeLink.Enabled = true;
                }
            }
        }

        private async void commentSaveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                var clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.Comment = commentTextBox.Text;

                try
                {
                    commentSaveLink.Enabled = false;

                    using (var channel = QueuePlanChannelManager.CreateChannel())
                    {
                        await taskPool.AddTask(channel.Service.EditCurrentClientRequest(clientRequest));
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
                    commentSaveLink.Enabled = true;
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

        private async void mainTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    Width = MinimumSize.Width;
                    break;

                case 1:
                    Width = MaximumSize.Width;

                    try
                    {
                        clientRequestsGridView.Rows.Clear();

                        using (var channel = QueuePlanChannelManager.CreateChannel())
                        {
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
                    break;
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

        #region actions

        private void callClientByRequestNumberMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new CallClientByRequestNumberForm())
            {
                f.ShowDialog();
            }
        }

        private void redirectToOperatorMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new RedirectToOperatorForm())
            {
                f.ShowDialog();
            }
        }

        #endregion actions

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

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    try
                    {
                        callClientButton.Enabled = false;

                        await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Calling));
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
                        callClientButton.Enabled = true;
                    }
                }
            }
        }

        #endregion step 1

        #region step 2

        private async void absenceButton_Click(object sender, EventArgs e)
        {
            try
            {
                absenceButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Absence));
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
                absenceButton.Enabled = true;

                digitalTimer.Reset();
            }
        }

        private async void recallingButton_Click(object sender, EventArgs e)
        {
            try
            {
                recallingButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.CallCurrentClient());
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
                recallingButton.Enabled = true;

                digitalTimer.Reset();
            }
        }

        private async void renderingButton_Click(object sender, EventArgs e)
        {
            try
            {
                renderingButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Rendering));
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
                renderingButton.Enabled = true;

                digitalTimer.Reset();
            }
        }

        #endregion step 2

        #region step 3

        private async void postponeButton_Click(object sender, EventArgs e)
        {
            try
            {
                postponeButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.PostponeCurrentClientRequest(new TimeSpan(0, (int)postponeMinutesUpDown.Value, 0)));
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
                postponeButton.Enabled = true;

                digitalTimer.Reset();
            }
        }

        private async void renderedButton_Click(object sender, EventArgs e)
        {
            try
            {
                renderedButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.UpdateCurrentClientRequest(ClientRequestState.Rendered));
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
                renderedButton.Enabled = true;

                digitalTimer.Reset();
            }
        }

        private async void returnButton_Click(object sender, EventArgs e)
        {
            try
            {
                returnButton.Enabled = false;

                using (var channel = QueuePlanChannelManager.CreateChannel())
                {
                    await taskPool.AddTask(channel.Service.ReturnCurrentClientRequest());
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
                returnButton.Enabled = true;

                digitalTimer.Reset();
            }
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

            var additionalService = additionalServices[currentRow.Index];

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

                using (var channel = ServerChannelManager.CreateChannel())
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