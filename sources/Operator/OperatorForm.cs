using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using log4net;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
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
    public partial class OperatorForm : Queue.UI.WinForms.RichForm
    {
        private const int PingInterval = 10000;
        private static readonly ILog logger = LogManager.GetLogger(typeof(OperatorForm));

        private readonly ServerCallback callbackObject;
        private readonly DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly QueueOperator currentUser;
        private readonly Timer pingTimer;
        private readonly TaskPool taskPool;
        private ClientRequestPlan currentClientRequestPlan;
        private Channel<IServerTcpService> pingChannel;

        public OperatorForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser as QueueOperator;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            step = 0;

            foreach (Control control in stepPanel.Controls)
            {
                control.Location = new Point(0, 0);
            }

            Text = string.Format("{0} | {1}", currentUser, (currentUser as QueueOperator).Workplace);

            callbackObject = new ServerCallback();
            callbackObject.OnCurrentClientRequestPlanUpdated += callbackObject_CurrentClientRequestPlanUpdated;
            callbackObject.OnOperatorPlanMetricsUpdated += callbackObject_OnOperatorPlanMetricsUpdated;

            pingChannel = channelManager.CreateChannel(callbackObject);

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;
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

                            typeTextBlock.Text = clientRequest.Type.Translate();
                            subjectsUpDown.Value = clientRequest.Subjects;

                            var client = clientRequest.Client;
                            clientTextBlock.Text = client != null ? client.ToString() : string.Empty;

                            var service = clientRequest.Service;
                            serviceTextBlock.Text = service.ToString();

                            if (service.Type != ServiceType.None)
                            {
                                var serviceTypes = new List<ServiceType>() { ServiceType.None };
                                foreach (ServiceType type in Enum.GetValues(typeof(ServiceType)))
                                {
                                    if (type != ServiceType.None && service.Type.HasFlag(type))
                                    {
                                        serviceTypes.Add(type);
                                    }
                                }
                                if (serviceTypes.Count > 0)
                                {
                                    serviceTypeControl.Initialize<ServiceType>(serviceTypes.ToArray());
                                    serviceTypeControl.Select<ServiceType>(clientRequest.ServiceType);
                                }
                            }

                            using (var channel = channelManager.CreateChannel())
                            {
                                try
                                {
                                    serviceStepControl.Initialize(await taskPool.AddTask(channel.Service.GetServiceSteps(service.Id)));
                                    serviceStepControl.Select<ServiceStep>(clientRequest.ServiceStep);
                                }
                                catch (Exception exception)
                                {
                                    logger.Warn(exception);
                                }
                            }

                            stateTextBlock.Text = clientRequest.State.Translate();
                            stateTextBlock.BackColor = ColorTranslator.FromHtml(clientRequest.Color);

                            parametersGridView.Rows.Clear();

                            foreach (var parameter in clientRequest.Parameters)
                            {
                                int index = parametersGridView.Rows.Add();
                                var row = parametersGridView.Rows[index];
                                row.Cells["parameterNameColumn"].Value = parameter.Name;
                                row.Cells["parameterValueColumn"].Value = parameter.Value;
                            }

                            versionLabel.Text = string.Format("[{0}]", clientRequest.Version);

                            switch (clientRequest.State)
                            {
                                case ClientRequestState.Waiting:
                                case ClientRequestState.Postponed:
                                    step = 1;
                                    break;

                                case ClientRequestState.Calling:
                                    step = 2;
                                    break;

                                case ClientRequestState.Rendering:
                                    step = 3;
                                    break;
                            }
                        }

                        currentClientRequestPlan = value;

                        switch (clientRequest.State)
                        {
                            case ClientRequestState.Waiting:
                            case ClientRequestState.Postponed:

                                if (IsAutocall && currentClientRequestPlan.StartTime <= ServerDateTime.Now.TimeOfDay)
                                {
                                    using (var channel = channelManager.CreateChannel())
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
                        serviceTypeControl.Clear();
                        serviceStepControl.Clear();
                        stateTextBlock.Text = string.Empty;
                        stateTextBlock.BackColor = Color.White;
                        parametersGridView.Rows.Clear();

                        versionLabel.Text = string.Empty;

                        step = 0;
                    }
                }));
            }
        }

        private int step
        {
            set
            {
                step1Panel.Visible =
                    step2Panel.Visible =
                    step3Panel.Visible = false;

                subjectsPanel.Enabled =
                    serviceChangeLink.Enabled =
                    serviceTypeControl.Enabled =
                    serviceStepControl.Enabled = false;

                switch (value)
                {
                    case 0:
                        break;

                    case 1:
                        step1Panel.Visible = true;
                        break;

                    case 2:
                        step2Panel.Visible = true;
                        break;

                    case 3:
                        step3Panel.Visible = true;
                        subjectsPanel.Enabled =
                            serviceChangeLink.Enabled =
                            serviceTypeControl.Enabled =
                            serviceStepControl.Enabled = true;
                        break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (pingTimer != null)
                {
                    pingTimer.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (pingChannel != null)
                {
                    pingChannel.Dispose();
                }
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private async void callbackObject_CurrentClientRequestPlanUpdated(object sender, ServerEventArgs e)
        {
            await Task.Run(() =>
            {
                CurrentClientRequestPlan = e.ClientRequestPlan;
            });
        }

        private async void callbackObject_OnOperatorPlanMetricsUpdated(object sender, ServerEventArgs e)
        {
            await Task.Run(() =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    standingLabel.Text = e.OperatorPlanMetrics.Standing.ToString();
                }));
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pingTimer.Stop();
            taskPool.Cancel();
            pingChannel.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pingTimer.Start();
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

                    using (var channel = channelManager.CreateChannel())
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
                                row.Cells["stateColumn"].Value = clientRequest.State.Translate();
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

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval < PingInterval)
            {
                pingTimer.Interval = PingInterval;
            }

            Invoke((MethodInvoker)async delegate
            {
                try
                {
                    serverStateLabel.Image = Icons.connecting16x16;

                    if (!pingChannel.IsConnected)
                    {
                        pingChannel.Service.Subscribe(ServerServiceEventType.CurrentClientRequestPlanUpdated,
                            new ServerSubscribtionArgs { Operators = new DTO.Operator[] { currentUser as QueueOperator } });
                        pingChannel.Service.Subscribe(ServerServiceEventType.OperatorPlanMetricsUpdated,
                            new ServerSubscribtionArgs { Operators = new DTO.Operator[] { currentUser as QueueOperator } });
                        CurrentClientRequestPlan = await taskPool.AddTask(pingChannel.Service.GetCurrentClientRequestPlan());
                    }

                    ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));
                    currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

                    await taskPool.AddTask(pingChannel.Service.UserHeartbeat());

                    serverStateLabel.Image = Icons.online16x16;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (Exception exception)
                {
                    currentDateTimeLabel.Text = exception.Message;
                    serverStateLabel.Image = Icons.offline16x16;

                    pingChannel.Dispose();
                    pingChannel = channelManager.CreateChannel(callbackObject);
                }
                finally
                {
                    pingTimer.Start();
                }
            });
        }

        private async void serviceChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentClientRequestPlan != null)
            {
                ClientRequest clientRequest = currentClientRequestPlan.ClientRequest;

                using (var f = new SelectServiceForm(channelBuilder, currentUser))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        clientRequest.Service = f.Service;

                        using (var channel = channelManager.CreateChannel())
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
                ClientRequest clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.ServiceStep = serviceStepControl.Selected<ServiceStep>();

                using (var channel = channelManager.CreateChannel())
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

                using (var channel = channelManager.CreateChannel())
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
                ClientRequest clientRequest = currentClientRequestPlan.ClientRequest;
                clientRequest.Subjects = (int)subjectsUpDown.Value;

                using (var channel = channelManager.CreateChannel())
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

                using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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
            using (var channel = channelManager.CreateChannel())
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

        #endregion step 3

        #region buttons

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        #endregion buttons
    }
}