using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Enums;
using System;
using System.Threading.Tasks;

namespace Queue.Terminal.Core
{
    public class ClientRequestModel : ObservableObject
    {
        private Service selectedService;
        private DateTime? selectedDate;
        private TimeSpan? selectedTime;
        private Client currentClient;
        private int? maxSubjects;
        private int? subjects;

        public ClientRequestModel()
        {
            Reset();
        }

        public ClientRequestType? RequestType { get; set; }

        public Service SelectedService
        {
            get { return selectedService; }
            set { SetProperty(ref selectedService, value); }
        }

        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref selectedDate, value); }
        }

        public TimeSpan? SelectedTime
        {
            get { return selectedTime; }
            set { SetProperty(ref selectedTime, value); }
        }

        public Client CurrentClient
        {
            get { return currentClient; }
            set { SetProperty(ref currentClient, value); }
        }

        public int? MaxSubjects
        {
            get { return maxSubjects; }
            set { SetProperty(ref maxSubjects, value); }
        }

        public int? Subjects
        {
            get { return subjects; }
            set { SetProperty(ref subjects, value); }
        }

        public Administrator CurrentAdministrator { get; set; }

        public void Reset()
        {
            SelectedService = null;
            RequestType = null;
            SelectedDate = null;
            SelectedTime = null;
            CurrentClient = null;
            MaxSubjects = null;
            Subjects = null;
        }

        public ClientRequestModelState GetCurrentState()
        {
            if (SelectedService == null)
            {
                return ClientRequestModelState.SetService;
            }

            if (RequestType == null)
            {
                return ClientRequestModelState.SetRequestType;
            }

            if ((SelectedDate == null) &&
                (SelectedTime == null) &&
                (RequestType == ClientRequestType.Early))
            {
                return ClientRequestModelState.SetRequestDate;
            }

            if ((CurrentClient == null) &&
                (SelectedService.ClientRequire))
            {
                return ClientRequestModelState.SetClient;
            }

            if ((MaxSubjects > 1) && (Subjects == null))
            {
                return ClientRequestModelState.SetSubjects;
            }

            return ClientRequestModelState.Completed;
        }

        public async Task AdjustMaxSubjects()
        {
            MaxSubjects = null;

            if (RequestType == ClientRequestType.Live)
            {
                ChannelManager<IServerTcpService> channelManager = ServiceLocator.Current.GetInstance<ChannelManager<IServerTcpService>>();
                TaskPool taskPool = ServiceLocator.Current.GetInstance<TaskPool>();
                using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
                {
                    TimeSpan[] timeIntervals = (await taskPool.AddTask(channel.Service.GetServiceFreeTime(SelectedService.Id, ServerDateTime.Today, ClientRequestType.Live))).TimeIntervals;
                    if (timeIntervals.Length > 0)
                    {
                        MaxSubjects = Math.Min(SelectedService.MaxSubjects, timeIntervals.Length);
                    }
                }
            }
            else
            {
                MaxSubjects = SelectedService.MaxSubjects;
            }

            if (MaxSubjects == null)
            {
                throw new ApplicationException("Нет свободных объектов для записи");
            }
        }
    }
}