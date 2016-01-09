using AutoMapper;
using Junte.Data.NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public partial class LifeSituationService : StandardServerService, ILifeSituationService
    {
        public LifeSituationService()
            : base()
        {
        }

        public async Task<DTO.LifeSituationGroup[]> GetRootGroups()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var groups = session.CreateCriteria<LifeSituationGroup>()
                        .Add(Expression.IsNull("ParentGroup"))
                        .AddOrder(Order.Asc("SortId"))
                        .List<LifeSituationGroup>();

                    return Mapper.Map<IList<LifeSituationGroup>, DTO.LifeSituationGroup[]>(groups);
                }
            });
        }

        public async Task<DTO.LifeSituationGroup[]> GetGroups(Guid groupId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Родительская группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    var groups = session.CreateCriteria<LifeSituationGroup>()
                        .Add(Restrictions.Eq("ParentGroup", group))
                        .AddOrder(Order.Asc("SortId"))
                        .List<LifeSituationGroup>();

                    return Mapper.Map<IList<LifeSituationGroup>, DTO.LifeSituationGroup[]>(groups);
                }
            });
        }

        public async Task<DTO.LifeSituationGroup> GetGroup(Guid groupId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    return Mapper.Map<LifeSituationGroup, DTO.LifeSituationGroup>(group);
                }
            });
        }

        public async Task<DTO.LifeSituationGroup> EditGroup(DTO.LifeSituationGroup source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    LifeSituationGroup group;

                    if (!source.Empty())
                    {
                        var serviceGroupId = source.Id;
                        group = session.Get<LifeSituationGroup>(serviceGroupId);
                        if (group == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceGroupId),
                                string.Format("Группа жизненной ситуации [{0}] не найдена", serviceGroupId));
                        }
                    }
                    else
                    {
                        group = new LifeSituationGroup();
                    }

                    group.IsActive = source.IsActive;
                    group.Code = source.Code;
                    group.Name = source.Name;
                    group.Comment = source.Comment;
                    group.Description = source.Description;
                    group.Columns = source.Columns;
                    group.Rows = source.Rows;
                    group.Color = source.Color;
                    group.FontSize = source.FontSize;

                    if (source.ParentGroup != null)
                    {
                        Guid parentGroupId = source.ParentGroup.Id;

                        var parentGroup = session.Get<LifeSituationGroup>(parentGroupId);
                        if (parentGroup == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parentGroupId),
                                string.Format("Группа жизненной ситуации [{0}] не найдена", parentGroupId));
                        }

                        group.ParentGroup = parentGroup;
                    }
                    else
                    {
                        group.ParentGroup = null;
                    }

                    var errors = group.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(group);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(group);
                    }

                    return Mapper.Map<LifeSituationGroup, DTO.LifeSituationGroup>(group);
                }
            });
        }

        public async Task MoveGroup(Guid sourceGroupId, Guid targetGroupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var sourceGroup = session.Get<LifeSituationGroup>(sourceGroupId);
                    if (sourceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sourceGroupId),
                            string.Format("Исходная группа жизненной ситуации [{0}] не найдена", sourceGroupId));
                    }

                    var targetGroup = session.Get<LifeSituationGroup>(targetGroupId);
                    if (targetGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(targetGroupId),
                            string.Format("Целевая группа жизненной ситуации [{0}] не найдена", targetGroupId));
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

        public async Task MoveGroupToRoot(Guid sourceGroupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var sourceGroup = session.Get<LifeSituationGroup>(sourceGroupId);
                    if (sourceGroup == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sourceGroupId),
                            string.Format("Исходная группа жизненной ситуации [{0}] не найдена", sourceGroupId));
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

        public async Task<bool> GroupUp(Guid groupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    var criteria = session.CreateCriteria<LifeSituationGroup>()
                        .Add(Expression.Lt("SortId", group.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    if (group.ParentGroup != null)
                    {
                        criteria.Add(Expression.Eq("ParentGroup", group.ParentGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ParentGroup"));
                    }

                    var prevGroup = criteria.UniqueResult<LifeSituationGroup>();
                    if (prevGroup == null)
                    {
                        return false;
                    }

                    long sortId = group.SortId;

                    group.SortId = prevGroup.SortId;
                    session.Save(group);

                    prevGroup.SortId = sortId;

                    session.Save(prevGroup);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> GroupDown(Guid groupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    var criteria = session.CreateCriteria<LifeSituationGroup>()
                        .Add(Expression.Gt("SortId", group.SortId))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1);

                    if (group.ParentGroup != null)
                    {
                        criteria.Add(Expression.Eq("ParentGroup", group.ParentGroup));
                    }
                    else
                    {
                        criteria.Add(Expression.IsNull("ParentGroup"));
                    }

                    var nextGroup = criteria.UniqueResult<LifeSituationGroup>();
                    if (nextGroup == null)
                    {
                        return false;
                    }

                    long sortId = group.SortId;

                    group.SortId = nextGroup.SortId;
                    session.Save(group);

                    nextGroup.SortId = sortId;

                    session.Save(nextGroup);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> GroupActivate(Guid groupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    if (!group.IsActive)
                    {
                        group.IsActive = true;

                        session.Save(group);
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

        public async Task<bool> GroupDeactivate(Guid groupId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    if (group.IsActive)
                    {
                        group.IsActive = false;

                        session.Save(group);
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

        public async Task DeleteGroup(Guid groupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    session.Delete(group);
                    transaction.Commit();
                }
            });
        }

        public async Task<DTO.LifeSituation[]> GetRootLifeSituations()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituations = session.CreateCriteria<LifeSituation>()
                        .Add(Restrictions.IsNull("LifeSituationGroup"))
                        .AddOrder(Order.Asc("SortId"))
                        .List<LifeSituation>();

                    return Mapper.Map<IList<LifeSituation>, DTO.LifeSituation[]>(lifeSituations);
                }
            });
        }

        public async Task<DTO.LifeSituation[]> GetLifeSituations(Guid groupId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа [{0}] не найдена", groupId));
                    }

                    var lifeSituations = session.CreateCriteria<LifeSituation>()
                        .Add(Restrictions.Eq("LifeSituationGroup", group))
                        .AddOrder(Order.Asc("SortId"))
                        .List<LifeSituation>();

                    return Mapper.Map<IList<LifeSituation>, DTO.LifeSituation[]>(lifeSituations);
                }
            });
        }

        public async Task<DTO.LifeSituation> GetLifeSituation(Guid lifeSituationId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                    if (lifeSituation == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                            string.Format("Услуга [{0}] не найдена", lifeSituationId));
                    }

                    return Mapper.Map<LifeSituation, DTO.LifeSituation>(lifeSituation);
                }
            });
        }

        public async Task<DTO.LifeSituation> EditLifeSituation(DTO.LifeSituation source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    LifeSituation lifeSituation;

                    if (!source.Empty())
                    {
                        var lifeSituationId = source.Id;
                        lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                        if (lifeSituation == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                                string.Format("Жизненная ситуация [{0}] не найдена", lifeSituationId));
                        }
                    }
                    else
                    {
                        lifeSituation = new LifeSituation();
                    }

                    lifeSituation.IsActive = source.IsActive;
                    lifeSituation.Code = source.Code;
                    lifeSituation.Name = source.Name;
                    lifeSituation.Comment = source.Comment;
                    lifeSituation.Description = source.Description;
                    lifeSituation.Color = source.Color;
                    lifeSituation.FontSize = source.FontSize;

                    if (source.LifeSituationGroup != null)
                    {
                        Guid groupId = source.LifeSituationGroup.Id;

                        var group = session.Get<LifeSituationGroup>(groupId);
                        if (group == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                                string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                        }

                        lifeSituation.LifeSituationGroup = group;
                    }
                    else
                    {
                        lifeSituation.LifeSituationGroup = null;
                    }

                    if (source.Service != null)
                    {
                        Guid serviceId = source.Service.Id;

                        var service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                string.Format("Услуга [{0}] не найдена", serviceId));
                        }

                        lifeSituation.Service = service;
                    }
                    else
                    {
                        lifeSituation.Service = null;
                    }

                    var errors = lifeSituation.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(lifeSituation);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(lifeSituation);
                    }

                    return Mapper.Map<LifeSituation, DTO.LifeSituation>(lifeSituation);
                }
            });
        }

        public async Task MoveLifeSituation(Guid lifeSituationId, Guid groupId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                    if (lifeSituation == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                            string.Format("Жизненная ситуация [{0}] не найдена", lifeSituationId));
                    }

                    var group = session.Get<LifeSituationGroup>(groupId);
                    if (group == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(groupId),
                            string.Format("Группа жизненной ситуации [{0}] не найдена", groupId));
                    }

                    lifeSituation.LifeSituationGroup = group;

                    session.Save(lifeSituation);
                    transaction.Commit();
                }
            });
        }

        public async Task DeleteLifeSituation(Guid lifeSituationId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                    if (lifeSituation == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                            string.Format("Жизненная ситуация [{0}] не найдена", lifeSituationId));
                    }

                    session.Delete(lifeSituation);
                    transaction.Commit();
                }
            });
        }

        public async Task<bool> LifeSituationUp(Guid lifeSituationId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                    if (lifeSituation == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                            string.Format("Услуга [{0}] не найдена", lifeSituationId));
                    }

                    var criteria = session.CreateCriteria<LifeSituation>()
                        .Add(Restrictions.Lt("SortId", lifeSituation.SortId))
                        .AddOrder(Order.Desc("SortId"))
                        .SetMaxResults(1);

                    if (lifeSituation.LifeSituationGroup != null)
                    {
                        criteria.Add(Restrictions.Eq("LifeSituationGroup", lifeSituation.LifeSituationGroup));
                    }
                    else
                    {
                        criteria.Add(Restrictions.IsNull("LifeSituationGroup"));
                    }

                    var prev = criteria.UniqueResult<LifeSituation>();
                    if (prev == null)
                    {
                        return false;
                    }

                    long sortId = lifeSituation.SortId;

                    lifeSituation.SortId = prev.SortId;
                    session.Save(lifeSituation);

                    prev.SortId = sortId;

                    session.Save(prev);
                    transaction.Commit();

                    return true;
                }
            });
        }

        public async Task<bool> LifeSituationDown(Guid lifeSituationId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.LifeSituations);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var lifeSituation = session.Get<LifeSituation>(lifeSituationId);
                    if (lifeSituation == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(lifeSituationId),
                            string.Format("Услуга [{0}] не найдена", lifeSituationId));
                    }

                    var criteria = session.CreateCriteria<LifeSituation>()
                        .Add(Restrictions.Gt("SortId", lifeSituation.SortId))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1);

                    if (lifeSituation.LifeSituationGroup != null)
                    {
                        criteria.Add(Restrictions.Eq("LifeSituationGroup", lifeSituation.LifeSituationGroup));
                    }
                    else
                    {
                        criteria.Add(Restrictions.IsNull("LifeSituationGroup"));
                    }

                    var next = criteria.UniqueResult<LifeSituation>();
                    if (next == null)
                    {
                        return false;
                    }

                    long sortId = lifeSituation.SortId;

                    lifeSituation.SortId = next.SortId;
                    session.Save(lifeSituation);

                    next.SortId = sortId;

                    session.Save(next);
                    transaction.Commit();

                    return true;
                }
            });
        }
    }
}