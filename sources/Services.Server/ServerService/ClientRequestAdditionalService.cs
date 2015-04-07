using AutoMapper;
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
        public async Task<DTO.ClientRequestAdditionalService[]> GetClientRequestAdditionalServices(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    var additionalServices = session.CreateCriteria<ClientRequestAdditionalService>()
                        .Add(Restrictions.Eq("ClientRequest", clientRequest))
                        .List<ClientRequestAdditionalService>();

                    return Mapper.Map<IList<ClientRequestAdditionalService>, DTO.ClientRequestAdditionalService[]>(additionalServices);
                }
            });
        }

        public async Task<DTO.ClientRequestAdditionalService> GetClientRequestAdditionalService(Guid clientRequestAdditionalServiceId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequestAdditionalService = session.Get<ClientRequestAdditionalService>(clientRequestAdditionalServiceId);
                    if (clientRequestAdditionalService == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestAdditionalServiceId),
                            string.Format("Дополнительная услуга запроса клиента [{0}] не найдена", clientRequestAdditionalServiceId));
                    }

                    return Mapper.Map<ClientRequestAdditionalService, DTO.ClientRequestAdditionalService>(clientRequestAdditionalService);
                }
            });
        }

        public async Task<DTO.ClientRequestAdditionalService> EditClientRequestAdditionalService(DTO.ClientRequestAdditionalService source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ClientRequestAdditionalService clientRequestAdditionalService;

                    if (!source.Empty())
                    {
                        var clientRequestAdditionalServiceId = source.Id;
                        clientRequestAdditionalService = session.Get<ClientRequestAdditionalService>(clientRequestAdditionalServiceId);
                        if (clientRequestAdditionalService == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestAdditionalServiceId),
                                string.Format("Дополнительная услуга запроса клиента [{0}] не найдена", clientRequestAdditionalServiceId));
                        }
                    }
                    else
                    {
                        clientRequestAdditionalService = new ClientRequestAdditionalService();
                    }

                    if (source.AdditionalService != null)
                    {
                        Guid additionalServiceId = source.AdditionalService.Id;

                        AdditionalService additionalService = session.Get<AdditionalService>(additionalServiceId);
                        if (additionalService == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(additionalServiceId),
                                string.Format("Дополнительная услуга [{0}] не найдена", additionalServiceId));
                        }

                        clientRequestAdditionalService.AdditionalService = additionalService;
                    }
                    else
                    {
                        clientRequestAdditionalService.AdditionalService = null;
                    }

                    clientRequestAdditionalService.Quantity = source.Quantity;

                    var errors = clientRequestAdditionalService.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(clientRequestAdditionalService);
                    transaction.Commit();

                    return Mapper.Map<ClientRequestAdditionalService, DTO.ClientRequestAdditionalService>(clientRequestAdditionalService);
                }
            });
        }

        public async Task DeleteClientRequestAdditionalService(Guid clientRequestAdditionalServiceId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequestAdditionalService = session.Get<AdditionalService>(clientRequestAdditionalServiceId);
                    if (clientRequestAdditionalService == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestAdditionalServiceId),
                            string.Format("Дополнительная услуга запроса клиента [{0}] не найдена", clientRequestAdditionalServiceId));
                    }

                    session.Delete(clientRequestAdditionalService);
                    transaction.Commit();
                }
            });
        }
    }
}