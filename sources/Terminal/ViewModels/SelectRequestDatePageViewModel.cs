using Junte.UI.WPF;
using Junte.WCF;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectRequestDatePageViewModel : PageViewModel
    {
        private ObservableCollection<EarlyRequestHour> availableHours = new ObservableCollection<EarlyRequestHour>();
        private EarlyRequestHour selectedHour;
        private int? selectedMinute;

        public SelectRequestDatePageViewModel()
        {
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);

            model.PropertyChanged += model_PropertyChanged;
        }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ObservableCollection<EarlyRequestHour> AvailableHours
        {
            get { return availableHours; }
            set { availableHours = value; }
        }

        public EarlyRequestHour SelectedHour
        {
            get { return selectedHour; }
            set
            {
                SetProperty(ref selectedHour, value);
                SelectedMinute = null;
            }
        }

        public int? SelectedMinute
        {
            get { return selectedMinute; }
            set { SetProperty(ref selectedMinute, value); }
        }

        public void Initialize()
        {
            if (model.SelectedDate == null)
            {
                DateTime date = DateTime.Now.Date;
                if (!terminalConfig.CurrentDayRecording)
                {
                    date = date.AddDays(1);
                }

                model.SelectedDate = date;
            }

            ReloadFreeTime();
        }

        private void Next()
        {
            if (!model.SelectedDate.HasValue)
            {
                screen.ShowWarning(Translater.Message("EarlyDateNotSelected"));
                return;
            }

            model.SelectedTime = GetSelectedTime();
            if (model.SelectedTime == TimeSpan.Zero)
            {
                screen.ShowWarning(Translater.Message("EarlyTimeNotSelected"));
                return;
            }

            navigator.NextPage();
        }

        private TimeSpan GetSelectedTime()
        {
            TimeSpan requestTime = TimeSpan.Zero;

            if (SelectedHour != null)
            {
                requestTime = requestTime.Add(TimeSpan.FromHours(SelectedHour.Hour));
            }

            if (SelectedMinute.HasValue)
            {
                requestTime = requestTime.Add(TimeSpan.FromMinutes(SelectedMinute.Value));
            }

            return requestTime;
        }

        private void Prev()
        {
            Model.SelectedDate = null;
            Model.SelectedTime = null;
            navigator.PrevPage();
        }

        private void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                ReloadFreeTime();
            }
        }

        private async void ReloadFreeTime()
        {
            AvailableHours.Clear();
            SelectedHour = null;
            SelectedMinute = null;

            if (model.SelectedService == null || !model.SelectedDate.HasValue)
            {
                return;
            }

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    ServiceFreeTime freeTime = await taskPool.AddTask(channel.Service.GetServiceFreeTime(model.SelectedService.Id, model.SelectedDate.Value, ClientRequestType.Early));
                    TimeSpan[] timeIntervals = freeTime.TimeIntervals;

                    if (timeIntervals.Length == 0)
                    {
                        return;
                    }

                    foreach (TimeSpan timeInterval in timeIntervals)
                    {
                        EarlyRequestHour hour = AvailableHours.SingleOrDefault(h => h.Hour == timeInterval.Hours);
                        if (hour == null)
                        {
                            hour = new EarlyRequestHour()
                            {
                                Hour = timeInterval.Hours
                            };
                            AvailableHours.Add(hour);
                        }

                        if (!hour.Minutes.Exists(m => m == timeInterval.Minutes))
                        {
                            hour.Minutes.Add(timeInterval.Minutes);
                        }
                    }

                    SelectedHour = AvailableHours.Count > 0 ? AvailableHours[0] : null;
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
                    loading.Hide();
                }
            }
        }

        internal void Unloaded()
        {
            model.PropertyChanged -= model_PropertyChanged;
        }

        public class EarlyRequestHour
        {
            private List<int> minutes = new List<int>();

            public int Hour { get; set; }

            public List<int> Minutes
            {
                get { return minutes; }
                set { minutes = value; }
            }
        }
    }
}