using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectServicePageViewModel : PageViewModel
    {
        private const string DefaultServiceColor = "Blue";

        public event EventHandler<RenderServicesEventArgs> OnRenderServices;

        public ICommand LoadedCommand { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TerminalWindow Window { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        public SelectServicePageViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private void Loaded()
        {
            LoadRootServiceGroups();
        }

        private async void LoadRootServiceGroups()
        {
            var buttons = new List<SelectServiceButton>();

            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    AddGroupsToButtons(await TaskPool.AddTask(channel.Service.GetRootServiceGroups()), buttons);
                    AddServicesToButtons(await TaskPool.AddTask(channel.Service.GetRootServices()), buttons);
                }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }

            RenderServices(buttons, TerminalConfig.Columns, TerminalConfig.Rows);
        }

        private async void LoadServiceGroup(Guid serviceGroupId, int cols, int rows)
        {
            var buttons = new List<SelectServiceButton>();

            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    AddGroupsToButtons(await TaskPool.AddTask(channel.Service.GetServiceGroups(serviceGroupId)), buttons);
                    AddServicesToButtons(await TaskPool.AddTask(channel.Service.GetServices(serviceGroupId)), buttons);
                }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }

            RenderServices(buttons, cols, rows);
        }

        private void OnServiceGroupSelected(ServiceGroup group)
        {
            LoadServiceGroup(group.Id, group.Columns, group.Rows);
        }

        private void AddGroupsToButtons(ServiceGroup[] groups, List<SelectServiceButton> buttons)
        {
            foreach (var group in groups)
            {
                if (!group.IsActive)
                {
                    continue;
                }

                buttons.Add(CreateSelectServiceButton(group.Code, group.Name, group.Color, group.FontSize, (s, a) => OnServiceGroupSelected(group)));
            }
        }

        private void AddServicesToButtons(Service[] services, List<SelectServiceButton> buttons)
        {
            foreach (var service in services)
            {
                if (!service.IsActive)
                {
                    continue;
                }

                buttons.Add(CreateSelectServiceButton(service.Code,
                                                    service.Name,
                                                    service.ServiceGroup == null ?
                                                            DefaultServiceColor :
                                                            service.ServiceGroup.Color,
                                                    service.FontSize,
                                                    (s, a) => SetSelectedService(service)
                ));
            }
        }

        private async void SetSelectedService(Service service)
        {
            Model.RequestType = null;
            Model.SelectedService = service;

            bool liveTerminal = service.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal);
            bool earlyTerminal = service.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Terminal);
            if ((!liveTerminal) && (!earlyTerminal))
            {
                Window.ShowWarning(Translater.Message("ServiceNotAvailableOnTerminal"));
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
                var loading = Window.ShowLoading();

                try
                {
                    await Model.AdjustMaxSubjects();
                    Navigator.NextPage();
                }
                catch (FaultException exception)
                {
                    Window.ShowWarning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    Window.ShowWarning(exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }
            else
            {
                Navigator.NextPage();
            }
        }

        private SelectServiceButton CreateSelectServiceButton(string code, string name, string color, float fontSize, EventHandler onSelected)
        {
            var model = new ServiceButtonViewModel()
            {
                Code = code,
                Name = name,
                FontSize = fontSize,
                ServiceBrush = color.GetBrushForColor()
            };
            model.OnServiceSelected += onSelected;

            var result = new SelectServiceButton();
            result.DataContext = model;
            return result;
        }

        private void RenderServices(List<SelectServiceButton> services, int cols, int rows)
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
    }
}