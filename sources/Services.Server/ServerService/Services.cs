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
        public async Task<DTO.IdentifiedEntityLink[]> GetServiceLinks()
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var services = session.CreateCriteria<Service>()
                        .AddOrder(Order.Asc("ServiceGroup"))
                        .AddOrder(Order.Asc("Code"))
                        .List<IdentifiedEntity>();

                    return Mapper.Map<IList<IdentifiedEntity>, DTO.IdentifiedEntityLink[]>(services);
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
                        .Add(Restrictions.IsNull("ServiceGroup"))
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
                    if (serviceGroup == null)
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
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    return Mapper.Map<Service, DTO.Service>(service);
                }
            });
        }

        public async Task<DTO.Service[]> FindServices(string query, int startIndex, int maxResults)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                {
                    ICriteria criteria = session.CreateCriteria<Service>()
                        .Add(Restrictions.Eq("IsActive", true))
                        .AddOrder(Order.Asc("Name"))
                        .SetFirstResult(startIndex)
                        .SetMaxResults(maxResults);

                    if (query.Length > 0)
                    {
                        criteria.Add(new Disjunction()
                            .Add(Restrictions.InsensitiveLike("Name", query, MatchMode.Anywhere))
                            .Add(Restrictions.InsensitiveLike("Tags", query, MatchMode.Anywhere)));
                    }

                    return Mapper.Map<IList<Service>, DTO.Service[]>(criteria.List<Service>());
                }
            });
        }

        public async Task<DTO.Service> EditService(DTO.Service source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Service service;

                    if (!source.Empty())
                    {
                        var serviceId = source.Id;
                        service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                        }
                    }
                    else
                    {
                        service = new Service();
                    }

                    service.IsActive = source.IsActive;
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
                    service.IsUseType = source.IsUseType;
                    service.LiveRegistrator = source.LiveRegistrator;
                    service.EarlyRegistrator = source.EarlyRegistrator;

                    if (source.ServiceGroup != null)
                    {
                        Guid serviceGroupId = source.ServiceGroup.Id;

                        ServiceGroup serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                        if (serviceGroup == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId),
                                string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                        }

                        service.ServiceGroup = serviceGroup;
                    }
                    else
                    {
                        service.ServiceGroup = null;
                    }

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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var criteria = session.CreateCriteria<Service>()
                        .Add(Restrictions.Lt("SortId", service.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    if (service.ServiceGroup != null)
                    {
                        criteria.Add(Restrictions.Eq("ServiceGroup", service.ServiceGroup));
                    }
                    else
                    {
                        criteria.Add(Restrictions.IsNull("ServiceGroup"));
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var criteria = session.CreateCriteria<Service>()
                        .Add(Restrictions.Gt("SortId", service.SortId))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1);

                    if (service.ServiceGroup != null)
                    {
                        criteria.Add(Restrictions.Eq("ServiceGroup", service.ServiceGroup));
                    }
                    else
                    {
                        criteria.Add(Restrictions.IsNull("ServiceGroup"));
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
    }
}