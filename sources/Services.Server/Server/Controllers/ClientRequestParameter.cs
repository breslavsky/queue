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
        public async Task<DTO.ClientRequestParameter[]> GetClientRequestParameters(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.All);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    var additionalServices = session.CreateCriteria<ClientRequestParameter>()
                        .Add(Restrictions.Eq("ClientRequest", clientRequest))
                        .List<ClientRequestParameter>();

                    return Mapper.Map<IList<ClientRequestParameter>, DTO.ClientRequestParameter[]>(additionalServices);
                }
            });
        }
    }
}