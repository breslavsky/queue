using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class SelectServicePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectServicePageViewModel); } }

        public SelectServicePage() :
            base()
        {
            InitializeComponent();

            var viewModel = DataContext as SelectServicePageViewModel;
            ctrlSelectService.ServiceSelected += (semder, service) => viewModel.SetSelectedService(service);
            ctrlSelectLifeSituation.ServiceSelected += (semder, service) => viewModel.SetSelectedService(service);
        }
    }
}