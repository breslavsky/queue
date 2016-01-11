using Junte.Translation;
using Junte.UI.WPF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Model.Common;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.WPF;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectServicePageViewModel : PageViewModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool showServices;
        private bool showLifeSituations;

        public ICommand ShowLifeSituationsCommand { get; set; }

        public ICommand ShowServicesCommand { get; set; }

        public bool ShowPagesSelector { get; set; }

        public bool ShowServices
        {
            get { return showServices; }
            set { SetProperty(ref showServices, value); }
        }

        public bool ShowLifeSituations
        {
            get { return showLifeSituations; }
            set { SetProperty(ref showLifeSituations, value); }
        }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        public SelectServicePageViewModel()
            : base()
        {
            ShowLifeSituationsCommand = new RelayCommand(() =>
            {
                ShowServices = false;
                ShowLifeSituations = true;
            });

            ShowServicesCommand = new RelayCommand(() =>
            {
                ShowServices = true;
                ShowLifeSituations = false;
            });

            ApplyConfig();
        }

        private void ApplyConfig()
        {
            ShowPagesSelector = TerminalConfig.Pages.HasFlag(TerminalPages.Services) &&
                                TerminalConfig.Pages.HasFlag(TerminalPages.LifeSituations);

            if (TerminalConfig.StartPage == TerminalPages.Services)
            {
                ShowServices = true;
            }
            else
            {
                ShowLifeSituations = true;
            }
        }

        public async void SetSelectedService(Service service)
        {
            logger.Debug("выбрана услуга [{0}]", service);

            Model.RequestType = null;
            Model.SelectedService = service;

            bool liveTerminal = service.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal);
            bool earlyTerminal = service.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Terminal);
            if (!liveTerminal && !earlyTerminal)
            {
                Window.Warning(Translater.Message("ServiceNotAvailableOnTerminal"));
                return;
            }

            if (liveTerminal && !earlyTerminal)
            {
                Model.RequestType = ClientRequestType.Live;
            }
            else if (!liveTerminal && earlyTerminal)
            {
                Model.RequestType = ClientRequestType.Early;
            }

            if (Model.RequestType != null)
            {
                try
                {
                    await Model.AdjustMaxSubjects();
                }
                catch { };
            }

            Navigator.NextPage();
        }
    }
}