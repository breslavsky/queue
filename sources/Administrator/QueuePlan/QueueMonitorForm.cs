using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class QueueMonitorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public LoginSettings LoginSettings { get; set; }

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public QueueMonitorForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            queueMonitorControl.Options = QueueMonitorControlOptions.ClientRequestEdit
                | QueueMonitorControlOptions.OperatorLogin;

            queueMonitorControl.OnOperatorLogin += queueMonitorControl_OperatorLogin;
            queueMonitorControl.OnClientRequestEdit += queueMonitorControl_ClientRequestEdit;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void QueueMonitorForm_Load(object sender, EventArgs e)
        {
            planDateTimePicker.Value = ServerDateTime.Today;
        }

        private async void queueMonitorControl_OperatorLogin(object sender, QueueMonitorEventArgs eventArgs)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var queueOperator = await channel.Service.GetUser(eventArgs.Operator.Id) as QueueOperator;

                    Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = "Queue.Operator.exe",
                        Arguments = string.Format("--AutoLogin --Endpoint=\"{0}\" --SessionId={1}",
                            LoginSettings.Endpoint, queueOperator.SessionId),
                        WorkingDirectory = Environment.CurrentDirectory
                    });
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

        private void queueMonitorControl_ClientRequestEdit(object sender, QueueMonitorEventArgs e)
        {
            using (var f = new EditClientRequestForm(e.ClientRequest.Id))
            {
                f.ShowDialog();
            }
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    loadButton.Enabled = false;

                    var planDate = planDateTimePicker.Value.Date;
                    var queuePlan = await channel.Service.GetQueuePlan(planDate);

                    var isToday = planDate == ServerDateTime.Today;

                    if (isToday)
                    {
                        if (!queueMonitorControl.Options.HasFlag(QueueMonitorControlOptions.OperatorLogin))
                        {
                            queueMonitorControl.Options |= QueueMonitorControlOptions.OperatorLogin;
                        }
                    }
                    else
                    {
                        if (queueMonitorControl.Options.HasFlag(QueueMonitorControlOptions.OperatorLogin))
                        {
                            queueMonitorControl.Options ^= QueueMonitorControlOptions.OperatorLogin;
                        }
                    }

                    queueMonitorControl.QueuePlan = queuePlan;
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
                    loadButton.Enabled = true;
                }
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            queueMonitorControl.Search((int)numberUpDown.Value);
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Перезагрузить текущий план очереди? В этом случае будет произведено полное обновление информации из базы данных. Продолжительность данной операции зависит от количества запросов клиентов.",
                "Подтвердите операцию", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await channel.Service.RefreshTodayQueuePlan();
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
        }

        private void QueueMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }
    }
}