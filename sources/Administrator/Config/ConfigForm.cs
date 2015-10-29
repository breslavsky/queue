using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ConfigForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;

        #endregion fields

        public ConfigForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}