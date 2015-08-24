using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using NLog;
using Queue.Common;
using Queue.Operator;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class QueueMonitorForm : RichForm
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public QueueMonitorForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
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

        public QueuePlan QueuePlan
        {
            set
            {
                if (value != null)
                {
                    queueMonitorControl.LoadQueuePlan(value);
                }
            }
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

                    var form = new OperatorForm(channelBuilder, queueOperator);
                    FormClosing += (s, e) =>
                    {
                        form.Close();
                    };
                    form.Show();
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
            using (var f = new EditClientRequestForm(channelBuilder, currentUser, e.ClientRequest.Id))
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

                    QueuePlan = queuePlan;
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