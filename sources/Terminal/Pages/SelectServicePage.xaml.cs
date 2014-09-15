using Queue.Terminal.Core;
using Queue.Terminal.Models.Pages;
using Queue.Terminal.Types;
using System;
using System.Windows;

namespace Queue.Terminal.Pages
{
    public partial class SelectServicePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectServicePageVM); } }

        private SelectServicePageVM viewModel;
        private ServicesPager pager;

        public SelectServicePage() :
            base()
        {
            InitializeComponent();

            viewModel = DataContext as SelectServicePageVM;
            viewModel.OnRenderServices += RenderServices;

            pager = new ServicesPager(this);

            prevButton.DataContext = pager;
            nextButton.DataContext = pager;
        }

        private void RenderServices(object sender, RenderServicesEventArgs args)
        {
            pager.UpdateServices(args.Services, args.Cols, args.Rows);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.Initialize();
        }
    }
}