﻿using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class PortalConfigControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;
        private PortalConfig config;

        #endregion fields

        #region properties

        public PortalConfig Config
        {
            get
            {
                return config;
            }
            private set
            {
                config = value;
                if (config != null)
                {
                    currentDayRecordingCheckBox.Checked = config.CurrentDayRecording;
                }
            }
        }

        #endregion properties

        public PortalConfigControl()
        {
            InitializeComponent();

            config = new PortalConfig();

            if (designtime)
            {
                return;
            }

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private async void PortalConfigControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetPortalConfig());
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

        private void portalCurrentDayRecordingCheckBox_Leave(object sender, EventArgs e)
        {
            config.CurrentDayRecording = currentDayRecordingCheckBox.Checked;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditPortalConfig(config));
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
                    saveButton.Enabled = true;
                }
            }
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}