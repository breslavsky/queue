using Queue.Common;
using Queue.Terminal.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Queue.Terminal.Views
{
    public partial class SelectRequestDatePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectRequestDatePageViewModel); } }

        public SelectRequestDatePage()
            : base()
        {
            InitializeComponent();
        }

        private void TerminalPage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void TerminalPage_Loaded(object sender, RoutedEventArgs e)
        {
            earlyRequestDateCalendar.BlackoutDates.Clear();
            DateTime end = ServerDateTime.Today;
            if (terminalConfig.CurrentDayRecording)
            {
                end = end.AddDays(-1);
            }
            earlyRequestDateCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, end));

            (DataContext as SelectRequestDatePageViewModel).Initialize();
        }

        private void TerminalPage_Unloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as SelectRequestDatePageViewModel).Unloaded();
        }
    }
}