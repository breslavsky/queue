using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
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
    public partial class RedirectToOperatorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueOperator CurrentOperator { get; set; }

        [Dependency]
        public ChannelManager<IUserTcpService> UserChannelManager { get; set; }

        [Dependency]
        public DuplexChannelManager<IQueuePlanTcpService> QueuePlanChannelManager { get; set; }

        #endregion dependency

        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TaskPool taskPool;

        #endregion fields

        public RedirectToOperatorForm()
            : base()
        {
            InitializeComponent();

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

        private async void RedirectToOperatorForm_Load(object sender, EventArgs e)
        {
            using (var channel = UserChannelManager.CreateChannel())
            {
                try
                {
                    redirectOperatorControl.Initialize(await taskPool.AddTask(channel.Service.GetRedirectOperatorsLinks()));
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

        private async void submitButton_Click(object sender, EventArgs e)
        {
            var redirectOperator = redirectOperatorControl.Selected<QueueOperator>();
            if (redirectOperator != null)
            {
                try
                {
                    submitButton.Enabled = false;

                    using (var channel = QueuePlanChannelManager.CreateChannel())
                    {
                        await taskPool.AddTask(channel.Service.RedirectToOperator(redirectOperator.Id));
                    }

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