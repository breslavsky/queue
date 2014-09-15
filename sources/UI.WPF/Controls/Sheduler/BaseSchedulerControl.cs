using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Queue.UI.WPF.Controls.Sheduler
{
    public abstract class BaseSchedulerControl : UserControl
    {
        protected DateTime selectedDate;
        protected TimeSpan startJourney;
        protected TimeSpan endJourney;

        public event EventHandler<Event> OnEventSelected;

        public bool PreventUIUpdate { get; set; }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                SetSelectedDate(value);
            }
        }

        public TimeSpan StartJourney
        {
            get { return startJourney; }
            set
            {
                startJourney = value;
                SetStartJourney(value);
            }
        }

        public TimeSpan EndJourney
        {
            get { return endJourney; }
            set
            {
                endJourney = value;
                SetEndJourney(value);
            }
        }

        public ObservableCollection<Event> Events;

        public virtual void Initialize()
        {
            Events = new ObservableCollection<Event>();
            SelectedDate = DateTime.Now;
        }

        public virtual void ClearEvents()
        {
            Events.Clear();
        }

        public void AddEvent(Event e)
        {
            if (e.Start > e.End)
            {
                throw new ArgumentException("End date is before Start date");
            }

            Events.Add(e);
            InternalAddEvent(e);
        }

        public void DeleteEvent(Guid id)
        {
            Event e = Events.SingleOrDefault(ev => ev.Id.Equals(id));
            if (e != null)
            {
                Events.Remove(e);
                InternalDeleteEvent(e);
            }
        }

        public virtual void UpdateUI()
        {
        }

        protected virtual void InternalAddEvent(Event e)
        {
        }

        protected virtual void InternalDeleteEvent(Event e)
        {
        }

        protected virtual void SetStartJourney(TimeSpan val)
        {
        }

        protected virtual void SetEndJourney(TimeSpan val)
        {
        }

        protected virtual void SetSelectedDate(DateTime val)
        {
        }

        protected void SelectEvent(Event e)
        {
            if (OnEventSelected != null)
            {
                OnEventSelected(this, e);
            }
        }
    }
}