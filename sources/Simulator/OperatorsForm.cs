using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Simulator
{
    public partial class OperatorsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IServerUserTcpService> ServerUser { get; set; }

        #endregion dependency

        private CancellationTokenSource cancellationTokenSource;
        private readonly Random random;
        private readonly TaskPool taskPool;

        public OperatorsForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
            random = new Random();
        }

        private void log(string message)
        {
            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    logTextBox.AppendText(string.Format("[{0:hh:mm:ss}] {1}", DateTime.Now, message) + Environment.NewLine);
                }));
            }
            catch { }
        }

        private void OperatorsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            taskPool.Cancel();
            ChannelManager.Dispose();
            ServerUser.Dispose();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            int delay = (int)delayUpDown.Value * 1000;

            using (var channel = ServerUser.CreateChannel())
            {
                try
                {
                    startButton.Enabled = false;

                    log("Получение списка пользователей");
                    var users = await taskPool.AddTask(channel.Service.GetUsers());

                    log(string.Format("Всего пользователей {0}", users.Length));

                    cancellationTokenSource = new CancellationTokenSource();
                    var token = cancellationTokenSource.Token;

                    foreach (var queueOperator in users.Where(u => u is QueueOperator))
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(random.Next(0, delay));

                            while (true)
                            {
                                token.ThrowIfCancellationRequested();

                                await Task.Run(() => UpdateCurrentClientRequest((QueueOperator)queueOperator));

                                await Task.Delay(random.Next(0, delay));
                            }
                        }, token);
                    }

                    stopButton.Enabled = true;
                }
                catch (Exception exception)
                {
                    log(exception.Message);

                    startButton.Enabled = true;
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }

        private async void UpdateCurrentClientRequest(QueueOperator queueOperator)
        {
            try
            {
                using (var channel = ServerUser.CreateChannel(queueOperator.SessionId))
                {
                    await taskPool.AddTask(channel.Service.UserHeartbeat());
                }

                using (var channel = ChannelManager.CreateChannel(queueOperator.SessionId))
                {
                    var clientRequestPlan = await channel.Service.GetCurrentClientRequestPlan();
                    if (clientRequestPlan != null)
                    {
                        ClientRequest clientRequest = clientRequestPlan.ClientRequest;

                        var state = ClientRequestState.Calling;

                        switch (clientRequest.State)
                        {
                            case ClientRequestState.Waiting:
                                break;

                            case ClientRequestState.Calling:
                                state = random.Next(1, 10) >= 5
                                    ? ClientRequestState.Rendering
                                    : ClientRequestState.Absence;
                                break;

                            case ClientRequestState.Rendering:
                                state = ClientRequestState.Rendered;
                                break;
                        }

                        await channel.Service.UpdateCurrentClientRequest(state);
                        if (state == ClientRequestState.Calling)
                        {
                            await channel.Service.CallCurrentClient();
                        }

                        log(string.Format("Установлен статус [{0}] для [{1}]", state, clientRequest));
                    }
                    else
                    {
                        log(string.Format("[{0}] = [нет активных запросов]", queueOperator));
                    }
                }
            }
            catch (FaultException exception)
            {
                log(string.Format("[{0}] = [{1}]", queueOperator, exception.Reason));
            }
            catch (Exception exception)
            {
                log(exception.ToString());
            }
        }
    }
}