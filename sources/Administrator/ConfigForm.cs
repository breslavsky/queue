using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.ServiceModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ConfigForm : Queue.UI.WinForms.RichForm
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

            defaultConfigControl.Initialize(channelBuilder, currentUser);
            couponConfigControl.Initialize(channelBuilder, currentUser);
            SMTPConfigControl.Initialize(channelBuilder, currentUser);
            portalConfigControl.Initialize(channelBuilder, currentUser);
            mediaConfigControl.Initialize(channelBuilder, currentUser);
            terminalConfigControl.Initialize(channelBuilder, currentUser);
            notificationConfigControl.Initialize(channelBuilder, currentUser);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private async void LoadConfig()
        {
            var selectedTab = сonfigTabControl.SelectedTab;
            if (selectedTab.Tag == null)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        switch (сonfigTabControl.SelectedIndex)
                        {
                            case 0:
                                defaultConfigControl.Config = await taskPool.AddTask(channel.Service.GetDefaultConfig());

                                break;

                            case 1:
                                couponConfigControl.Config = await taskPool.AddTask(channel.Service.GetCouponConfig());

                                break;

                            case 2:
                                SMTPConfigControl.Config = await taskPool.AddTask(channel.Service.GetSMTPConfig());

                                break;

                            case 3:
                                portalConfigControl.Config = await taskPool.AddTask(channel.Service.GetPortalConfig());

                                break;

                            case 4:
                                mediaConfigControl.Config = await taskPool.AddTask(channel.Service.GetMediaConfig());

                                break;

                            case 5:
                                terminalConfigControl.Config = await taskPool.AddTask(channel.Service.GetTerminalConfig());

                                break;

                            case 6:
                                notificationConfigControl.Config = await taskPool.AddTask(channel.Service.GetNotificationConfig());

                                break;
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

                selectedTab.Tag = true;
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