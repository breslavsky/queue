using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using System;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SearchServiceResultsViewModel : ObservableObject, IServiceSearch
    {
        private const int ResultsColCount = 2;
        private const int ResultsRowCount = 3;
        private const int ResultsPerPage = ResultsColCount * ResultsRowCount;
        private const string DefaultServiceColor = "Blue";

        private Grid resultsGrid;
        private bool hasNext;
        private bool hasPrev;
        private bool hasResults;
        private int currentPage;

        private TerminalWindow screen;
        private DuplexChannelManager<IServerTcpService> channelManager;
        private Navigator navigator;
        private ClientRequestModel request;

        private string filter;

        public bool HasNext
        {
            get { return hasNext; }
            set { SetProperty(ref hasNext, value); }
        }

        public bool HasPrev
        {
            get { return hasPrev; }
            set { SetProperty(ref hasPrev, value); }
        }

        public bool HasResults
        {
            get { return hasResults; }
            set { SetProperty(ref hasResults, value); }
        }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public SearchServiceResultsViewModel()
        {
            NextCommand = new RelayCommand(Next);
            PrevCommand = new RelayCommand(Prev);

            screen = ServiceLocator.Current.GetInstance<TerminalWindow>();
            channelManager = ServiceLocator.Current.GetInstance<DuplexChannelManager<IServerTcpService>>();
            navigator = ServiceLocator.Current.GetInstance<Navigator>();
            request = ServiceLocator.Current.GetInstance<ClientRequestModel>();
        }

        public void Initialize(Grid resultsGrid)
        {
            this.resultsGrid = resultsGrid;
        }

        public void Search(string filter)
        {
            this.filter = filter;
            ShowResultPage(0);
        }

        private void Prev()
        {
            ShowResultPage(--currentPage);
        }

        private void Next()
        {
            ShowResultPage(++currentPage);
        }

        private async void ShowResultPage(int pageNo)
        {
            HasPrev = pageNo > 0;
            using (var channel = channelManager.CreateChannel())
            {
                var loading = screen.ShowLoading();

                try
                {
                    var services = await channel.Service.FindServices(filter, pageNo * ResultsPerPage, ResultsPerPage);

                    RenderServices(services);
                    HasNext = services.Length == ResultsPerPage;
                    if (pageNo == 0)
                    {
                        HasResults = services.Length > 0;
                    }

                    currentPage = pageNo;
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
        }

        private void RenderServices(Service[] services)
        {
            resultsGrid.Children.Clear();
            resultsGrid.RowDefinitions.Clear();
            resultsGrid.RowDefinitions.Add(new RowDefinition());

            resultsGrid.ColumnDefinitions.Clear();

            int row = 0;
            int col = 0;

            foreach (var service in services)
            {
                var button = CreateServiceButton(service, (s, a) =>
                                                            {
                                                                request.SelectedService = service;
                                                                navigator.NextPage();
                                                            });

                if (col >= ResultsColCount)
                {
                    col = 0;
                    resultsGrid.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                else
                {
                    if (col + 1 > resultsGrid.ColumnDefinitions.Count)
                    {
                        resultsGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }

                button.SetValue(Grid.ColumnProperty, col++);
                button.SetValue(Grid.RowProperty, row);
                resultsGrid.Children.Add(button);
            }
        }

        private SelectServiceButton CreateServiceButton(Service service, EventHandler onSelected)
        {
            var color = service.ServiceGroup == null ?
                                    DefaultServiceColor :
                                    service.ServiceGroup.Color;

            var model = new ServiceButtonViewModel()
            {
                Code = service.Code,
                Name = service.Name,
                FontSize = service.FontSize,
                ServiceBrush = color.GetBrushForColor()
            };
            model.OnServiceSelected += onSelected;

            return new SelectServiceButton()
            {
                DataContext = model
            };
        }
    }
}