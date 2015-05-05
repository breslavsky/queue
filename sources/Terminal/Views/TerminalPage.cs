using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.WPF.Types;
using System;
using System.Windows.Controls;

namespace Queue.Terminal.Views
{
    public abstract class TerminalPage : Page
    {
        protected ClientRequestModel terminalModel;
        protected TaskPool taskPool;
        protected ChannelManager<IServerTcpService> channelManager;
        protected IMainWindow screen;
        protected TerminalConfig terminalConfig;
        protected Navigator navigator;

        public TerminalPage()
        {
            terminalModel = ServiceLocator.Current.GetInstance<ClientRequestModel>();
            taskPool = ServiceLocator.Current.GetInstance<TaskPool>();
            screen = ServiceLocator.Current.GetInstance<IMainWindow>();
            navigator = ServiceLocator.Current.GetInstance<Navigator>();
            channelManager = ServiceLocator.Current.GetInstance<ChannelManager<IServerTcpService>>();
            terminalConfig = ServiceLocator.Current.GetInstance<TerminalConfig>();

            DataContext = Activator.CreateInstance(ModelType);
        }

        protected abstract Type ModelType { get; }
    }
}