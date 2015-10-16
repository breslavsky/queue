using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.DTO;
using Queue.Terminal.Core;
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
        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public ClientRequestModel Model { get; set; }

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
            var end = ServerDateTime.Today;
            if (TerminalConfig.CurrentDayRecording)
            {
                end = end.AddDays(-1);
            }

            earlyRequestDateCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, end));

            if (Model.SelectedService != null)
            {
                var range = new CalendarDateRange(ServerDateTime.Today.AddDays(Model.SelectedService.MaxEarlyDays + 1), DateTime.MaxValue);
                earlyRequestDateCalendar.BlackoutDates.Add(range);
            }
        }
    }
}