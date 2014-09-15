﻿using AutoMapper;
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
        public async Task<IDictionary<Guid, string>> GetWorkplacesList()
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplaces = new Dictionary<Guid, string>();

                    foreach (var workplace in session.CreateCriteria<Workplace>()
                        .AddOrder(Order.Asc("Type"))
                        .AddOrder(Order.Asc("Number"))
                        .AddOrder(Order.Asc("Modificator"))
                        .List<Workplace>())
                    {
                        workplaces[workplace.Id] = workplace.ToString();
                    }

                    return workplaces;
                }
            });
        }

        public async Task<DTO.Workplace[]> GetWorkplaces()
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
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
                using (var session = sessionProvider.OpenSession())
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

        public async Task<DTO.Workplace> AddWorkplace()
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplace = new Workplace();

                    session.Save(workplace);
                    transaction.Commit();

                    return Mapper.Map<Workplace, DTO.Workplace>(workplace);
                }
            });
        }

        public async Task<DTO.Workplace> EditWorkplace(DTO.Workplace source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplaceId = source.Id;

                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                    }

                    workplace.Type = source.Type;
                    workplace.Number = source.Number;
                    workplace.Modificator = source.Modificator;
                    workplace.Comment = source.Comment;
                    workplace.Display = source.Display;
                    workplace.Segments = source.Segments;

                    var errors = workplace.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(workplace);
                    transaction.Commit();

                    return Mapper.Map<Workplace, DTO.Workplace>(workplace);
                }
            });
        }

        public async Task DeleteWorkplace(Guid workplaceId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
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
                using (var session = sessionProvider.OpenSession())
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