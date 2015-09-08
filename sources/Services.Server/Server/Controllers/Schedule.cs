using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
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
                using (var session = SessionProvider.OpenSession())
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.DefaultSchedule);

                using (var session = SessionProvider.OpenSession())
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.DefaultSchedule);

                using (var session = SessionProvider.OpenSession())
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
                            LiveClientInterval = template.LiveClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation,
                            EarlyClientInterval = template.EarlyClientInterval
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

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        // Why?
                        //session.Refresh(schedule);

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                }
            });
        }

        public async Task<DTO.Schedule> GetServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek)
        {
            CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
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
                            LiveClientInterval = template.LiveClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation,
                            EarlyClientInterval = template.EarlyClientInterval
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

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
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
                            LiveClientInterval = template.LiveClientInterval,
                            Intersection = template.Intersection,
                            MaxClientRequests = template.MaxClientRequests,
                            RenderingMode = template.RenderingMode,
                            EarlyStartTime = template.EarlyStartTime,
                            EarlyFinishTime = template.EarlyFinishTime,
                            EarlyReservation = template.EarlyReservation,
                            EarlyClientInterval = template.EarlyClientInterval
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

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
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
                using (var session = SessionProvider.OpenSession())
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
                CheckPermission(UserRole.Administrator);

                using (var session = SessionProvider.OpenSession())
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
                    schedule.LiveClientInterval = source.LiveClientInterval;
                    schedule.Intersection = source.Intersection;
                    schedule.MaxClientRequests = source.MaxClientRequests;
                    schedule.RenderingMode = source.RenderingMode;
                    schedule.EarlyStartTime = source.EarlyStartTime;
                    schedule.EarlyFinishTime = source.EarlyFinishTime;
                    schedule.EarlyReservation = source.EarlyReservation;
                    schedule.EarlyClientInterval = source.EarlyClientInterval;

                    var errors = schedule.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(schedule);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
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
                CheckPermission(UserRole.Administrator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var schedule = session.Get<Schedule>(scheduleId);
                    if (schedule == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(scheduleId), string.Format("Расписание [{0}] не найдено", scheduleId));
                    }

                    session.Delete(schedule);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.ServiceSchedule);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }
                }
            });
        }
    }
}