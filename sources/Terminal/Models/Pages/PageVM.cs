using Junte.Parallel.Common;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.WPF.Types;

namespace Queue.Terminal.Models.Pages
{
    public abstract class PageVM : ObservableObject
    {
        protected ClientRequestModel model;
        protected TaskPool taskPool;
        protected ChannelManager<IServerTcpService> channelManager;
        protected IRichPage screen;
        protected TerminalConfig terminalConfig;
        protected Navigator navigator;

        public PageVM()
        {
            this.Model = ServiceLocator.Current.GetInstance<ClientRequestModel>();
            this.taskPool = ServiceLocator.Current.GetInstance<TaskPool>();
            this.screen = ServiceLocator.Current.GetInstance<IRichPage>();
            this.navigator = ServiceLocator.Current.GetInstance<Navigator>();
            this.channelManager = ServiceLocator.Current.GetInstance<ChannelManager<IServerTcpService>>();
            this.terminalConfig = ServiceLocator.Current.GetInstance<TerminalConfig>();
        }

        public ClientRequestModel Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }
}