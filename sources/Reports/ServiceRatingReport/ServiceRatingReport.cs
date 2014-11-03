using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;
using System.ServiceModel;

namespace Queue.Reports.ServiceRatingReport
{
    public class ServiceRatingReport : BaseReport
    {
        private readonly Guid[] servicesIds;
        private readonly ServiceRatingReportSettings settings;
        private readonly ServiceRatingReportDetailLavel detailLavel;

        public ServiceRatingReport(Guid[] servicesIds, ServiceRatingReportDetailLavel detailLavel, ServiceRatingReportSettings settings)
        {
            this.servicesIds = servicesIds;
            this.detailLavel = detailLavel;
            this.settings = settings;
        }

        public override HSSFWorkbook Generate()
        {
            switch (detailLavel)
            {
                case ServiceRatingReportDetailLavel.Year:
                    return new YearDetailedReport(servicesIds, settings).Generate();

                case ServiceRatingReportDetailLavel.Month:
                    return new MonthDetailedReport(servicesIds, settings).Generate();

                case ServiceRatingReportDetailLavel.Day:

                    return new DayDetailedReport(servicesIds, settings).Generate();

                default:
                    throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", detailLavel.ToString()));
            }
        }
    }
}