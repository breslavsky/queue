﻿using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace Queue.Services.Server
{
    public sealed class OperatorPlan
    {
        public ClientRequestPlan currentClientRequestPlan;

        private readonly List<TimeInterval> clientRequestIntervals = new List<TimeInterval>();

        public OperatorPlan(Operator queueOperator, DateTime planDate)
        {
            Operator = queueOperator;
            PlanDate = planDate;

            Metrics = new OperatorPlanMetrics(queueOperator);

            ClientRequestPlans = new ObservableCollection<ClientRequestPlan>();
            ClientRequestPlans.CollectionChanged += ClientRequestPlans_CollectionChanged;
        }

        public ObservableCollection<ClientRequestPlan> ClientRequestPlans { get; private set; }

        public ClientRequestPlan CurrentClientRequestPlan
        {
            get { return currentClientRequestPlan; }
            private set
            {
                currentClientRequestPlan = value;
                if (currentClientRequestPlan == null)
                {
                    //TODO: why?
                    Metrics.Reset();
                }
            }
        }

        public OperatorPlanMetrics Metrics { get; private set; }
        public Operator Operator { get; private set; }
        public DateTime PlanDate { get; set; }
        public TimeSpan PlanTime { get; set; }
        public OperatorInterruption[] Interruptions { get; set; }

        private TimeInterval[] GetIgnoredIntervals(ClientRequestType clientRequestType)
        {
            var serviceRenderingMode = clientRequestType == ClientRequestType.Early
                ? ServiceRenderingMode.EarlyRequests
                : ServiceRenderingMode.LiveRequests;

            var formatInfo = DateTimeFormatInfo.CurrentInfo;
            var calendar = formatInfo.Calendar;
            int week = calendar.GetWeekOfYear(PlanDate, CalendarWeekRule.FirstFourDayWeek, formatInfo.FirstDayOfWeek);

            return Interruptions.Where(i => i.ServiceRenderingMode == ServiceRenderingMode.AllRequests
                        || i.ServiceRenderingMode == serviceRenderingMode)
                .Select(i => new TimeInterval(i.StartTime, i.FinishTime))
                .ToArray();
        }

        public void AddClientRequest(ClientRequest clientRequest, Schedule schedule)
        {
            if (!clientRequest.IsClosed)
            {
                TimeSpan startTime, finishTime;

                var clientInterval = clientRequest.Type == ClientRequestType.Live
                    ? schedule.LiveClientInterval : schedule.EarlyClientInterval;
                clientInterval = TimeSpan.FromTicks(clientInterval.Ticks * clientRequest.Subjects);

                switch (clientRequest.State)
                {
                    case ClientRequestState.Waiting:
                    case ClientRequestState.Redirected:
                    case ClientRequestState.Postponed:
                        startTime = clientRequest.RequestTime;
                        if (startTime < schedule.StartTime)
                        {
                            startTime = schedule.StartTime;
                        }
                        if (startTime < PlanTime)
                        {
                            startTime = PlanTime;
                        }
                        startTime = GetNearTimeInterval(startTime, schedule, clientRequest.Type, clientInterval);
                        finishTime = startTime.Add(clientInterval);
                        break;

                    case ClientRequestState.Calling:
                        startTime = clientRequest.CallingStartTime;
                        finishTime = PlanTime.Add(clientInterval);
                        break;

                    case ClientRequestState.Rendering:
                        startTime = clientRequest.RenderStartTime;
                        finishTime = startTime.Add(clientInterval);
                        if (finishTime < PlanTime)
                        {
                            finishTime = PlanTime;
                        }
                        break;

                    default:
                        throw new Exception(string.Format("Недопустимое состояние для запроса [{0}]", clientRequest));
                }

                ClientRequestPlans.Add(new ClientRequestPlan(clientRequest, startTime, finishTime));
            }
            else if (clientRequest.State == ClientRequestState.Rendered)
            {
                Metrics.DailyWorkload = Metrics.DailyWorkload.Add(clientRequest.RenderFinishTime - clientRequest.RenderStartTime);
            }
        }

        public TimeSpan GetNearTimeInterval(TimeSpan startTime, Schedule schedule,
            ClientRequestType clientRequestType, TimeSpan clientInterval)
        {
            // Недоступные интервалы
            var reservedIntervals = new List<TimeInterval>();

            // Перерывы оператора
            reservedIntervals.AddRange(GetIgnoredIntervals(clientRequestType));
            // Запланированные запросы клиентов
            reservedIntervals.AddRange(clientRequestIntervals);

            var renderStartTime = startTime;

            var exceptions = reservedIntervals
                .Where(i => i.FinishTime >= startTime)
                .OrderBy(i => i.StartTime);

            foreach (var exception in exceptions)
            {
                // Игнорируем данный интервал
                if (exception.FinishTime < renderStartTime)
                {
                    continue;
                }

                var renderFinishTime = exception.StartTime;

                var interval = renderFinishTime - renderStartTime;
                // Найденный интервал обслуживания клиента больше или равен плановому?
                if (interval < clientInterval - schedule.Intersection)
                {
                    renderStartTime = exception.FinishTime;
                }
                else
                {
                    break;
                }
            }

            return renderStartTime;
        }

        public override string ToString()
        {
            return Operator.ToString();
        }

        private void ClientRequestPlans_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ClientRequestPlan c in e.NewItems)
                    {
                        c.Position = ++Metrics.LastPosition;

                        if (CurrentClientRequestPlan == null || CurrentClientRequestPlan.StartTime > c.StartTime)
                        {
                            CurrentClientRequestPlan = c;
                        }

                        var interval = new TimeInterval(c.StartTime, c.FinishTime);
                        clientRequestIntervals.Add(interval);

                        Metrics.PlaningWorkload += interval.Duration;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    CurrentClientRequestPlan = null;
                    clientRequestIntervals.Clear();
                    break;
            }
        }
    }
}