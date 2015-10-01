﻿using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Contracts;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator
{
    public partial class CallClientByRequestNumberForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueOperator CurrentOperator { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DuplexChannelManager<IServerTcpService> serverChannelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public CallClientByRequestNumberForm()
            : base()
        {
            InitializeComponent();

            serverChannelManager = ServerService.CreateChannelManager(CurrentOperator.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        #region task pool

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #endregion task pool

        private async void submitButton_Click(object sender, EventArgs e)
        {
            var clientRequestNumber = (int)clientRequestNumberUpDown.Value;
            if (clientRequestNumber > 0)
            {
                using (var channel = serverChannelManager.CreateChannel())
                {
                    try
                    {
                        submitButton.Enabled = false;

                        await taskPool.AddTask(channel.Service.CallClientByRequestNumber(clientRequestNumber));

                        DialogResult = DialogResult.OK;
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
                        submitButton.Enabled = true;
                    }
                }
            }
        }
    }
}