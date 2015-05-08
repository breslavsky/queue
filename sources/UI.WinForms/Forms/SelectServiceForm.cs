using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class SelectServiceForm : RichForm
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        #endregion fields

        #region properties

        public Service Service
        {
            get { return serviceControl.Service; }
        }

        #endregion properties

        public SelectServiceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            serviceControl.Initialize(channelBuilder, currentUser);
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
    }
}