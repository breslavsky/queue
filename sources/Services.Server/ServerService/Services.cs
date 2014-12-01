using AutoMapper;
using Junte.Data.NHibernate;
using NHibernate;
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
        public async Task<DTO.IdentifiedEntity[]> GetServiceList()
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var services = session.CreateCriteria<Service>()
                        .AddOrder(Order.Asc("ServiceGroup"))
                        .AddOrder(Order.Asc("Code"))
                        .List<Service>();

                    return Mapper.Map<IList<Service>, DTO.IdentifiedEntity[]>(services);
                }
            });
        }

        public async Task<DTO.Service[]> GetRootServices()
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var services = session.CreateCriteria<Service>()
                        .Add(Expression.IsNull("ServiceGroup"))
                        .AddOrder(Order.Asc("SortId"))
                        .List<Service>();

                    return Mapper.Map<IList<Service>, DTO.Service[]>(services);
                }
            });
        }

        public async Task<DTO.Service[]> GetServices(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroupId == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    var services = session.CreateCriteria<Service>()
                        .Add(Restrictions.Eq("ServiceGroup", serviceGroup))
                        .AddOrder(Order.Asc("SortId"))
                        .List<Service>();

                    return Mapper.Map<IList<Service>, DTO.Service[]>(services);
                }
            });
        }

        public async Task<DTO.Service> GetService(Guid serviceId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Service service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    return Mapper.Map<Service, DTO.Service>(service);
                }
            });
        }

        public async Task<DTO.Service[]> FindServices(string filter, int startIndex, int maxResults)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                {
                    ICriteria criteria = session.CreateCriteria<Service>()
                        .Add(Expression.Eq("IsActive", true))
                        .AddOrder(Order.Asc("Name"))
                        .SetFirstResult(startIndex)
                        .SetMaxResults(maxResults);

                    if (filter.Length > 0)
                    {
                        criteria.Add(new Disjunction()
                            .Add(Expression.InsensitiveLike("Name", filter, MatchMode.Anywhere))
                            .Add(Expression.InsensitiveLike("Tags", filter, MatchMode.Anywhere)));
                    }

                    return Mapper.Map<IList<Service>, DTO.Service[]>(criteria.List<Service>());
                }
            });
        }

        public async Task<DTO.Service> AddRootService()
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = new Service();

                    var errors = service.Validate();
                    if (errors.Length > 0)
                    {
                        logger.Error(ValidationError.ToException(errors));
                    }

                    session.Save(service);
                    transaction.Commit();

                    return Mapper.Map<Service, DTO.Service>(service);
                }
            });
        }

        public async Task<DTO.Service> AddService(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = new Service();

                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    service.ServiceGroup = serviceGroup;

                    var errors = service.Validate();
                    if (errors.Length > 0)
                    {
                        logger.Error(ValidationError.ToException(errors));
                    }

                    session.Save(service);
                    transaction.Commit();

                    return Mapper.Map<Service, DTO.Service>(service);
                }
            });
        }

        public async Task<DTO.Service> EditService(DTO.Service source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceId = source.Id;

                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    service.Code = source.Code;
                    service.Priority = source.Priority;
                    service.Name = source.Name;
                    service.Comment = source.Comment;
                    service.Tags = source.Tags;
                    service.Description = source.Description;
                    service.Link = source.Link;
                    service.MaxSubjects = source.MaxSubjects;
                    service.MaxEarlyDays = source.MaxEarlyDays;
                    service.IsPlanSubjects = source.IsPlanSubjects;
                    service.ClientRequire = source.ClientRequire;
                    service.ClientCallDelay = source.ClientCallDelay;
                    service.TimeIntervalRounding = source.TimeIntervalRounding;
                    service.Type = source.Type;
                    service.LiveRegistrator = source.LiveRegistrator;
                    service.EarlyRegistrator = source.EarlyRegistrator;

                    var errors = service.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(service);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(service);
                    }

                    return Mapper.Map<Service, DTO.Service>(service);
                }
            });
        }

        public async Task MoveService(Guid serviceId, Guid serviceGroupId)
        {
            await Task.Run(() =>
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

                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    service.ServiceGroup = serviceGroup;

                    session.Save(service);
                    transaction.Commit();
                }
            });
        }

        public async Task DeleteService(Guid serviceId)
        {
            await Task.Run(() =>
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

                    session.Delete(service);
                    transaction.Commit();
                }
            });
        }

        public async Task<bool> ServiceUp(Guid serviceId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var criteria = session.CreateCriteria<Service>()
                        .Add(Expression.Lt("SortId", service.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    if (service.ServiceGroup != null)
                    {
                        criteria.Add(Expression.Eq("ServiceGroup", service.ServiceGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ServiceGroup"));
                    }

                    var prevService = criteria.UniqueResult<Service>();
                    if (prevService == null)
                    {
                        return false;
                    }

                    long sortId = service.SortId;

                    service.SortId = prevService.SortId;
                    session.Save(service);

                    prevService.SortId = sortId;

                    session.Save(prevService);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> ServiceDown(Guid serviceId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var criteria = session.CreateCriteria<Service>()
                        .Add(Expression.Gt("SortId", service.SortId))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1);

                    if (service.ServiceGroup != null)
                    {
                        criteria.Add(Expression.Eq("ServiceGroup", service.ServiceGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ServiceGroup"));
                    }

                    var nextService = criteria.UniqueResult<Service>();
                    if (nextService == null)
                    {
                        return false;
                    }

                    long sortId = service.SortId;

                    service.SortId = nextService.SortId;
                    session.Save(service);

                    nextService.SortId = sortId;

                    session.Save(nextService);
                    session.Flush();
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task ChangeServiceActivity(Guid serviceId, bool isActive)
        {
            await Task.Run(() =>
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

                    service.IsActive = isActive;

                    session.Save(service);
                    transaction.Commit();
                }
            });
        }

        public async Task<DTO.IdentifiedEntity[]> GetServiceList(Guid serviceId)
        {
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

                    var serviceSteps = session.CreateCriteria<ServiceStep>()
                        .Add(Restrictions.Eq("Service", service))
                        .AddOrder(Order.Asc("SortId"))
                        .List<ServiceStep>();
                    return Mapper.Map<IList<ServiceStep>, DTO.IdentifiedEntity[]>(serviceSteps);
                }
            });
        }

        public async Task<DTO.ServiceStep[]> GetServiceSteps(Guid serviceId)
        {
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

                    var serviceSteps = session.CreateCriteria<ServiceStep>()
                        .Add(Restrictions.Eq("Service", service))
                        .AddOrder(Order.Asc("SortId"))
                        .List<ServiceStep>();
                    return Mapper.Map<IList<ServiceStep>, DTO.ServiceStep[]>(serviceSteps);
                }
            });
        }

        public async Task<DTO.ServiceStep> GetServiceStep(Guid serviceStepId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStep = session.Get<ServiceStep>(serviceStepId);
                    if (serviceStep == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStep), string.Format("Этап услуги [{0}] не найден", serviceStep));
                    }

                    return Mapper.Map<ServiceStep, DTO.ServiceStep>(serviceStep);
                }
            });
        }

        public async Task<DTO.ServiceStep> AddServiceStep(Guid serviceId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStep = new ServiceStep();

                    if (serviceId != Guid.Empty)
                    {
                        var service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                        }

                        serviceStep.Service = service;
                    }

                    var errors = serviceStep.Validate();
                    if (errors.Length > 0)
                    {
                        logger.Error(ValidationError.ToException(errors));
                    }

                    session.Save(serviceStep);
                    transaction.Commit();

                    return Mapper.Map<ServiceStep, DTO.ServiceStep>(serviceStep);
                }
            });
        }

        public async Task<DTO.ServiceStep> EditServiceStep(DTO.ServiceStep source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStepId = source.Id;

                    var serviceStep = session.Get<ServiceStep>(serviceStepId);
                    if (serviceStep == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId), string.Format("Этап услуги [{0}] не найден", serviceStepId));
                    }

                    serviceStep.Name = source.Name;

                    var errors = serviceStep.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(serviceStep);
                    transaction.Commit();

                    return Mapper.Map<ServiceStep, DTO.ServiceStep>(serviceStep);
                }
            });
        }

        public async Task DeleteServiceStep(Guid serviceStepId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStep = session.Get<ServiceStep>(serviceStepId);
                    if (serviceStep == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId), string.Format("Этап услуги [{0}] не найден", serviceStepId));
                    }

                    session.Delete(serviceStep);
                    transaction.Commit();
                }
            });
        }

        public async Task<bool> ServiceStepUp(Guid serviceStepId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStep = session.Get<ServiceStep>(serviceStepId);
                    if (serviceStep == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId), string.Format("Этап услуги [{0}] не найден", serviceStepId));
                    }

                    var criteria = session.CreateCriteria<ServiceStep>()
                        .Add(Expression.Eq("Service", serviceStep.Service))
                        .Add(Expression.Lt("SortId", serviceStep.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    var prevServiceStep = criteria.UniqueResult<ServiceStep>();
                    if (prevServiceStep == null)
                    {
                        return false;
                    }

                    long sortId = serviceStep.SortId;

                    serviceStep.SortId = prevServiceStep.SortId;
                    session.Save(serviceStep);

                    prevServiceStep.SortId = sortId;

                    session.Save(prevServiceStep);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> ServiceStepDown(Guid serviceStepId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceStep = session.Get<ServiceStep>(serviceStepId);
                    if (serviceStep == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId), string.Format("Этап услуги [{0}] не найден", serviceStepId));
                    }

                    var criteria = session.CreateCriteria<ServiceStep>()
                        .Add(Expression.Gt("Service", serviceStep.Service))
                        .Add(Expression.Lt("SortId", serviceStep.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    var nextServiceStep = criteria.UniqueResult<ServiceStep>();
                    if (nextServiceStep == null)
                    {
                        return false;
                    }

                    long sortId = serviceStep.SortId;

                    serviceStep.SortId = nextServiceStep.SortId;
                    session.Save(serviceStep);

                    nextServiceStep.SortId = sortId;

                    session.Save(nextServiceStep);
                    transaction.Commit();

                    return true;
                }
            });
        }
    }
}