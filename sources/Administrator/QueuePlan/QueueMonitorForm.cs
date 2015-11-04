using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Contracts;
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
        public LoginSettings Settings { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IServerUserTcpService> ServerUserChannelManager { get; set; }

        #endregion dependency

        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Timer reloadInterval;
        private readonly TaskPool taskPool;
        private DateTime planDate;

        #endregion fields

        public QueueMonitorForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            reloadInterval = new Timer();
            reloadInterval.Tick += reloadInterval_Tick;

            queueMonitorControl.OnOperatorLogin += queueMonitorControl_OperatorLogin;
            queueMonitorControl.OnClientRequestEdit += queueMonitorControl_ClientRequestEdit;
        }

        private void reloadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (reloadCheckBox.Checked)
            {
                reloadInterval.Interval = (int)reloadIntervalUpDown.Value * 1000;
                reloadInterval.Start();
                reloadIntervalUpDown.Enabled = false;
            }
            else
            {
                reloadInterval.Stop();
                reloadIntervalUpDown.Enabled = true;
            }
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    loadButton.Enabled = false;

                    queueMonitorControl.QueuePlan = await channel.Service.GetQueuePlan(planDate);
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

        private void operatorOnlneCheckBox_Leave(object sender, EventArgs e)
        {
            if (operatorOnlneCheckBox.Checked)
            {
                queueMonitorControl.Options |= QueueMonitorControlOptions.OperatorOnline;
            }
            else
            {
                queueMonitorControl.Options ^= QueueMonitorControlOptions.OperatorOnline;
            }
        }

        private void queueMonitorControl_ClientRequestEdit(object sender, QueueMonitorEventArgs e)
        {
            using (var f = new EditClientRequestForm(e.ClientRequest.Id))
            {
                f.ShowDialog();
            }
        }

        private async void queueMonitorControl_OperatorLogin(object sender, QueueMonitorEventArgs eventArgs)
        {
            try
            {
                using (var channel = ServerUserChannelManager.CreateChannel())
                {
                    var queueOperator = await channel.Service.GetUser(eventArgs.Operator.Id) as QueueOperator;

                    Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = "Queue.Operator.exe",
                        Arguments = string.Format("--AutoLogin --Endpoint=\"{0}\" --SessionId={1}",
                            Settings.Endpoint, queueOperator.SessionId),
                        WorkingDirectory = Environment.CurrentDirectory
                    });
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

        private void QueueMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            ChannelManager.Dispose();

            reloadInterval.Stop();
            reloadInterval.Dispose();
        }

        private void QueueMonitorForm_Load(object sender, EventArgs e)
        {
            planDate = ServerDateTime.Today;
            planDateTimePicker.Value = planDate;
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Перезагрузить текущий план очереди? В этом случае будет произведено полное обновление информации из базы данных. Продолжительность данной операции зависит от количества запросов клиентов.",
                "Подтвердите операцию", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var channel = ChannelManager.CreateChannel())
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

        private async void reloadInterval_Tick(object sender, EventArgs e)
        {
            reloadInterval.Stop();

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    queueMonitorControl.QueuePlan = await channel.Service.GetQueuePlan(planDate);
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    logger.Warn(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    logger.Error(exception.Message);
                }
                finally
                {
                    reloadInterval.Start();
                }
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            queueMonitorControl.Search((int)numberUpDown.Value);
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void planDateTimePicker_Leave(object sender, EventArgs e)
        {
            planDate = planDateTimePicker.Value;
        }
    }
}