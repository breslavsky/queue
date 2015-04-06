using Junte.Parallel.Common;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using Timer = System.Timers.Timer;

namespace Queue.Hub
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;

        private ChannelManager<IServerTcpService> channelManager;

        private TaskPool taskPool;

        private ServerCallback callbackObject;

        private Channel<IServerTcpService> pingChannel;

        private Timer pingTimer;

        private int PING_INTERVAL = 10000;

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            callbackObject = new ServerCallback();
            callbackObject.OnConfigUpdated += callbackObject_OnConfigUpdated;

            pingChannel = channelManager.CreateChannel(callbackObject);

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;
        }

        public bool IsLogout { get; private set; }

        private Controller controller
        {
            get
            {
                return ControllerManager.Current;
            }
        }

        private void callbackObject_OnConfigUpdated(object sender, ServerEventArgs e)
        {
            Debug.WriteLine(e.Config);
        }

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval < PING_INTERVAL)
            {
                pingTimer.Interval = PING_INTERVAL;
            }

            try
            {
                Invoke((MethodInvoker)async delegate
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        if (!pingChannel.IsConnected)
                        {
                            pingChannel.Service.Subscribe(ServerServiceEventType.ConfigUpdated, new ServerSubscribtionArgs()
                            {
                                ConfigTypes = new ConfigType[] { ConfigType.Media }
                            });
                        }

                        ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

                        serverStateLabel.Image = QIcons.online16x16;
                    }
                    catch (Exception exception)
                    {
                        currentDateTimeLabel.Text = exception.Message;
                        serverStateLabel.Image = QIcons.offline16x16;

                        pingChannel.Dispose();
                        pingChannel = channelManager.CreateChannel(callbackObject);
                    }
                    finally
                    {
                        pingTimer.Start();
                    }
                });
            }
            catch (Exception)
            {
                // disposed
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pingTimer.Start();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }
    }
}