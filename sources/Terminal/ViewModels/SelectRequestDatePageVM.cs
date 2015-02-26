using Junte.UI.WPF;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectRequestDatePageVM : PageVM
    {
        private ObservableCollection<EarlyRequestHour> availableHours = new ObservableCollection<EarlyRequestHour>();
        private EarlyRequestHour selectedHour;
        private int? selectedMinute;

        public SelectRequestDatePageVM()
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
            SelectedDatesChanged();
        }

        private void Next()
        {
            if (!model.SelectedDate.HasValue)
            {
                screen.ShowWarning("Дата предварительной записи не выбрана");
                return;
            }

            model.SelectedTime = GetSelectedTime();
            if (model.SelectedTime == TimeSpan.Zero)
            {
                screen.ShowWarning("Время предварительной записи не выбрано");
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
                SelectedDatesChanged();
            }
        }

        private void SelectedDatesChanged()
        {
            ReloadFreeTime();
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
                var loading = screen.ShowLoading();

                try
                {
                    ServiceFreeTime freeTime = await taskPool.AddTask(channel.Service.GetServiceFreeTime(model.SelectedService.Id, model.SelectedDate.Value, ClientRequestType.Early));
                    TimeSpan[] timeIntervals = freeTime.TimeIntervals;

                    if (timeIntervals.Length == 0)
                    {
                        return;
                    }

                    foreach (var timeInterval in timeIntervals)
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
    }
}