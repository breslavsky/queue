using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Resources;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;
using UserControl = System.Windows.Forms.UserControl;

namespace Queue.Administrator
{
    public partial class CouponConfigControl : UserControl
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
            Grid grid;

            try
            {
                var xmlReader = new XmlTextReader(new StringReader(couponTemplateEditor.Text));
                grid = (Grid)XamlReader.Load(xmlReader);
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
                return;
            }

            string xpsFile = Path.GetTempFileName() + ".xps";

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

            Process.Start(xpsFile);
        }

        private void templateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            couponTemplateEditor.Text = Templates.Coupon;
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