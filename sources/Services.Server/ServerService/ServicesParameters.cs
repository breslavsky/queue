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
        public async Task<DTO.ServiceParameter> GetServiceParameter(Guid serviceParameterId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var serviceParameter = session.Get<ServiceParameter>(serviceParameterId);
                    if (serviceParameter == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceParameterId),
                            string.Format("Параметр услуги [{0}] не найден", serviceParameterId));
                    }

                    return Mapper.Map<ServiceParameter, DTO.ServiceParameter>(serviceParameter);
                }
            });
        }

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

        public async Task<DTO.ServiceParameterNumber> EditServiceParameterNumber(DTO.ServiceParameterNumber source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    ServiceParameterNumber parameter;

                    if (!source.Empty())
                    {
                        parameter = session.Get<ServiceParameterNumber>(parameterId);
                        if (parameter == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId),
                                string.Format("Параметр услуги [{0}] не найден", parameterId));
                        }
                    }
                    else
                    {
                        parameter = new ServiceParameterNumber();
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;

                    if (source.Service != null)
                    {
                        Guid serviceId = source.Service.Id;

                        Service service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                string.Format("Услуга [{0}] не найдена", serviceId));
                        }
                        parameter.Service = service;
                    }
                    else
                    {
                        parameter.Service = null;
                    }

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameterNumber, DTO.ServiceParameterNumber>(parameter);
                }
            });
        }

        public async Task<DTO.ServiceParameterText> EditServiceParameterText(DTO.ServiceParameterText source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    ServiceParameterText parameter;

                    if (!source.Empty())
                    {
                        parameter = session.Get<ServiceParameterText>(parameterId);
                        if (parameter == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId),
                                string.Format("Параметр услуги [{0}] не найден", parameterId));
                        }
                    }
                    else
                    {
                        parameter = new ServiceParameterText();
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;
                    parameter.MinLength = source.MinLength;
                    parameter.MaxLength = source.MaxLength;

                    if (source.Service != null)
                    {
                        Guid serviceId = source.Service.Id;

                        Service service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                string.Format("Услуга [{0}] не найдена", serviceId));
                        }
                        parameter.Service = service;
                    }
                    else
                    {
                        parameter.Service = null;
                    }

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw ValidationError.ToException(errors);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameterText, DTO.ServiceParameterText>(parameter);
                }
            });
        }

        public async Task<DTO.ServiceParameterOptions> EditServiceParameterOptions(DTO.ServiceParameterOptions source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var parameterId = source.Id;

                    ServiceParameterOptions parameter;

                    if (!source.Empty())
                    {
                        parameter = session.Get<ServiceParameterOptions>(parameterId);
                        if (parameter == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(parameterId),
                                string.Format("Параметр услуги [{0}] не найден", parameterId));
                        }
                    }
                    else
                    {
                        parameter = new ServiceParameterOptions();
                    }

                    parameter.Name = source.Name;
                    parameter.ToolTip = source.ToolTip;
                    parameter.IsRequire = source.IsRequire;
                    parameter.Options = source.Options;
                    parameter.IsMultiple = source.IsMultiple;

                    if (source.Service != null)
                    {
                        Guid serviceId = source.Service.Id;

                        Service service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                string.Format("Услуга [{0}] не найдена", serviceId));
                        }
                        parameter.Service = service;
                    }
                    else
                    {
                        parameter.Service = null;
                    }

                    var errors = parameter.Validate();
                    if (errors.Length > 0)
                    {
                        throw ValidationError.ToException(errors);
                    }

                    session.Save(parameter);
                    transaction.Commit();

                    return Mapper.Map<ServiceParameterOptions, DTO.ServiceParameterOptions>(parameter);
                }
            });
        }

        public async Task DeleteServiceParameter(Guid serviceParameterId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Services);

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