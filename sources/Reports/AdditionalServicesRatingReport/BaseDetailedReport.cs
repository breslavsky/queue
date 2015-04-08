using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;
using NPOI.HSSF.UserModel;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using ClientRequestAdditionalServiceQuery = NHibernate.IQueryOver<Queue.Model.ClientRequestAdditionalService, Queue.Model.ClientRequestAdditionalService>;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public abstract class BaseDetailedReport
    {
        protected AdditionalServicesRatingReportSettings settings;

        protected ISessionProvider SessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public BaseDetailedReport(AdditionalServicesRatingReportSettings settings)
        {
            this.settings = settings;
        }

        public HSSFWorkbook Generate()
        {
            DateTime startDate = GetStartDate();
            DateTime finishDate = GetFinishDate();

            if (startDate > finishDate)
            {
                throw new FaultException("Начальная дата не может быть больше чем конечная");
            }

            using (ISession session = SessionProvider.OpenSession())
            {
                ClientRequestAdditionalService service = null;
                ClientRequest request = null;
                AdditionalService additionalService = null;

                ClientRequestAdditionalServiceQuery query = session.QueryOver<ClientRequestAdditionalService>(() => service)
                                                                    .JoinAlias(() => service.ClientRequest, () => request)
                                                                    .JoinAlias(() => service.AdditionalService, () => additionalService)
                                                                    .Where(Restrictions.On(() => request.RequestDate).IsBetween(startDate).And(finishDate));

                if (settings.Services.Length > 0)
                {
                    query.Where(Restrictions.On(() => additionalService.Id).IsIn(settings.Services));
                }

                IList<AdditionalServiceStatisticsDTO> results = query
                                                               .SelectList(CreateProjections)
                                                               .TransformUsing(Transformers.AliasToBean<AdditionalServiceStatisticsDTO>())
                                                               .List<AdditionalServiceStatisticsDTO>();

                int y = 9;
            }
            return null;
        }

        private QueryOverProjectionBuilder<ClientRequestAdditionalService> CreateProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder)
        {
            AdditionalServiceStatisticsDTO dto = null;
            ClientRequest request = null;

            return builder.SelectGroup(m => m.AdditionalService).WithAlias(() => dto.Service)
                            .SelectCount(m => m.Id).WithAlias(() => dto.Count)
                            .Select(
                                Projections.GroupProperty(
                                    Projections.SqlFunction("YEAR", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))
                               )
                            .WithAlias(() => dto.Year));
        }

        protected abstract DateTime GetStartDate();

        protected abstract DateTime GetFinishDate();
    }
}