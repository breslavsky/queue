using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var criteria = session.CreateCriteria<QueuePlanMetric>()
                        .Add(Restrictions.Eq("Year", year))
                        .AddOrder(Order.Desc("Year"));

                    if (month != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Month", month))
                            .AddOrder(Order.Desc("Month"));
                    }

                    if (day != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Day", day))
                            .AddOrder(Order.Desc("Day"));
                    }

                    if (hour != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Hour", hour))
                            .AddOrder(Order.Desc("Hour"));
                    }

                    if (minute != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Minute", minute))
                            .AddOrder(Order.Desc("Minute"));
                    }

                    if (second != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Second", second))
                            .AddOrder(Order.Desc("Second"));
                    }

                    var metric = criteria
                        .SetMaxResults(1)
                        .UniqueResult<QueuePlanMetric>();
                    if (metric == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(), "Метрика не найдена");
                    }

                    return Mapper.Map<QueuePlanMetric, DTO.QueuePlanMetric>(metric);
                }
            });
        }

        public async Task<DTO.QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hour, int minute, int second, Guid serviceId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Service service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var criteria = session.CreateCriteria<QueuePlanServiceMetric>()
                        .Add(Restrictions.Eq("Service", service))
                        .Add(Restrictions.Eq("Year", year))
                        .AddOrder(Order.Desc("Year"));

                    if (month != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Month", month))
                            .AddOrder(Order.Desc("Month"));
                    }

                    if (day != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Day", day))
                            .AddOrder(Order.Desc("Day"));
                    }

                    if (hour != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Hour", hour))
                            .AddOrder(Order.Desc("Hour"));
                    }

                    if (minute != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Minute", minute))
                            .AddOrder(Order.Desc("Minute"));
                    }

                    if (second != 0)
                    {
                        criteria
                            .Add(Restrictions.Eq("Second", second))
                            .AddOrder(Order.Desc("Second"));
                    }

                    var metric = criteria
                        .SetMaxResults(1)
                        .UniqueResult<QueuePlanServiceMetric>();
                    if (metric == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(), "Метрика не найдена");
                    }

                    return Mapper.Map<QueuePlanServiceMetric, DTO.QueuePlanServiceMetric>(metric);
                }
            });
        }
    }
}