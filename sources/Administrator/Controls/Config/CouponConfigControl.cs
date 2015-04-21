using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Resources;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.Common;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class CouponConfigControl : RichUserControl
    {
        private const string HighligtingStyle = "XML";

        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private CouponConfig config;

        #endregion fields

        #region properties

        public CouponConfig Config
        {
            set
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
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void couponTemplateEditor_Leave(object sender, EventArgs e)
        {
            config.Template = couponTemplateEditor.Text;
        }

        private void previewLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs eventArgs)
        {
            try
            {
                Process.Start(XPSGenerator.FromXaml(couponTemplateEditor.Text, null));
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
            }
        }

        private void templateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            config.Template = couponTemplateEditor.Text = Templates.ClientRequestCoupon;
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
    }
}