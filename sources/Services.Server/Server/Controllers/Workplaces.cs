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
        public async Task<DTO.IdentifiedEntityLink[]> GetWorkplacesLinks()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplaces = session.CreateCriteria<Workplace>()
                        .AddOrder(Order.Asc("Type"))
                        .AddOrder(Order.Asc("Number"))
                        .AddOrder(Order.Asc("Modificator"))
                        .List<IdentifiedEntity>();

                    return Mapper.Map<IList<IdentifiedEntity>, DTO.IdentifiedEntityLink[]>(workplaces);
                }
            });
        }

        public async Task<DTO.Workplace[]> GetWorkplaces()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplaces = session.CreateCriteria<Workplace>()
                        .AddOrder(Order.Asc("Type"))
                        .AddOrder(Order.Asc("Number"))
                        .AddOrder(Order.Asc("Modificator"))
                        .List<Workplace>();

                    return Mapper.Map<IList<Workplace>, DTO.Workplace[]>(workplaces);
                }
            });
        }

        public async Task<DTO.Workplace> GetWorkplace(Guid workplaceId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                    }

                    return Mapper.Map<Workplace, DTO.Workplace>(workplace);
                }
            });
        }

        public async Task<DTO.Workplace> EditWorkplace(DTO.Workplace source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Workplaces);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Workplace workplace;

                    if (!source.Empty())
                    {
                        var workplaceId = source.Id;
                        workplace = session.Get<Workplace>(workplaceId);
                        if (workplace == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                        }
                    }
                    else
                    {
                        workplace = new Workplace();
                    }

                    workplace.Type = source.Type;
                    workplace.Number = source.Number;
                    workplace.Modificator = source.Modificator;
                    workplace.Comment = source.Comment;
                    workplace.DisplayDeviceId = source.DisplayDeviceId;
                    workplace.QualityPanelDeviceId = source.QualityPanelDeviceId;
                    workplace.Segments = source.Segments;

                    var errors = workplace.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(workplace);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(workplace);
                    }

                    return Mapper.Map<Workplace, DTO.Workplace>(workplace);
                }
            });
        }

        public async Task DeleteWorkplace(Guid workplaceId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Workplaces);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                    }

                    foreach (var o in session.CreateCriteria<Operator>()
                        .Add(Restrictions.Eq("Workplace", workplace))
                        .List<Operator>())
                    {
                        session.SessionFactory.EvictEntity("Operator", o.Id);
                    }

                    session.Delete(workplace);
                    transaction.Commit();
                }
            });
        }

        public async Task<DTO.Operator[]> GetWorkplaceOperators(Guid workplaceId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                    }

                    var operators = session.CreateCriteria<Operator>()
                        .Add(Restrictions.Eq("Workplace", workplace))
                        .List<Operator>();

                    return Mapper.Map<IList<Operator>, DTO.Operator[]>(operators);
                }
            });
        }
    }
}