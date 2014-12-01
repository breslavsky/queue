using AutoMapper;
using Junte.Data.NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.IdentifiedEntity> GetEntity(DTO.EntityLink link)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Guid id = link.Id;

                    Dictionary<Type, Type> dic = new Dictionary<Type, Type>();
                    dic.Add(typeof(DTO.UserLink), typeof(User));

                    IdentifiedEntity entity = session.Get(link.GetType(), id) as IdentifiedEntity;
                    if (entity == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(id), string.Format("Сущность [{0}] не найден", id));
                    }

                    return Mapper.Map<IdentifiedEntity, DTO.IdentifiedEntity>(entity);
                }
            });
        }
    }
}