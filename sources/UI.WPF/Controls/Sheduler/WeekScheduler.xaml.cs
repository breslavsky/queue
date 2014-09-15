using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.UI.WPF.Controls.Sheduler
{
    public partial class WeekScheduler : BaseSchedulerControl
    {
        private DateTime firstDay;

        public DateTime FirstDay
        {
            get { return firstDay; }
            set
            {
                while (value.DayOfWeek != DayOfWeek.Monday)
                {
                    value = value.AddDays(-1);
                }

                firstDay = value;
                AdjustFirstDay(value);
            }
        }

        public WeekScheduler()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs ea)
        {
            SizeChanged += WeekScheduler_SizeChanged;
            UpdateUIState();
        }

        public override void ClearEvents()
        {
            base.ClearEvents();

            if (!PreventUIUpdate)
            {
                allDayEvents.Children.Clear();
                ClearColumns();
            }
        }

        public override void UpdateUI()
        {
            UpdateUIState();
        }

        private void UpdateUIState()
        {
            FirstDay = SelectedDate;

            ResizeGrids(new Size(this.ActualWidth, this.ActualHeight));

            UpdateStartJourney();
            UpdateEndJourney();

            PaintAllEvents(null);
            PaintAllDayEvents();
        }

        protected override void SetSelectedDate(DateTime date)
        {
            base.SetSelectedDate(date);

            if (!PreventUIUpdate)
            {
                FirstDay = SelectedDate;
            }
        }

        private void AdjustFirstDay(DateTime firstDay)
        {
            SetDayLabelContent(dayLabel1, firstDay);
            SetDayLabelContent(dayLabel2, firstDay.AddDays(1));
            SetDayLabelContent(dayLabel3, firstDay.AddDays(2));
            SetDayLabelContent(dayLabel4, firstDay.AddDays(3));
            SetDayLabelContent(dayLabel5, firstDay.AddDays(4));
            SetDayLabelContent(dayLabel6, firstDay.AddDays(5));
            SetDayLabelContent(dayLabel7, firstDay.AddDays(6));

            PaintAllEvents(null);
            PaintAllDayEvents();
        }

        private void SetDayLabelContent(Label label, DateTime date)
        {
            label.Content = date.ToString("dddd") + Environment.NewLine + date.ToString("dd.MM.yyyy");
        }

        private int GetActiveRowCount()
        {
            int result = 0;

            foreach (RowDefinition row in EventsGrid.RowDefinitions)
            {
                if (row.Height.Value > 0)
                {
                    result++;
                }
            }

            return result;
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

        protected override void SetStartJourney(TimeSpan val)
        {
            base.SetStartJourney(val);
            UpdateStartJourney();
        }

        protected override void SetEndJourney(TimeSpan val)
        {
            base.SetEndJourney(val);
            UpdateEndJourney();
        }

        private void UpdateStartJourney()
        {
            if (StartJourney.Hours == 0)
            {
                startJourneyPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                for (int i = 0; i < StartJourney.Hours; i++)
                {
                    EventsGrid.RowDefinitions[i].Height = new GridLength(0);
                }

                Grid.SetRowSpan(startJourneyPanel, StartJourney.Hours);
            }

            EventsGrid.UpdateLayout();
        }

        private void UpdateEndJourney()
        {
            if (EndJourney.Hours == 0)
            {
                endJourneyPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                for (int i = EndJourney.Hours; i < 24; i++)
                {
                    EventsGrid.RowDefinitions[i].Height = new GridLength(0);
                }

                Grid.SetRow(endJourneyPanel, EndJourney.Hours);
                Grid.SetRowSpan(endJourneyPanel, 24 - EndJourney.Hours);
            }

            EventsGrid.UpdateLayout();
        }

        public void EventsChanged(Event e)
        {
            if (e.AllDay)
            {
                PaintAllDayEvents();
            }
            else if (e.Start.Date == e.End.Date)
            {
                PaintAllEvents(e.Start);
            }
        }

        private void WeekScheduler_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeGrids(e.NewSize);
            PaintAllEvents(null);
            PaintAllDayEvents();
        }

        private void PaintAllEvents(DateTime? date)
        {
            IEnumerable<Event> eventList = Events.Where(ev => ev.Start.Date == ev.End.Date && !ev.AllDay).OrderBy(ev => ev.Start);

            if (date == null)
            {
                ClearColumns();
            }
            else
            {
                int numColumn = (int)date.Value.Date.Subtract(FirstDay.Date).TotalDays + 1;
                ((Canvas)this.FindName("column" + numColumn)).Children.Clear();

                eventList = eventList.Where(ev => ev.Start.Date == date.Value.Date).OrderBy(ev => ev.Start);
            }

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in eventList)
            {
                int numColumn = (int)e.Start.Date.Subtract(FirstDay.Date).TotalDays + 1;
                if (numColumn >= 0 && numColumn < 7)
                {
                    Canvas sp = (Canvas)this.FindName("column" + numColumn);
                    sp.Width = columnWidth;

                    double oneHourHeight = sp.ActualHeight / GetActiveRowCount();

                    var concurrentEvents = Events.Where(e1 => ((e1.Start <= e.Start && e1.End > e.Start) ||
                                                                    (e1.Start > e.Start && e1.Start < e.End)) &&
                                                                   e1.End.Date == e1.Start.Date).OrderBy(ev => ev.Start);

                    double marginTop = oneHourHeight * ((e.Start.Hour - StartJourney.Hours) + (e.Start.Minute / 60.0));
                    double width = columnWidth / (concurrentEvents.Count());
                    double marginLeft = width * GetIndex(e, concurrentEvents.ToList());

                    sp.Children.Add(CreateEventUserControl(e,
                        width,
                        new Thickness(marginLeft, marginTop, 0, 0),
                        e.End.Subtract(e.Start).TotalHours * oneHourHeight,
                        true));
                }
            }
        }

        private void ClearColumns()
        {
            column1.Children.Clear();
            column2.Children.Clear();
            column3.Children.Clear();
            column4.Children.Clear();
            column5.Children.Clear();
            column6.Children.Clear();
            column7.Children.Clear();
        }

        private int GetIndex(Event e, List<Event> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (e.Id == list[i].Id)
                {
                    return i;
                }
            }
            return -1;
        }

        private void PaintAllDayEvents()
        {
            allDayEvents.Children.Clear();

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in Events.Where(ev => ev.End.Date > ev.Start.Date || ev.AllDay))
            {
                int numColumn = (int)e.Start.Date.Subtract(FirstDay.Date).TotalDays;
                int numEndColumn = (int)e.End.Date.Subtract(FirstDay.Date).TotalDays + 1;

                if (numColumn >= 7 || numEndColumn <= 0)
                {
                    continue;
                }

                if (numColumn < 0)
                {
                    numColumn = 0;
                }
                if (numEndColumn > 7)
                {
                    numEndColumn = 7;
                }

                if ((numColumn >= 0 && numColumn < 7) || (numEndColumn >= 0 && numEndColumn < 7))
                {
                    double marginLeft = (numColumn) * columnWidth;
                    allDayEvents.Children.Add(CreateEventUserControl(e,
                        columnWidth * (numEndColumn - numColumn),
                        new Thickness(marginLeft, 0, 0, 0)));
                }
            }

            double h = 0;
            foreach (UIElement el in allDayEvents.Children)
            {
                FrameworkElement fEl = el as FrameworkElement;
                if (fEl != null)
                {
                    fEl.UpdateLayout();
                    if (h <= fEl.ActualHeight)
                    {
                        h = fEl.ActualHeight;
                    }
                }
            }

            allDayEvents.Height = h;
        }

        private EventUserControl CreateEventUserControl(Event e, double width, Thickness margin, double? height = null, bool showTime = false)
        {
            EventUserControl result = new EventUserControl(e, showTime);
            result.Width = width;
            result.Margin = margin;

            if (height.HasValue)
            {
                result.Height = height.Value;
            }

            result.PreviewMouseUp += ((object sender, MouseButtonEventArgs ea) =>
            {
                ea.Handled = true;
                SelectEvent(result.Event);
            });

            e.UIElement = result;

            return result;
        }

        private void ResizeGrids(Size newSize)
        {
            EventsGrid.Width = newSize.Width;
            EventsHeaderGrid.Width = newSize.Width;

            double columnWidth = (ScrollEventsViewer.ViewportWidth - EventsGrid.ColumnDefinitions[0].ActualWidth) / 7;
            for (int i = 1; i < EventsGrid.ColumnDefinitions.Count; i++)
            {
                EventsGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
                EventsHeaderGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
            }
        }
    }
}