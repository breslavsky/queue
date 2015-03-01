using Queue.Terminal.ViewModels;
using System;
using System.Windows;

namespace Queue.Terminal.Views
{
    public partial class PrintCouponPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(PrintCouponPageViewModel); } }

        public PrintCouponPage()
            : base()
        {
            InitializeComponent();
        }

        private void TerminalPage_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as PrintCouponPageViewModel).Initialize();
        }

        private void TerminalPage_Unloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as PrintCouponPageViewModel).Dispose();
        }
    }
}