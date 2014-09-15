using Junte.Parallel.Common;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.WPF.Types;
using System;
using System.Windows.Controls;

namespace Queue.Terminal.Pages
{
    public abstract class TerminalPage : Page
    {
        protected ClientRequestModel terminalModel;
        protected TaskPool taskPool;
        protected ChannelManager<IServerService> channelManager;
        protected IRichPage screen;
        protected TerminalConfig terminalConfig;
        protected Navigator navigator;

        public TerminalPage()
        {
            this.terminalModel = ServiceLocator.Current.GetInstance<ClientRequestModel>();
            this.taskPool = ServiceLocator.Current.GetInstance<TaskPool>();
            this.screen = ServiceLocator.Current.GetInstance<IRichPage>();
            this.navigator = ServiceLocator.Current.GetInstance<Navigator>();
            this.channelManager = ServiceLocator.Current.GetInstance<ChannelManager<IServerService>>();
            this.terminalConfig = ServiceLocator.Current.GetInstance<TerminalConfig>();

            DataContext = Activator.CreateInstance(ModelType);
        }

        protected abstract Type ModelType { get; }
    }
}