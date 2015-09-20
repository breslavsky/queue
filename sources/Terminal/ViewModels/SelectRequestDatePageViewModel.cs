using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
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

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

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

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        public SelectRequestDatePageViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
        }

        private void Loaded()
        {
            Model.PropertyChanged += model_PropertyChanged;

            if (Model.SelectedDate == null)
            {
                var date = DateTime.Now.Date;
                if (!TerminalConfig.CurrentDayRecording)
                {
                    date = date.AddDays(1);
                }

                Model.SelectedDate = date;
            }

            ReloadFreeTime();
        }

        private void Next()
        {
            if (!Model.SelectedDate.HasValue)
            {
                Window.Warning(Translater.Message("EarlyDateNotSelected"));
                return;
            }

            Model.SelectedTime = GetSelectedTime();
            if (Model.SelectedTime == TimeSpan.Zero)
            {
                Window.Warning(Translater.Message("EarlyTimeNotSelected"));
                return;
            }

            Navigator.NextPage();
        }

        private TimeSpan GetSelectedTime()
        {
            var requestTime = TimeSpan.Zero;

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
            Navigator.PrevPage();
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

            if (Model.SelectedService == null || !Model.SelectedDate.HasValue)
            {
                return;
            }

            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    var freeTime = await TaskPool.AddTask(channel.Service.GetServiceFreeTime(Model.SelectedService.Id, Model.SelectedDate.Value, ClientRequestType.Early));
                    var timeIntervals = freeTime.TimeIntervals;

                    if (timeIntervals.Length == 0)
                    {
                        return;
                    }

                    foreach (var timeInterval in timeIntervals)
                    {
                        var hour = AvailableHours.SingleOrDefault(h => h.Hour == timeInterval.Hours);
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
                    Window.Warning(exception.Reason.ToString());
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
            Model.PropertyChanged -= model_PropertyChanged;
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