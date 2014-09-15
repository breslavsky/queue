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
    partial class ServerService
    {
        public async Task<DTO.ServiceParameter[]> GetServiceParameters(Guid serviceId)
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

                    var serviceParameters = session.CreateCriteria<ServiceParameter>()
                        .Add(Restrictions.Eq("Service", service))
                        .List<ServiceParameter>();

                    return Mapper.Map<IList<ServiceParameter>, DTO.ServiceParameter[]>(serviceParameters);
                }
            });
        }

        public async Task<DTO.ServiceParameter> AddServiceParameter(Guid serviceId, ServiceParameterType type)
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

                    ServiceParameter parameter;

                    switch (type)
                    {
                        case ServiceParameterType.Number:
                            parameter = new ServiceParameterNumber();
                            break;

                        case ServiceParameterType.Text:
                            parameter = new ServiceParameterText();
                            break;

                        case ServiceParameterType.Options:
                            parameter = new ServiceParameterOptions();
                            break;

                        default:
                            throw new SystemException();
                    }

                    parameter.Service = service;

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        logger.Error(ValidationError.ToException(errors));
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameter, DTO.ServiceParameter>(parameter);
                }
            });
        }

        public async Task<DTO.ServiceParameter> EditNumberServiceParameter(DTO.ServiceParameterNumber source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    var parameter = session.Get<ServiceParameterNumber>(parameterId);
                    if (parameter == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId), string.Format("Параметр услуги [{0}] не найден", parameterId));
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameter, DTO.ServiceParameter>(parameter);
                }
            });
        }

        public async Task<DTO.ServiceParameter> EditTextServiceParameter(DTO.ServiceParameterText source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    var parameter = session.Get<ServiceParameterText>(parameterId);
                    if (parameter == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId), string.Format("Параметр услуги [{0}] не найден", parameterId));
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;
                    parameter.MinLength = source.MinLength;
                    parameter.MaxLength = source.MaxLength;

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw ValidationError.ToException(errors);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameter, DTO.ServiceParameter>(parameter);
                }
            });
        }

        public async Task<DTO.ServiceParameter> EditOptionsServiceParameter(DTO.ServiceParameterOptions source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    var parameter = session.Get<ServiceParameterOptions>(parameterId);
                    if (parameter == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId), string.Format("Параметр услуги [{0}] не найден", parameterId));
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;
                    parameter.Options = source.Options;
                    parameter.IsMultiple = source.IsMultiple;

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw ValidationError.ToException(errors);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameter, DTO.ServiceParameter>(parameter);
                }
            });
        }

        public async Task DeleteServiceParameter(Guid serviceParameterId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ServiceParameter parameter = session.Get<ServiceParameter>(serviceParameterId);
                    if (parameter == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceParameterId), string.Format("Параметр услуги [{0}] не найден", serviceParameterId));
                    }

                    session.Delete(parameter);
                    transaction.Commit();
                }
            });
        }
    }
}