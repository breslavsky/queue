using Microsoft.Practices.ServiceLocation;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace Queue.Notification.Views
{
    public partial class MainPage : RichPage
    {
        public MainPage()
            : base()
        {
            InitializeComponent();

            Content = ServiceLocator.Current.GetInstance<ITemplateManager>().GetTemplate("main-page.xaml");
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Mouse.OverrideCursor = Cursors.None;
        }
    }
}