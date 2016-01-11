using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Extensions;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectLifeSituationViewModel : RichViewModel
    {
        public event EventHandler<RenderServicesEventArgs> OnRenderServices = delegate { };

        public event EventHandler<Service> OnServiceSelected = delegate { };

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public ChannelManager<ILifeSituationTcpService> ChannelManager { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        public SelectLifeSituationViewModel()
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
            LifeSituationGroup[] groups = null;
            LifeSituation[] situations = null;

            await Window.ExecuteLongTask(async () =>
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    groups = await channel.Service.GetRootGroups();
                    situations = await channel.Service.GetRootLifeSituations();
                }
            });

            RenderServices(CreateControlsForGroupsAndSituations(groups, situations), TerminalConfig.Columns, TerminalConfig.Rows);
        }

        private async void LoadGroup(Guid serviceGroupId, int cols, int rows)
        {
            LifeSituationGroup[] groups = null;
            LifeSituation[] services = null;

            await Window.ExecuteLongTask(async () =>
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    groups = await channel.Service.GetGroups(serviceGroupId);
                    services = await channel.Service.GetLifeSituations(serviceGroupId);
                }
            });

            RenderServices(CreateControlsForGroupsAndSituations(groups, services), cols, rows);
        }

        private SelectServiceButton[] CreateControlsForGroupsAndSituations(LifeSituationGroup[] groups, LifeSituation[] services)
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
                                                        (s, a) => SetSelectedLifeSituation(service)
                    ));
                }
            }

            return buttons.ToArray();
        }

        private void OnServiceGroupSelected(LifeSituationGroup group)
        {
            LoadGroup(group.Id, group.Columns, group.Rows);
        }

        private void AddSituationsToButtons(LifeSituation[] services, List<SelectServiceButton> buttons)
        {
            foreach (var service in services.Where(s => s.IsActive))
            {
                buttons.Add(CreateSelectServiceButton(service.Code,
                                                    service.Name,
                                                    service.GetColor(),
                                                    service.FontSize,
                                                    (s, a) => SetSelectedLifeSituation(service)
                ));
            }
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
            OnRenderServices(this, new RenderServicesEventArgs()
            {
                Services = services,
                Cols = cols,
                Rows = rows
            });
        }

        private void SetSelectedLifeSituation(LifeSituation situation)
        {
            OnServiceSelected(this, situation.Service);
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