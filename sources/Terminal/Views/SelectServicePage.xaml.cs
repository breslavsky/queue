using Queue.Terminal.Core;
using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class SelectServicePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectServicePageViewModel); } }

        private ServicesPager pager;

        public SelectServicePage() :
            base()
        {
            InitializeComponent();

            var viewModel = DataContext as SelectServicePageViewModel;
            viewModel.OnRenderServices += RenderServices;

            pager = new ServicesPager(this);

            prevButton.DataContext = pager;
            nextButton.DataContext = pager;
        }

        private void RenderServices(object sender, RenderServicesEventArgs args)
        {
            pager.UpdateServices(args.Services, args.Cols, args.Rows);
        }
    }
}