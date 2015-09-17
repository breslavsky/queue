using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Administrator.Settings;
using Queue.Services.Contracts;
using Queue.UI.WinForms;
using System;
using System.Printing;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class CurrentUserForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        [Dependency]
        public AdministratorSettings Settings { get; set; }

        [Dependency]
        public ConfigurationManager Configuration { get; set; }

        #endregion dependency

        #region fields

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public CurrentUserForm()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

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

        private void CurrentUserForm_Load(object sender, EventArgs e)
        {
            currentUserLabel.Text = CurrentUser.ToString();

            foreach (var p in new PrintServer().GetPrintQueues(new[] {
                EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }))
            {
                couponPrintersComboBox.Items.Add(p.FullName);
            }

            couponPrintersComboBox.SelectedItem = string.IsNullOrWhiteSpace(Settings.CouponPrinter)
                ? LocalPrintServer.GetDefaultPrintQueue().FullName
                : Settings.CouponPrinter;
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

                            await taskPool.AddTask(channel.Service.ChangeUserPassword(CurrentUser.Id, f.Password));
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
            Configuration.Save();
            Close();
        }

        private void couponPrintersComboBox_Leave(object sender, EventArgs e)
        {
            Settings.CouponPrinter = couponPrintersComboBox.SelectedItem.ToString();
        }
    }
}