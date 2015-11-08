using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.Extensions;
using Queue.Terminal.UserControls;
using Queue.UI.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SearchServiceResultsViewModel : RichViewModel, IServiceSearch
    {
        private const int ResultsColCount = 2;
        private const int ResultsRowCount = 3;
        private const int ResultsPerPage = ResultsColCount * ResultsRowCount;

        private Grid resultsGrid;
        private bool hasNext;
        private bool hasPrev;
        private bool hasResults;
        private int currentPage;
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

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public ClientRequestModel Request { get; set; }

        public SearchServiceResultsViewModel()
            : base()
        {
            NextCommand = new RelayCommand(Next);
            PrevCommand = new RelayCommand(Prev);
            UnloadedCommand = new RelayCommand(Unloaded);
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

            var services = await Window.ExecuteLongTask(async () =>
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    return await channel.Service.FindServices(filter, pageNo * ResultsPerPage, ResultsPerPage);
                };
            });

            RenderServices(services);
            HasNext = services.Length == ResultsPerPage;
            if (pageNo == 0)
            {
                HasResults = services.Length > 0;
            }

            currentPage = pageNo;
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
                                                                Request.SelectedService = service;
                                                                Navigator.NextPage();
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
            var model = new ServiceButtonViewModel()
            {
                Code = service.Code,
                Name = service.Name,
                FontSize = service.FontSize == 0 ? 1 : service.FontSize,
                ServiceBrush = service.GetColor().GetBrushForColor()
            };
            model.OnServiceSelected += onSelected;

            return new SelectServiceButton()
            {
                DataContext = model
            };
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