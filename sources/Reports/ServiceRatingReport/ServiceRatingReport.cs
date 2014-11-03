﻿using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;
using System.ServiceModel;

namespace Queue.Reports.ServiceRatingReport
{
    public class ServiceRatingReport : BaseReport
    {
        private readonly Guid[] servicesIds;
        private readonly ServiceRatingReportSettings settings;
        private readonly ReportDetailLevel detailLavel;

        public ServiceRatingReport(Guid[] services, ReportDetailLevel detailLevel, ServiceRatingReportSettings settings)
        {
            this.servicesIds = services;
            this.detailLavel = detailLevel;
            this.settings = settings;
        }

        public override HSSFWorkbook Generate()
        {
            switch (detailLavel)
            {
                case ReportDetailLevel.Year:
                    return new YearDetailedReport(servicesIds, settings).Generate();

                case ReportDetailLevel.Month:
                    return new MonthDetailedReport(servicesIds, settings).Generate();

                case ReportDetailLevel.Day:
                    return new DayDetailedReport(servicesIds, settings).Generate();

                default:
                    throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", detailLavel.ToString()));
            }
        }
    }
}