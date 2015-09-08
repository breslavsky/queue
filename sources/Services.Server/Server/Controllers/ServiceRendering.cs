using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.ServiceRendering[]> GetServiceRenderings(Guid scheduleId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var schedule = session.Get<Schedule>(scheduleId);
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId), string.Format("Расписание [{0}] не найдено", scheduleId));
                    }

                    var serviceRenderings = session.CreateCriteria<ServiceRendering>()
                        .Add(Restrictions.Eq("Schedule", schedule))
                        .AddOrder(Order.Asc("ServiceStep"))
                        .AddOrder(Order.Desc("Priority"))
                        .List<ServiceRendering>();
                    return Mapper.Map<IList<ServiceRendering>, DTO.ServiceRendering[]>(serviceRenderings);
                }
            });
        }

        public async Task<DTO.ServiceRendering> GetServiceRendering(Guid serviceRenderingId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceRendering = session.Get<ServiceRendering>(serviceRenderingId);
                    if (serviceRendering == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceRenderingId), string.Format("Обслуживание [{0}] не найдено", serviceRenderingId));
                    }

                    return Mapper.Map<ServiceRendering, DTO.ServiceRendering>(serviceRendering);
                }
            });
        }

        public async Task<DTO.ServiceRendering> EditServiceRendering(DTO.ServiceRendering source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceRendering serviceRendering;

                    if (!source.Empty())
                    {
                        Guid serviceRenderingId = source.Id;
                        serviceRendering = session.Get<ServiceRendering>(serviceRenderingId);
                        if (serviceRendering == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceRenderingId),
                                string.Format("Обслуживание услуги [{0}] не найдено", serviceRenderingId));
                        }
                    }
                    else
                    {
                        serviceRendering = new ServiceRendering();
                    }

                    if (source.Schedule != null)
                    {
                        var scheduleId = source.Schedule.Id;

                        var schedule = session.Get<Schedule>(scheduleId);
                        if (schedule == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId),
                                string.Format("Расписание [{0}] не найдено", scheduleId));
                        }
                        serviceRendering.Schedule = schedule;
                    }
                    else
                    {
                        serviceRendering.Schedule = null;
                    }

                    if (source.Operator != null)
                    {
                        var operatorId = source.Operator.Id;

                        var queueOperator = session.Get<Operator>(operatorId);
                        if (queueOperator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorId),
                                string.Format("Оператор [{0}] не найден", operatorId));
                        }
                        serviceRendering.Operator = queueOperator;
                    }
                    else
                    {
                        serviceRendering.Operator = null;
                    }

                    if (source.ServiceStep != null)
                    {
                        var serviceStepId = source.ServiceStep.Id;

                        var serviceStep = session.Get<ServiceStep>(serviceStepId);
                        if (serviceStep == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId),
                                string.Format("Этап услуги [{0}] не найден", serviceStepId));
                        }
                        serviceRendering.ServiceStep = serviceStep;
                    }
                    else
                    {
                        serviceRendering.ServiceStep = null;
                    }

                    serviceRendering.Mode = source.Mode;
                    serviceRendering.Priority = source.Priority;

                    var errors = serviceRendering.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(serviceRendering);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceRenderings);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<ServiceRendering, DTO.ServiceRendering>(serviceRendering);
                }
            });
        }

        public async Task DeleteServiceRendering(Guid serviceRenderingId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceRendering = session.Get<ServiceRendering>(serviceRenderingId);
                    if (serviceRendering == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceRenderingId), string.Format("Обслуживание услуги [{0}] не найдено", serviceRenderingId));
                    }

                    session.Delete(serviceRendering);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceRenderings);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }
                }
            });
        }
    }
}