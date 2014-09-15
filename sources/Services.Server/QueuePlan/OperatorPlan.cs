using log4net;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Queue.Services.Server
{
    public class OperatorPlan
    {
        public ClientRequestPlan currentClientRequestPlan;
        private static readonly ILog logger = LogManager.GetLogger(typeof(OperatorPlan));

        private List<TimeInterval> clientRequestIntervals;

        public OperatorPlan(Operator queueOperator)
        {
            Operator = queueOperator;

            Metrics = new OperatorPlanMetrics(queueOperator);

            ClientRequestPlans = new ObservableCollection<ClientRequestPlan>();
            ClientRequestPlans.CollectionChanged += ClientRequestPlans_CollectionChanged;

            clientRequestIntervals = new List<TimeInterval>();
        }

        public Operator Operator { get; private set; }

        public ObservableCollection<ClientRequestPlan> ClientRequestPlans { get; private set; }

        public OperatorPlanMetrics Metrics { get; private set; }

        public TimeSpan PlanTime { get; set; }

        public ClientRequestPlan CurrentClientRequestPlan
        {
            get { return currentClientRequestPlan; }
            private set
            {
                currentClientRequestPlan = value;
                if (currentClientRequestPlan == null)
                {
                    Metrics.Reset();
                }
            }
        }

        public TimeSpan GetNearTimeInterval(TimeSpan startTime, Schedule schedule, int subjects = 1)
        {
            // Недоступные интервалы
            var reservedIntervals = new List<TimeInterval>(clientRequestIntervals);

            // Если установлен перерыв у оператора
            if (Operator.IsInterruption)
            {
                reservedIntervals.Add(new TimeInterval(Operator.InterruptionStartTime, Operator.InterruptionFinishTime));
            }

            // Если установлен перерыв у расписания
            if (schedule.IsInterruption)
            {
                reservedIntervals.Add(new TimeInterval(schedule.InterruptionStartTime, schedule.InterruptionFinishTime));
            }

            var clientInterval = TimeSpan.FromTicks(schedule.ClientInterval.Ticks * subjects);

            var renderStartTime = startTime;

            foreach (var exception in reservedIntervals
                .Where<TimeInterval>(i => i.FinishTime >= startTime)
                .OrderBy(i => i.FinishTime))
            {
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

        public void AddClientRequest(ClientRequest clientRequest, Schedule schedule)
        {
            TimeSpan startTime = TimeSpan.Zero, finishTime = TimeSpan.Zero;

            if (!clientRequest.IsClosed)
            {
                var clientInterval = TimeSpan.FromTicks(schedule.ClientInterval.Ticks * clientRequest.Subjects);

                switch (clientRequest.State)
                {
                    case ClientRequestState.Postponed:
                    case ClientRequestState.Waiting:
                        startTime = clientRequest.RequestTime;
                        if (startTime < schedule.StartTime)
                        {
                            startTime = schedule.StartTime;
                        }
                        if (startTime < PlanTime)
                        {
                            startTime = PlanTime;
                        }
                        startTime = GetNearTimeInterval(startTime, schedule);
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
                Metrics.Workload = Metrics.Workload.Add(clientRequest.RenderFinishTime - clientRequest.RenderStartTime);
            }
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

                        Metrics.Capacity += interval.Duration;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    CurrentClientRequestPlan = null;
                    clientRequestIntervals.Clear();
                    break;

                default:
                    break;
            }
        }
    }
}