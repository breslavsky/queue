using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ConfigForm : RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public ConfigForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            defaultConfigControl.Initialize(channelBuilder, currentUser);
            couponConfigControl.Initialize(channelBuilder, currentUser);
            SMTPConfigControl.Initialize(channelBuilder, currentUser);
            portalConfigControl.Initialize(channelBuilder, currentUser);
            mediaConfigControl.Initialize(channelBuilder, currentUser);
            terminalConfigControl.Initialize(channelBuilder, currentUser);
            notificationConfigControl.Initialize(channelBuilder, currentUser);
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private async void LoadConfig()
        {
            var selectedTab = сonfigTabControl.SelectedTab;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    if (selectedTab.Equals(defaultConfigTabPage))
                    {
                        defaultConfigControl.Config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
                    }
                    else if (selectedTab.Equals(couponConfigTabPage))
                    {
                        couponConfigControl.Config = await taskPool.AddTask(channel.Service.GetCouponConfig());
                    }
                    else if (selectedTab.Equals(SMTPConfigTabPage))
                    {
                        SMTPConfigControl.Config = await taskPool.AddTask(channel.Service.GetSMTPConfig());
                    }
                    else if (selectedTab.Equals(portalConfigTabPage))
                    {
                        portalConfigControl.Config = await taskPool.AddTask(channel.Service.GetPortalConfig());
                    }
                    else if (selectedTab.Equals(mediaConfigTabPage))
                    {
                        mediaConfigControl.Config = await taskPool.AddTask(channel.Service.GetMediaConfig());
                    }
                    else if (selectedTab.Equals(terminalConfigTabPage))
                    {
                        terminalConfigControl.Config = await taskPool.AddTask(channel.Service.GetTerminalConfig());
                    }
                    else if (selectedTab.Equals(notificationTabPage))
                    {
                        notificationConfigControl.Config = await taskPool.AddTask(channel.Service.GetNotificationConfig());
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

        private void сonfigTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
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