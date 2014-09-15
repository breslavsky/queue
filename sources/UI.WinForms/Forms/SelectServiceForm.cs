using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class SelectServiceForm : Queue.UI.WinForms.RichForm
    {
        #region fields

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        #endregion fields

        #region properties

        public Service SelectedService { get { return selectServiceControl.SelectedService; } }

        #endregion properties

        public SelectServiceForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();

            selectServiceControl.Initialize(channelBuilder, currentUser);
        }

        private void selectServiceControl_ServiceSelected(object sender, EventArgs e)
        {
            if (SelectedService != null)
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