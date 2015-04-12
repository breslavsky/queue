using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class CurrentUserForm : Form
    {
        private IConfigurationManager configuration;
        private AdministratorSettings settings;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public CurrentUserForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);

            taskPool = new TaskPool();

            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            InitializeComponent();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void CurrentUserForm_Load(object sender, EventArgs e)
        {
            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            settings = configuration.GetSection<AdministratorSettings>(AdministratorSettings.SectionKey);

            currentUserLabel.Text = currentUser.ToString();

            foreach (var p in new PrintServer().GetPrintQueues(new[] {
                EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }))
            {
                couponPrintersComboBox.Items.Add(p.FullName);
            }

            couponPrintersComboBox.SelectedItem = string.IsNullOrWhiteSpace(settings.CouponPrinter)
                ? LocalPrintServer.GetDefaultPrintQueue().FullName
                : settings.CouponPrinter;
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

        private void CurrentUserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void passwordButton_Click(object sender, EventArgs e)
        {
            using (var f = new PasswordForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            passwordButton.Enabled = false;

                            await taskPool.AddTask(channel.Service.ChangeUserPassword(currentUser.Id, f.Password));
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
                            passwordButton.Enabled = true;
                        }
                    }
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            configuration.Save();
            Close();
        }

        private void couponPrintersComboBox_Leave(object sender, EventArgs e)
        {
            settings.CouponPrinter = couponPrintersComboBox.SelectedItem.ToString();
        }
    }
}