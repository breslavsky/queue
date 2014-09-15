using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ExceptionScheduleReportForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        public ExceptionScheduleReportForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void ExceptionScheduleReportForm_Load(object sender, EventArgs e)
        {
            fromDateCalendar.SelectionStart = ServerDateTime.Today;
        }

        private async void createReportButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    createReportButton.Enabled = false;

                    DateTime fromDate = fromDateCalendar.SelectionStart;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    byte[] report = await taskPool.AddTask(channel.Service.GetExceptionScheduleReport(fromDate));
                    string path = Path.GetTempPath() + Path.GetRandomFileName() + ".xls";

                    FileStream file = File.OpenWrite(path);
                    file.Write(report, 0, report.Length);
                    file.Close();

                    Process.Start(path);
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
                    createReportButton.Enabled = true;
                }
            }
        }

        private void ExceptionScheduleReportForm_FormClosing(object sender, FormClosingEventArgs e)
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