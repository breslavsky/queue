using Junte.UI.WPF;
using Queue.Model.Common;
using Queue.Services.DTO;
using System;

namespace Queue.Terminal.Core
{
    public class ClientRequestModel : ObservableObject
    {
        private Service selectedService;
        private DateTime? selectedDate;
        private TimeSpan? selectedTime;
        private Client currentClient;
        private double? maxSubjects;
        private double? subjectsCount;

        public ClientRequestModel()
        {
            Reset();
        }

        public ClientRequestType? QueueType { get; set; }

        public Service SelectedService
        {
            get { return selectedService; }
            set
            {
                SetProperty(ref selectedService, value);
                if ((value != null) && (value.EarlyRegistrator == ClientRequestRegistrator.None))
                {
                    QueueType = ClientRequestType.Live;
                }
                else
                {
                    QueueType = null;
                }
            }
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

        public double? MaxSubjects
        {
            get { return maxSubjects; }
            set { SetProperty(ref maxSubjects, value); }
        }

        public double? SubjectsCount
        {
            get { return subjectsCount; }
            set { SetProperty(ref subjectsCount, value); }
        }

        public Administrator CurrentAdministrator { get; set; }

        public void Reset()
        {
            SelectedService = null;
            QueueType = null;
            SelectedDate = null;
            SelectedTime = null;
            CurrentClient = null;
            MaxSubjects = null;
            SubjectsCount = null;
        }

        public ClientRequestModelState GetState()
        {
            if (SelectedService == null)
            {
                return ClientRequestModelState.SetService;
            }

            if (QueueType == null)
            {
                return ClientRequestModelState.SetRequestType;
            }

            if ((SelectedDate == null) &&
                (SelectedTime == null) &&
                (QueueType == ClientRequestType.Early))
            {
                return ClientRequestModelState.SetRequestDate;
            }

            if ((CurrentClient == null) &&
                (SelectedService.ClientRequire))
            {
                return ClientRequestModelState.SetClient;
            }

            if ((MaxSubjects > 1) && (SubjectsCount == null))
            {
                return ClientRequestModelState.SetSubjectsCount;
            }

            return ClientRequestModelState.Completed;
        }
    }
}