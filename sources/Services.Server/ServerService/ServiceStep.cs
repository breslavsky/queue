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
        public async Task<DTO.IdentifiedEntityLink[]> GetServiceStepLinks(Guid serviceId)
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
                        .List<IdentifiedEntity>();
                    return Mapper.Map<IList<IdentifiedEntity>, DTO.IdentifiedEntityLink[]>(serviceSteps);
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
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                            string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    var steps = session.CreateCriteria<ServiceStep>()
                        .Add(Restrictions.Eq("Service", service))
                        .AddOrder(Order.Asc("SortId"))
                        .List<ServiceStep>();
                    return Mapper.Map<IList<ServiceStep>, DTO.ServiceStep[]>(steps);
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

        public async Task<DTO.ServiceStep> EditServiceStep(DTO.ServiceStep source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceStep serviceStep;

                    if (!source.Empty())
                    {
                        var serviceStepId = source.Id;
                        serviceStep = session.Get<ServiceStep>(serviceStepId);
                        if (serviceStep == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId),
                                string.Format("Этап услуги [{0}] не найден", serviceStepId));
                        }
                    }
                    else
                    {
                        serviceStep = new ServiceStep();
                    }

                    if (source.Service != null)
                    {
                        var serviceId = source.Service.Id;

                        var service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                string.Format("Услуга [{0}] не найдена", serviceId));
                        }
                        serviceStep.Service = service;
                    }
                    else
                    {
                        serviceStep.Service = null;
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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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
                        .Add(Expression.Gt("SortId", serviceStep.SortId))
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