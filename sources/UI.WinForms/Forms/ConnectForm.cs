﻿using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class ConnectForm : RichForm
    {
        public bool IsRemember;

        private DuplexChannelManager<IServerTcpService> channelManager;

        private TaskPool taskPool;

        public ConnectForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
        }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        public string Endpoint
        {
            get { return endpointTextBox.Text; }
            set { endpointTextBox.Text = value; }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (IsRemember)
            {
                Connect();
            }
        }

        private async void Connect()
        {
            try
            {
                ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
                channelManager = new DuplexChannelManager<IServerTcpService>(ChannelBuilder);

                using (var channel = channelManager.CreateChannel())
                {
                    connectButton.Enabled = false;

                    await channel.Service.GetDateTime();
                    DialogResult = DialogResult.OK;
                }
            }
            catch (OperationCanceledException) { }
            catch (CommunicationObjectAbortedException) { }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (FaultException ex)
            {
                UIHelper.Warning(ex.Reason.ToString());
            }
            catch (Exception ex)
            {
                UIHelper.Warning(ex.Message);
            }
            finally
            {
                connectButton.Enabled = true;
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void serverTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Connect();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            if (channelManager != null)
            {
                channelManager.Dispose();
            }
        }
    }
}