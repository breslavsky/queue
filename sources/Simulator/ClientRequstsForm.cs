using Junte.Parallel.Common;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Simulator
{
    public partial class ClientRequstsForm : Queue.UI.WinForms.RichForm
    {
        private const int Subjects = 1;
        private CancellationTokenSource cancellationTokenSource;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Random random;
        private TaskPool taskPool;

        public ClientRequstsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            random = new Random();
        }

        private async void addClientRequest(Service service)
        {
            using (var channel = channelManager.CreateChannel())
            {
                string surname = string.Format("client {0}", random.Next());

                try
                {
                    Client client = await channel.Service.EditClient(new Client()
                    {
                        Surname = surname
                    });

                    var isPriority = random.Next(0, 1) == 1;

                    var clientRequest = await channel.Service.AddLiveClientRequest(client.Id, service.Id, isPriority,
                        new Dictionary<Guid, object>() { }, Subjects);
                    log(string.Format("Добавлен запроса клиента {0}", clientRequest));
                }
                catch (FaultException exception)
                {
                    log(string.Format("{0} = {1}", service, exception.Reason));
                }
                catch (Exception exception)
                {
                    log(exception.ToString());
                }
            }
        }

        private void ClientRequstsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async Task<Service[]> getServices(ServiceGroup serviceGroup = null)
        {
            using (var channel = channelManager.CreateChannel())
            {
                var serviceGroups = serviceGroup != null
                    ? await taskPool.AddTask(channel.Service.GetServiceGroups(serviceGroup.Id))
                    : await taskPool.AddTask(channel.Service.GetRootServiceGroups());

                var services = new List<Service>();

                foreach (var g in serviceGroups)
                {
                    services.AddRange(await getServices(g));
                }

                if (serviceGroup != null)
                {
                    services.AddRange(await taskPool.AddTask(channel.Service.GetServices(serviceGroup.Id)));
                }

                return services.ToArray();
            }
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

        private async void startButton_Click(object sender, EventArgs e)
        {
            int delay = (int)delayUpDown.Value * 1000;

            try
            {
                startButton.Enabled = false;

                log("Получение списка услуг");
                var services = await getServices();

                log(string.Format("Всего услуг {0}", services.Length));

                cancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = cancellationTokenSource.Token;

                for (int i = 0; i <= workersUpDown.Value; i++)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(random.Next(0, delay));

                        while (true)
                        {
                            token.ThrowIfCancellationRequested();

                            int number = random.Next(0, services.Length - 1);

                            var service = services[number];

                            await Task.Run(() => addClientRequest(service));

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

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }
    }
}