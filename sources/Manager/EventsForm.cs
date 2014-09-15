using Junte.Parallel.Common;
using Junte.WCF.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using QueueManager = Queue.Services.DTO.Manager;
using Timer = System.Timers.Timer;

namespace Queue.Manager
{
    public partial class EventsForm : RichForm
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private QueueManager currentManager;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private ServerCallback callbackObject;
        private Channel<IServerService> pingChannel;

        private Timer pingTimer;
        private int PING_INTERVAL = 10000;

        public EventsForm(DuplexChannelBuilder<IServerService> channelBuilder, QueueManager currentManager)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentManager = currentManager;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();

            Text = currentManager.ToString();

            callbackObject = new ServerCallback();
            callbackObject.OnEvent += callbackObject_QueueEvent;

            pingChannel = channelManager.CreateChannel(callbackObject);

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;
        }

        private void Events_Load(object sender, EventArgs e)
        {
            pingTimer.Start();
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
                Invoke(new MethodInvoker(async () =>
                {
                    try
                    {
                        if (!pingChannel.IsConnected)
                        {
                            await taskPool.AddTask(pingChannel.Service.OpenUserSession(currentManager.SessionId));
                            pingChannel.Service.Subscribe(ServerServiceEventType.Event);
                        }
                        else
                        {
                            await taskPool.AddTask(pingChannel.Service.UserHeartbeat());
                        }

                        await taskPool.AddTask(pingChannel.Service.GetDateTime());
                    }
                    catch
                    {
                        pingChannel.Dispose();
                        pingChannel = channelManager.CreateChannel(callbackObject);
                    }
                    finally
                    {
                        pingTimer.Start();
                    }
                }));
            }
            catch
            {
                // nothing
            }
        }

        private void callbackObject_QueueEvent(object sender, ServerEventArgs e)
        {
            Task.Run(() =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    int index = 0;
                    eventsGridView.Rows.Insert(index, 1);
                    var row = eventsGridView.Rows[index];

                    var queueEvent = e.Event;

                    row.Cells["createDateColumn"].Value = queueEvent.CreateDate;
                    row.Cells["messageColumn"].Value = queueEvent.Message;
                    row.Tag = queueEvent;
                }));
            });
        }

        private void EventsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            pingTimer.Stop();
            taskPool.Dispose();
            channelManager.Dispose();
            pingChannel.Dispose();
        }
    }
}