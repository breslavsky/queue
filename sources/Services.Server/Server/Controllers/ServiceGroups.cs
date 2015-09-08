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
    partial class ServerService
    {
        public async Task<DTO.ServiceGroup[]> GetRootServiceGroups()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroups = session.CreateCriteria<ServiceGroup>()
                        .Add(Expression.IsNull("ParentGroup"))
                        .AddOrder(Order.Asc("SortId"))
                        .List<ServiceGroup>();

                    return Mapper.Map<IList<ServiceGroup>, DTO.ServiceGroup[]>(serviceGroups);
                }
            });
        }

        public async Task<DTO.ServiceGroup[]> GetServiceGroups(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Родительская группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    var serviceGroups = session.CreateCriteria<ServiceGroup>()
                        .Add(Restrictions.Eq("ParentGroup", serviceGroup))
                        .AddOrder(Order.Asc("SortId"))
                        .List<ServiceGroup>();

                    return Mapper.Map<IList<ServiceGroup>, DTO.ServiceGroup[]>(serviceGroups);
                }
            });
        }

        public async Task<DTO.ServiceGroup> GetServiceGroup(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    return Mapper.Map<ServiceGroup, DTO.ServiceGroup>(serviceGroup);
                }
            });
        }

        public async Task<DTO.ServiceGroup> EditServiceGroup(DTO.ServiceGroup source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceGroup serviceGroup;

                    if (!source.Empty())
                    {
                        var serviceGroupId = source.Id;
                        serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                        if (serviceGroup == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId),
                                string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                        }
                    }
                    else
                    {
                        serviceGroup = new ServiceGroup();
                    }

                    serviceGroup.IsActive = source.IsActive;
                    serviceGroup.Code = source.Code;
                    serviceGroup.Name = source.Name;
                    serviceGroup.Comment = source.Comment;
                    serviceGroup.Description = source.Description;
                    serviceGroup.Columns = source.Columns;
                    serviceGroup.Rows = source.Rows;
                    serviceGroup.Color = source.Color;

                    if (source.ParentGroup != null)
                    {
                        Guid parentGroupId = source.ParentGroup.Id;

                        ServiceGroup parentGroup = session.Get<ServiceGroup>(parentGroupId);
                        if (parentGroup == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parentGroupId),
                                string.Format("Группа услуг [{0}] не найдена", parentGroupId));
                        }

                        serviceGroup.ParentGroup = parentGroup;
                    }
                    else
                    {
                        serviceGroup.ParentGroup = null;
                    }

                    var errors = serviceGroup.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(serviceGroup);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(serviceGroup);
                    }

                    return Mapper.Map<ServiceGroup, DTO.ServiceGroup>(serviceGroup);
                }
            });
        }

        public async Task MoveServiceGroup(Guid sourceGroupId, Guid targetGroupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceGroup sourceGroup = session.Get<ServiceGroup>(sourceGroupId);
                    if (sourceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sourceGroupId), string.Format("Исходная группа услуг [{0}] не найдена", sourceGroupId));
                    }

                    ServiceGroup targetGroup = session.Get<ServiceGroup>(targetGroupId);
                    if (targetGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(targetGroupId), string.Format("Целевая группа услуг [{0}] не найдена", targetGroupId));
                    }

                    sourceGroup.ParentGroup = targetGroup;

                    session.Save(sourceGroup);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(sourceGroup);
                    }
                }
            });
        }

        public async Task MoveServiceGroupToRoot(Guid sourceGroupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceGroup sourceGroup = session.Get<ServiceGroup>(sourceGroupId);
                    if (sourceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sourceGroupId), string.Format("Исхоодная группа услуг [{0}] не найдена", sourceGroupId));
                    }

                    sourceGroup.ParentGroup = null;

                    session.Save(sourceGroup);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(sourceGroup);
                    }
                }
            });
        }

        public async Task<bool> ServiceGroupUp(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    ICriteria criteria = session.CreateCriteria<ServiceGroup>()
                        .Add(Expression.Lt("SortId", serviceGroup.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    if (serviceGroup.ParentGroup != null)
                    {
                        criteria.Add(Expression.Eq("ParentGroup", serviceGroup.ParentGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ParentGroup"));
                    }

                    ServiceGroup prevServiceGroup = criteria.UniqueResult<ServiceGroup>();
                    if (prevServiceGroup == null)
                    {
                        return false;
                    }

                    long sortId = serviceGroup.SortId;

                    serviceGroup.SortId = prevServiceGroup.SortId;
                    session.Save(serviceGroup);

                    prevServiceGroup.SortId = sortId;

                    session.Save(prevServiceGroup);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> ServiceGroupDown(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    var criteria = session.CreateCriteria<ServiceGroup>()
                        .Add(Expression.Gt("SortId", serviceGroup.SortId))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1);

                    if (serviceGroup.ParentGroup != null)
                    {
                        criteria.Add(Expression.Eq("ParentGroup", serviceGroup.ParentGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ParentGroup"));
                    }

                    var nextServiceGroup = criteria.UniqueResult<ServiceGroup>();
                    if (nextServiceGroup == null)
                    {
                        return false;
                    }

                    long sortId = serviceGroup.SortId;

                    serviceGroup.SortId = nextServiceGroup.SortId;
                    session.Save(serviceGroup);

                    nextServiceGroup.SortId = sortId;

                    session.Save(nextServiceGroup);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> ServiceGroupActivate(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    if (!serviceGroup.IsActive)
                    {
                        serviceGroup.IsActive = true;

                        session.Save(serviceGroup);
                        transaction.Commit();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            });
        }

        public async Task<bool> ServiceGroupDeactivate(Guid serviceGroupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    if (serviceGroup.IsActive)
                    {
                        serviceGroup.IsActive = false;

                        session.Save(serviceGroup);
                        transaction.Commit();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            });
        }

        public async Task DeleteServiceGroup(Guid serviceGroupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceGroup = session.Get<ServiceGroup>(serviceGroupId);
                    if (serviceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId), string.Format("Группа услуг [{0}] не найдена", serviceGroupId));
                    }

                    session.Delete(serviceGroup);
                    transaction.Commit();
                }
            });
        }
    }
}