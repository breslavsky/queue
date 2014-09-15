using Junte.UI.WPF;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.UI.WPF;
using Queue.UI.WPF.Controls.Sheduler;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using DTO = Queue.Services.DTO;

namespace Queue.Terminal.Models.Pages
{
    public class SelectDateTimeShedulerPageVM : PageVM
    {
        private WeekScheduler sheduler;
        private readonly DayOfWeek firstDayOfWeek;
        private Event selected;
        private bool hasNextWeek;
        private bool hasPrevWeek;
        private int currentWeek;

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public bool HasNextWeek
        {
            get { return hasNextWeek; }
            set { SetProperty(ref hasNextWeek, value); }
        }

        public bool HasPrevWeek
        {
            get { return hasPrevWeek; }
            set { SetProperty(ref hasPrevWeek, value); }
        }

        public ICommand NextWeekCommand { get; set; }

        public ICommand PrevWeekCommand { get; set; }

        public SelectDateTimeShedulerPageVM()
        {
            firstDayOfWeek = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
            PrevWeekCommand = new RelayCommand(PrevWeek);
            NextWeekCommand = new RelayCommand(NextWeek);

            HasNextWeek = true;
        }

        public void Initialize(WeekScheduler sheduler)
        {
            this.sheduler = sheduler;
            sheduler.Initialize();
            sheduler.OnEventSelected += sheduler_OnEventSelected;

            currentWeek = 0;
            UpdateFreeTimesForWeek(currentWeek);
        }

        private void sheduler_OnEventSelected(object sender, Event e)
        {
            SetSelected(e);
        }

        private void SetSelected(Event e)
        {
            if (e.AllDay)
            {
                return;
            }

            if ((selected != null) && (selected.UIElement != null))
            {
                selected.UIElement.SetBackgroundColor(selected.Color);
            }

            selected = e;
            if (e.UIElement != null)
            {
                e.UIElement.SetBackgroundColor(Brushes.Blue);
            }
        }

        private void Next()
        {
            DateTime? date = GetSelectedTime();
            if (!date.HasValue)
            {
                screen.ShowWarning("Время предварительной записи не выбрано");
                return;
            }
            model.SelectedDate = date.Value.Date;
            model.SelectedTime = date.Value.TimeOfDay;
            navigator.NextPage();
        }

        private DateTime? GetSelectedTime()
        {
            if (selected == null)
            {
                return null;
            }

            return selected.Start;
        }

        private void Prev()
        {
            navigator.PrevPage();
        }

        private void NextWeek()
        {
            UpdateFreeTimesForWeek(++currentWeek);
        }

        private void PrevWeek()
        {
            UpdateFreeTimesForWeek(--currentWeek);
        }

        private async void UpdateFreeTimesForWeek(int week)
        {
            if (model.SelectedService == null)
            {
                return;
            }

            HasPrevWeek = week > 0;

            DateTime date = GetDayForWeek(week);

            sheduler.PreventUIUpdate = true;

            sheduler.ClearEvents();
            sheduler.SelectedDate = date;

            using (Channel<IServerService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    int currentDay = (int)date.DayOfWeek - (int)firstDayOfWeek;

                    List<Task> tasks = new List<Task>();
                    for (int i = 0; i <= 6 - currentDay; i++)
                    {
                        tasks.Add(LoadTimeIntervalForDay(channel, date.AddDays(i)));
                    }

                    Task[] ar = tasks.ToArray();

                    await Task.WhenAll(ar);
                }
                catch (FaultException exception)
                {
                    screen.ShowWarning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    sheduler.PreventUIUpdate = false;
                    sheduler.UpdateUI();
                    loading.Hide();
                }
            }
        }

        private DateTime GetDayForWeek(int week)
        {
            if (week == 0)
            {
                return terminalConfig.CurrentDayRecording ? DateTime.Now.Date : DateTime.Now.Date.AddDays(1);
            }

            DateTime now = DateTime.Now.Date;
            return now.AddDays(7 * week - (int)now.DayOfWeek + 1);
        }

        private async Task LoadTimeIntervalForDay(Channel<IServerService> channel, DateTime date)
        {
            try
            {
                DTO.ServiceFreeTime freeTime = await taskPool.AddTask(channel.Service.GetFreeTime(model.SelectedService.Id, date, ClientRequestType.Early));

                SetStartJourney(freeTime.Schedule.EarlyStartTime);
                SetEndJourney(freeTime.Schedule.EarlyFinishTime);

                foreach (TimeSpan interval in freeTime.TimeIntervals.Distinct())
                {
                    DateTime start = date.Add(interval);
                    sheduler.AddEvent(new Event()
                           {
                               Color = Brushes.Green,
                               Start = start,
                               End = start.Add(freeTime.Schedule.ClientInterval)
                           });
                }
            }
            catch (FaultException exception)
            {
                sheduler.AddEvent(new Event()
                          {
                              AllDay = true,
                              Subject = exception.Message,
                              Color = Brushes.Red,
                              Start = date,
                              End = date
                          });
            }
        }

        private void SetStartJourney(TimeSpan time)
        {
            if ((sheduler.StartJourney == TimeSpan.Zero) || (sheduler.StartJourney > time))
            {
                sheduler.StartJourney = time;
            }
        }

        private void SetEndJourney(TimeSpan time)
        {
            if ((sheduler.EndJourney == TimeSpan.Zero) || (sheduler.EndJourney < time))
            {
                sheduler.EndJourney = time;
            }
        }
    }
}