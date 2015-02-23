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
        public async Task<DTO.AdditionalService[]> GetAdditionalServices()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.AdditionalServices);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var additionalServices = session.CreateCriteria<AdditionalService>().List<AdditionalService>();

                    return Mapper.Map<IList<AdditionalService>, DTO.AdditionalService[]>(additionalServices);
                }
            });
        }

        public async Task<DTO.AdditionalService> GetAdditionalService(Guid additionalServiceId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var additionalService = session.Get<AdditionalService>(additionalServiceId);
                    if (additionalService == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(additionalServiceId), string.Format("Дополнительная услуга [{0}] не найдена", additionalServiceId));
                    }

                    return Mapper.Map<AdditionalService, DTO.AdditionalService>(additionalService);
                }
            });
        }

        public async Task<DTO.AdditionalService> EditAdditionalService(DTO.AdditionalService source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.AdditionalServices);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    AdditionalService additionalService;

                    if (!source.Empty())
                    {
                        var additionalServiceId = source.Id;
                        additionalService = session.Get<AdditionalService>(additionalServiceId);
                        if (additionalService == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(additionalServiceId), string.Format("Дополнительная услуга [{0}] не найдена", additionalServiceId));
                        }
                    }
                    else
                    {
                        additionalService = new AdditionalService();
                    }

                    additionalService.Name = source.Name;
                    additionalService.Price = source.Price;
                    additionalService.Measure = source.Measure;

                    var errors = additionalService.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(additionalService);
                    transaction.Commit();

                    return Mapper.Map<AdditionalService, DTO.AdditionalService>(additionalService);
                }
            });
        }

        public async Task DeleteAdditionalService(Guid additionalServiceId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.AdditionalServices);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var additionalService = session.Get<AdditionalService>(additionalServiceId);
                    if (additionalService == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(additionalServiceId), string.Format("Дополнительная услуга [{0}] не найдена", additionalServiceId));
                    }

                    session.Delete(additionalService);
                    transaction.Commit();
                }
            });
        }
    }
}