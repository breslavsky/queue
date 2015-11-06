using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Simulator
{
    public partial class ClientRequstsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        private const int Subjects = 1;
        private CancellationTokenSource cancellationTokenSource;
        private readonly TaskPool taskPool;
        private readonly Random random;

        public ClientRequstsForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
            random = new Random();
        }

        private async void addClientRequest(Service service)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                string surname = string.Format("client {0}", random.Next());

                try
                {
                    var client = await channel.Service.EditClient(new Client()
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

            taskPool.Cancel();
            ChannelManager.Dispose();
        }

        private async Task<Service[]> getServices(ServiceGroup serviceGroup = null)
        {
            using (var channel = ChannelManager.CreateChannel())
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
                else
                {
                    services.AddRange(await taskPool.AddTask(channel.Service.GetRootServices()));
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