using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Notification.ViewModels;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace Queue.Notification.Views
{
    public partial class MainPage : RichPage
    {
        private MainPageViewModel model;

        public MainPage()
            : base()
        {
            InitializeComponent();

            model = ServiceLocator.Current.GetInstance<UnityContainer>().Resolve<MainPageViewModel>();
            model.RequestUpdated += model_CurrentClientRequestPlanUpdated;
            model.RequestsLengthChanged += model_ClientRequestsLengthChanged;

            DataContext = model;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Mouse.OverrideCursor = Cursors.None;
        }

        private void model_ClientRequestsLengthChanged(object sender, int length)
        {
            CallingClientRequestsControl.Model.ClientRequestsLength = length;
        }

        private void model_CurrentClientRequestPlanUpdated(object sender, ClientRequest e)
        {
            CallingClientRequestsControl.Model.AddToClientRequests(e);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            model.Dispose();
        }
    }
}