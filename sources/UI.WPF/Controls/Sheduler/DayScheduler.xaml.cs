using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Queue.UI.WPF.Controls.Sheduler
{
    //TODO: перед использование отрефакторить
    public partial class DayScheduler : BaseSchedulerControl
    {
        private DateTime _currentDay;

        internal DateTime CurrentDay
        {
            get { return _currentDay; }
            set
            {
                _currentDay = value;
                AdjustCurrentDay(value);
            }
        }

        private void AdjustCurrentDay(DateTime currentDay)
        {
            dayLabel.Content = currentDay.ToString("dddd dd/MM");

            PaintAllEvents();
            PaintAllDayEvents();
        }

        public DayScheduler()
        {
            InitializeComponent();

            column.Background = Brushes.Transparent;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs ea)
        {
            (sender as DayScheduler).SizeChanged += DayScheduler_SizeChanged;

            ResizeGrids(new Size(this.ActualWidth, this.ActualHeight));
            PaintAllEvents();
            PaintAllDayEvents();

            UpdateStartJourney();
            UpdateEndJourney();
        }

        protected override void SetStartJourney(TimeSpan val)
        {
            base.SetStartJourney(val);
            UpdateStartJourney();
        }

        private void UpdateStartJourney()
        {
            if (StartJourney.Hours != 0)
            {
                double hourHeight = EventsGrid.ActualHeight / 22;
                ScrollEventsViewer.ScrollToVerticalOffset(hourHeight * (StartJourney.Hours - 1));
            }

            if (StartJourney.Hours == 0)
            {
                startJourneyPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                Grid.SetRowSpan(startJourneyPanel, StartJourney.Hours * 2);
            }
        }

        protected override void SetEndJourney(TimeSpan val)
        {
            base.SetEndJourney(val);
            UpdateEndJourney();
        }

        private void UpdateEndJourney()
        {
            if (EndJourney.Hours == 0)
            {
                endJourneyPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                Grid.SetRow(endJourneyPanel, EndJourney.Hours * 2);
                Grid.SetRowSpan(endJourneyPanel, 48 - EndJourney.Hours * 2);
            }
        }

        protected override void InternalAddEvent(Event e)
        {
            if (!PreventUIUpdate)
            {
                EventsChanged(e);
            }
        }

        protected override void InternalDeleteEvent(Event e)
        {
            if (!PreventUIUpdate)
            {
                EventsChanged(e);
            }
        }

        public void EventsChanged(Event e)
        {
            if (e.AllDay)
            {
                PaintAllDayEvents();
            }
            else if (e.Start.Date == e.End.Date)
            {
                PaintAllEvents();
            }
        }

        private void DayScheduler_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeGrids(e.NewSize);
            PaintAllEvents();
            PaintAllDayEvents();
        }

        private IEnumerable<Event> TodayEvents
        {
            get
            {
                return Events.Where(ev => ev.Start.Date <= CurrentDay.Date && ev.End.Date >= CurrentDay.Date);
            }
        }

        private void PaintAllEvents()
        {
            IEnumerable<Event> eventList = TodayEvents.Where(ev => ev.Start.Date == ev.End.Date && !ev.AllDay).OrderBy(ev => ev.Start);

            column.Children.Clear();

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in eventList)
            {
                //column.Width = columnWidth;

                double oneHourHeight = 50;// column.ActualHeight / 46;

                var concurrentEvents = TodayEvents.Where(e1 => ((e1.Start <= e.Start && e1.End > e.Start) ||
                                                                (e1.Start > e.Start && e1.Start < e.End)) &&
                                                                e1.End.Date == e1.Start.Date).OrderBy(ev => ev.Start);

                double marginTop = oneHourHeight * (e.Start.Hour + (e.Start.Minute / 60.0));
                double width = columnWidth / (concurrentEvents.Count());
                double marginLeft = width * getIndex(e, concurrentEvents.ToList());

                EventUserControl wEvent = new EventUserControl(e, true);
                wEvent.Width = width;
                wEvent.Height = e.End.Subtract(e.Start).TotalHours * oneHourHeight;
                wEvent.Margin = new Thickness(marginLeft, marginTop, 0, 0);

                column.Children.Add(wEvent);
            }
        }

        private int getIndex(Event e, List<Event> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (e.Id == list[i].Id) return i;
            }
            return -1;
        }

        private void PaintAllDayEvents()
        {
            allDayEvents.Children.Clear();

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in TodayEvents.Where(ev => ev.End.Date > ev.Start.Date || ev.AllDay))
            {
                EventUserControl wEvent = new EventUserControl(e, false);
                wEvent.Width = columnWidth;
                wEvent.Margin = new Thickness(0, 0, 0, 0);

                allDayEvents.Children.Add(wEvent);
            }
        }

        private void ResizeGrids(Size newSize)
        {
            EventsGrid.Width = newSize.Width;
            EventsHeaderGrid.Width = newSize.Width;

            double columnWidth = (this.ActualWidth - EventsGrid.ColumnDefinitions[0].ActualWidth);
            for (int i = 1; i < EventsGrid.ColumnDefinitions.Count; i++)
            {
                EventsGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
                EventsHeaderGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
            }
        }
    }
}