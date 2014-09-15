using Junte.UI.WPF;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Types;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using Queue.UI.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Queue.Terminal.Models.Pages
{
    public class SelectServicePageVM : PageVM
    {
        private const string DefaultServiceColor = "Blue";

        public event EventHandler<RenderServicesEventArgs> OnRenderServices;

        public void Initialize()
        {
            LoadRootServiceGroups();
        }

        private async void LoadRootServiceGroups()
        {
            List<SelectServiceButton> buttons = new List<SelectServiceButton>();

            using (Channel<IServerService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    AddGroupsToButtons(await taskPool.AddTask(channel.Service.GetRootServiceGroups()), buttons);
                    AddServicesToButtons(await taskPool.AddTask(channel.Service.GetRootServices()), buttons);
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

            RenderServices(buttons, terminalConfig.Columns, terminalConfig.Rows);
        }

        private async void LoadServiceGroup(Guid serviceGroupId, int cols, int rows)
        {
            List<SelectServiceButton> buttons = new List<SelectServiceButton>();

            using (var channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    AddGroupsToButtons(await taskPool.AddTask(channel.Service.GetServiceGroups(serviceGroupId)), buttons);
                    AddServicesToButtons(await taskPool.AddTask(channel.Service.GetServices(serviceGroupId)), buttons);
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
            foreach (ServiceGroup group in groups)
            {
                if (!group.IsActive)
                {
                    continue;
                }

                buttons.Add(CreateSelectServiceButton(group.Code, group.Name, group.Color, (s, a) => OnServiceGroupSelected(group)));
            }
        }

        private void AddServicesToButtons(Service[] services, List<SelectServiceButton> buttons)
        {
            foreach (Service service in services)
            {
                if (!service.IsActive)
                {
                    continue;
                }

                buttons.Add(CreateSelectServiceButton(service.Code, service.Name, service.ServiceGroup == null ? DefaultServiceColor : service.ServiceGroup.Color, (s, a) =>
                {
                    Model.SelectedService = service;
                    navigator.NextPage();
                }));
            }
        }

        private SelectServiceButton CreateSelectServiceButton(string code, string name, string color, EventHandler onSelected)
        {
            SelectServiceButton result = new SelectServiceButton();

            ServiceButtonVM model = new ServiceButtonVM()
            {
                Code = code,
                Name = name,
                ServiceBrush = color.GetBrushForColor()
            };
            model.OnServiceSelected += onSelected;

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