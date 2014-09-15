using AutoMapper;
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
        public async Task<DTO.Office[]> GetOffices()
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var offices = session.CreateCriteria<Office>().List<Office>();

                    return Mapper.Map<IList<Office>, DTO.Office[]>(offices);
                }
            });
        }

        public async Task<DTO.Office> GetOffice(Guid officeId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var office = session.Get<Office>(officeId);
                    if (office == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(officeId), string.Format("Филиал [{0}] не найдена", officeId));
                    }

                    return Mapper.Map<Office, DTO.Office>(office);
                }
            });
        }

        public async Task<DTO.Office> AddOffice()
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var office = new Office();

                    session.Save(office);
                    transaction.Commit();

                    return Mapper.Map<Office, DTO.Office>(office);
                }
            });
        }

        public async Task<DTO.Office> EditOffice(DTO.Office source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var officeId = source.Id;

                    var office = session.Get<Office>(officeId);
                    if (office == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(officeId), string.Format("Филиал [{0}] не найден", officeId));
                    }

                    office.Name = source.Name;
                    office.Endpoint = source.Endpoint;
                    office.SessionId = source.SessionId;

                    var errors = office.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(office);
                    transaction.Commit();

                    return Mapper.Map<Office, DTO.Office>(office);
                }
            });
        }

        public async Task DeleteOffice(Guid officeId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var office = session.Get<Office>(officeId);
                    if (office == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(officeId), string.Format("Филиал [{0}] не найдена", officeId));
                    }

                    session.Delete(office);
                    transaction.Commit();
                }
            });
        }
    }
}