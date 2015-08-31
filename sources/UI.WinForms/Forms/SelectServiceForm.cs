using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class SelectServiceForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public User CurrentUser { get; set; }

        [Dependency]
        public IClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        #endregion fields

        #region properties

        public Service Service
        {
            get { return serviceControl.SelectedService; }
        }

        #endregion properties

        public SelectServiceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void serviceControl_Selected(object sender, EventArgs e)
        {
            if (Service != null)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
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