using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Resources;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.Common;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class CouponConfigControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private const string HighligtingStyle = "XML";
        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private CouponConfig config;

        #endregion fields

        #region properties

        public CouponConfig Config
        {
            get
            {
                return config;
            }
            private set
            {
                config = value;
                if (config != null)
                {
                    couponTemplateEditor.Text = config.Template;
                    couponTemplateEditor.SetHighlighting(HighligtingStyle);
                }
            }
        }

        #endregion properties

        public CouponConfigControl()
        {
            InitializeComponent();

            config = new CouponConfig();

            if (designtime)
            {
                return;
            }

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private async void CouponConfigControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetCouponConfig());
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

        private void couponTemplateEditor_Leave(object sender, EventArgs e)
        {
            config.Template = couponTemplateEditor.Text;
        }

        private void previewLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs eventArgs)
        {
            try
            {
                Process.Start(XPSUtils.WriteXaml(couponTemplateEditor.Text, null));
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditCouponConfig(config));
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void templateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            config.Template = couponTemplateEditor.Text = Templates.ClientRequestCoupon;
        }
    }
}