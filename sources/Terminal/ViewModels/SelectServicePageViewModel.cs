using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.Extensions;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectServicePageViewModel : PageViewModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<RenderServicesEventArgs> OnRenderServices;

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        public SelectServicePageViewModel()
            : base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            LoadRootServiceGroups();
        }

        private async void LoadRootServiceGroups()
        {
            ServiceGroup[] groups = null;
            Service[] services = null;

            await Window.ExecuteLongTask(async () =>
             {
                 using (var channel = ChannelManager.CreateChannel())
                 {
                     groups = await channel.Service.GetRootServiceGroups();
                     services = await channel.Service.GetRootServices();
                 }
             });

            RenderServices(CreateControlsForGroupsAndServices(groups, services), TerminalConfig.Columns, TerminalConfig.Rows);
        }

        private async void LoadServiceGroup(Guid serviceGroupId, int cols, int rows)
        {
            ServiceGroup[] groups = null;
            Service[] services = null;

            await Window.ExecuteLongTask(async () =>
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    groups = await channel.Service.GetServiceGroups(serviceGroupId);
                    services = await channel.Service.GetServices(serviceGroupId);
                }
            });

            RenderServices(CreateControlsForGroupsAndServices(groups, services), cols, rows);
        }

        private SelectServiceButton[] CreateControlsForGroupsAndServices(ServiceGroup[] groups, Service[] services)
        {
            var buttons = new List<SelectServiceButton>();
            if (groups != null)
            {
                foreach (var group in groups.Where(g => g.IsActive))
                {
                    buttons.Add(CreateSelectServiceButton(group.Code, group.Name, group.Color, group.FontSize, (s, a) => OnServiceGroupSelected(group)));
                }
            }
            if (services != null)
            {
                foreach (var service in services.Where(s => s.IsActive))
                {
                    buttons.Add(CreateSelectServiceButton(service.Code,
                                                        service.Name,
                                                        service.GetColor(),
                                                        service.FontSize,
                                                        (s, a) => SetSelectedService(service)
                    ));
                }
            }

            return buttons.ToArray();
        }

        private void OnServiceGroupSelected(ServiceGroup group)
        {
            LoadServiceGroup(group.Id, group.Columns, group.Rows);
        }

        private void AddServicesToButtons(Service[] services, List<SelectServiceButton> buttons)
        {
            foreach (var service in services.Where(s => s.IsActive))
            {
                buttons.Add(CreateSelectServiceButton(service.Code,
                                                    service.Name,
                                                    service.GetColor(),
                                                    service.FontSize,
                                                    (s, a) => SetSelectedService(service)
                ));
            }
        }

        private async void SetSelectedService(Service service)
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

        private SelectServiceButton CreateSelectServiceButton(string code, string name, string color, float fontSize, EventHandler onSelected)
        {
            var model = new ServiceButtonViewModel()
            {
                Code = code,
                Name = name,
                FontSize = fontSize == 0 ? 1 : fontSize,
                ServiceBrush = color.GetBrushForColor()
            };
            model.OnServiceSelected += onSelected;

            var result = new SelectServiceButton();
            result.DataContext = model;
            return result;
        }

        private void RenderServices(SelectServiceButton[] services, int cols, int rows)
        {
            if (OnRenderServices != null)
            {
                OnRenderServices(this, new RenderServicesEventArgs()
                {
                    Services = services,
                    Cols = cols,
                    Rows = rows
                });
            }
        }

        private void Unloaded()
        {
            try
            {
                ChannelManager.Dispose();
            }
            catch { }
        }
    }
}