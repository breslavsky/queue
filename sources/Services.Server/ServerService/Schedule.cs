using AutoMapper;
using Junte.Data.NHibernate;
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
        public async Task<DTO.Schedule> GetDefaultWeekdaySchedule(DayOfWeek dayOfWeek)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var schedule = session.CreateCriteria<DefaultWeekdaySchedule>()
                        .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                        .SetMaxResults(1)
                        .UniqueResult<DefaultWeekdaySchedule>();
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(dayOfWeek), string.Format("Расписание по умолчанию на день недели [{0}] не найдено", dayOfWeek));
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> GetDefaultExceptionSchedule(DateTime scheduleDate)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    scheduleDate = scheduleDate.Date;

                    var schedule = session.CreateCriteria<DefaultExceptionSchedule>()
                        .Add(Expression.Eq("ScheduleDate", scheduleDate))
                        .SetMaxResults(1)
                        .UniqueResult<DefaultExceptionSchedule>();
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleDate), string.Format("Исключение в расписании на дату [{0}] не найдено", scheduleDate));
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> AddDefaultExceptionSchedule(DateTime scheduleDate)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    scheduleDate = scheduleDate.Date;

                    var schedule = session.CreateCriteria<DefaultExceptionSchedule>()
                        .Add(Expression.Eq("ScheduleDate", scheduleDate))
                        .SetMaxResults(1)
                        .UniqueResult<DefaultExceptionSchedule>();
                    if (schedule == null)
                    {
                        var dayOfWeek = scheduleDate.DayOfWeek;

                        var template = session.CreateCriteria<DefaultWeekdaySchedule>()
                            .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                            .SetMaxResults(1)
                            .UniqueResult<DefaultWeekdaySchedule>();

                        schedule = new DefaultExceptionSchedule()
                        {
                            ScheduleDate = scheduleDate,
                            StartTime = template.StartTime,
                            FinishTime = template.FinishTime,
                            IsWorked = template.IsWorked,
                            IsInterruption = template.IsInterruption,
                            InterruptionStartTime = template.InterruptionStartTime,
                            InterruptionFinishTime = template.InterruptionFinishTime,
                            ClientInterval = template.ClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation
                        };
                        session.Save(schedule);

                        foreach (var r in session.CreateCriteria<ServiceRendering>()
                                                     .Add(Expression.Eq("Schedule", template))
                                                     .AddOrder(Order.Asc("ServiceStep"))
                                                     .AddOrder(Order.Desc("Priority"))
                                                     .List<ServiceRendering>().ToArray())
                        {
                            var rendering = new ServiceRendering()
                            {
                                Schedule = schedule,
                                Operator = r.Operator,
                                Mode = r.Mode,
                                Priority = r.Priority
                            };

                            session.Save(rendering);
                        }
                    }

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        session.Refresh(schedule);

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> GetServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek)
        {
            checkPermission(UserRole.Manager | UserRole.Administrator);

            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var schedule = session.CreateCriteria<ServiceWeekdaySchedule>()
                        .Add(Expression.Eq("Service", service))
                        .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceWeekdaySchedule>();
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(dayOfWeek), string.Format("Расписание [{0}] не найдено для услуги [{1}]", dayOfWeek, service));
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> AddServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var schedule = session.CreateCriteria<ServiceWeekdaySchedule>()
                        .Add(Expression.Eq("Service", service))
                        .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceWeekdaySchedule>();
                    if (schedule == null)
                    {
                        var template = session.CreateCriteria<DefaultWeekdaySchedule>()
                             .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                             .SetMaxResults(1)
                             .UniqueResult<Schedule>();

                        schedule = new ServiceWeekdaySchedule()
                        {
                            Service = service,
                            DayOfWeek = dayOfWeek,

                            StartTime = template.StartTime,
                            FinishTime = template.FinishTime,
                            IsWorked = template.IsWorked,
                            IsInterruption = template.IsInterruption,
                            InterruptionStartTime = template.InterruptionStartTime,
                            InterruptionFinishTime = template.InterruptionFinishTime,
                            ClientInterval = template.ClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation
                        };
                        session.Save(schedule);

                        foreach (var r in session.CreateCriteria<ServiceRendering>()
                                                     .Add(Expression.Eq("Schedule", template))
                                                     .List<ServiceRendering>().ToArray())
                        {
                            var rendering = new ServiceRendering()
                            {
                                Schedule = schedule,
                                Operator = r.Operator,
                                Mode = r.Mode,
                                Priority = r.Priority
                            };

                            session.Save(rendering);
                        }
                    }

                    session.Save(schedule);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> GetServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    scheduleDate = scheduleDate.Date;

                    var schedule = session.CreateCriteria<ServiceExceptionSchedule>()
                        .Add(Expression.Eq("Service", service))
                        .Add(Expression.Eq("ScheduleDate", scheduleDate))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceExceptionSchedule>();
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleDate), string.Format("Исключение в расписании для услуги [{0}] на дату [{1}] не найдено", service, scheduleDate));
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> AddServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var schedule = session.CreateCriteria<ServiceExceptionSchedule>()
                        .Add(Expression.Eq("Service", service))
                        .Add(Expression.Eq("ScheduleDate", scheduleDate))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceExceptionSchedule>();
                    if (schedule == null)
                    {
                        var template = session.CreateCriteria<DefaultExceptionSchedule>()
                            .Add(Expression.Eq("ScheduleDate", scheduleDate))
                            .SetMaxResults(1)
                            .UniqueResult<Schedule>();

                        if (template == null)
                        {
                            var dayOfWeek = scheduleDate.DayOfWeek;

                            template = session.CreateCriteria<ServiceWeekdaySchedule>()
                                .Add(Expression.Eq("Service", service))
                                .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                                .SetMaxResults(1)
                                .UniqueResult<Schedule>();

                            if (template == null)
                            {
                                template = session.CreateCriteria<DefaultWeekdaySchedule>()
                                    .Add(Expression.Eq("DayOfWeek", dayOfWeek))
                                    .SetMaxResults(1)
                                    .UniqueResult<Schedule>();
                            }
                        }

                        schedule = new ServiceExceptionSchedule()
                        {
                            Service = service,
                            ScheduleDate = scheduleDate,

                            StartTime = template.StartTime,
                            FinishTime = template.FinishTime,
                            IsWorked = template.IsWorked,
                            IsInterruption = template.IsInterruption,
                            InterruptionStartTime = template.InterruptionStartTime,
                            InterruptionFinishTime = template.InterruptionFinishTime,
                            ClientInterval = template.ClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation
                        };
                        session.Save(schedule);

                        foreach (var r in session.CreateCriteria<ServiceRendering>()
                                                     .Add(Expression.Eq("Schedule", template))
                                                     .List<ServiceRendering>().ToArray())
                        {
                            var rendering = new ServiceRendering()
                            {
                                Schedule = schedule,
                                Operator = r.Operator,
                                Mode = r.Mode,
                                Priority = r.Priority
                            };

                            session.Save(rendering);
                        }
                    }

                    session.Save(schedule);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> GetSchedule(Guid scheduleId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var schedule = session.Get<Schedule>(scheduleId);
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId), string.Format("Расписание [{0}] не найдено", scheduleId));
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> EditSchedule(DTO.Schedule source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var scheduleId = source.Id;

                    var schedule = session.Get<Schedule>(scheduleId);
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId), string.Format("Расписание [{0}] не найдено", scheduleId));
                    }

                    schedule.StartTime = source.StartTime;
                    schedule.FinishTime = source.FinishTime;
                    schedule.IsWorked = source.IsWorked;
                    schedule.IsInterruption = source.IsInterruption;
                    schedule.InterruptionStartTime = source.InterruptionStartTime;
                    schedule.InterruptionFinishTime = source.InterruptionFinishTime;
                    schedule.ClientInterval = source.ClientInterval;
                    schedule.Intersection = source.Intersection;
                    schedule.MaxClientRequests = source.MaxClientRequests;
                    schedule.RenderingMode = source.RenderingMode;
                    schedule.EarlyStartTime = source.EarlyStartTime;
                    schedule.EarlyFinishTime = source.EarlyFinishTime;
                    schedule.EarlyReservation = source.EarlyReservation;

                    var errors = schedule.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(schedule);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task DeleteSchedule(Guid scheduleId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var schedule = session.Get<Schedule>(scheduleId);
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId), string.Format("Расписание [{0}] не найдено", scheduleId));
                    }

                    session.Delete(schedule);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }
                }
            });
        }

        public async Task<DTO.ServiceRendering[]> GetServiceRenderings(Guid scheduleId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
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
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
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
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Guid serviceRenderingId = source.Id;

                    ServiceRendering serviceRendering;

                    if (serviceRenderingId != Guid.Empty)
                    {
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

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
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
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceRendering = session.Get<ServiceRendering>(serviceRenderingId);
                    if (serviceRendering == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceRenderingId), string.Format("Обслуживание услуги [{0}] не найдено", serviceRenderingId));
                    }

                    session.Delete(serviceRendering);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
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