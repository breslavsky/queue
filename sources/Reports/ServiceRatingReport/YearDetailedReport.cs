using NHibernate;
using NHibernate.Criterion;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Reports.ServiceRatingReport
{
    internal class YearDetailedReport : BaseDetailedReport<ServiceYearRating>
    {
        public YearDetailedReport(Guid[] servicesIds, ServiceRatingReportSettings settings)
            : base(servicesIds, settings)
        {
        }

        protected override HSSFWorkbook InternalGenerate(ISession session, ServiceYearRating[] data)
        {
            Dictionary<int, List<ServiceYearRating>> yearsData = new Dictionary<int, List<ServiceYearRating>>();

            foreach (var year in data.OrderBy(y => y.Year).GroupBy(r => r.Year))
            {
                if (!yearsData.ContainsKey(year.Key))
                {
                    yearsData[year.Key] = new List<ServiceYearRating>();
                }

                var ratings = yearsData[year.Key];
                foreach (var r in year)
                {
                    ratings.Add(r);
                }
            }

            HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ServiceRating));
            ISheet worksheet = workbook.GetSheetAt(0);

            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

            IRow row;
            ICell cell;
            int rowIndex = worksheet.LastRowNum + 1;

            row = worksheet.GetRow(0);
            cell = row.CreateCell(0);
            cell.SetCellValue(string.Format("Период с {0} по {1}", GetStartDate().ToShortDateString(), GetStartDate().ToShortDateString()));
            cell.CellStyle = boldCellStyle;

            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

            foreach (var year in yearsData)
            {
                var ratings = year.Value;

                row = worksheet.CreateRow(rowIndex++);
                cell = row.CreateCell(0);
                cell.SetCellValue(year.Key);
                cell.CellStyle = boldCellStyle;

                Action<ServiceGroup> recursion = null;
                recursion = (parentGroup) =>
                {
                    var serviceGroups = session.CreateCriteria<ServiceGroup>()
                        .AddOrder(Order.Asc("SortId"))
                        .Add(parentGroup != null
                            ? Restrictions.Eq("ParentGroup", parentGroup)
                            : Restrictions.IsNull("ParentGroup"))
                        .List<ServiceGroup>();

                    foreach (var g in serviceGroups)
                    {
                        row = worksheet.CreateRow(rowIndex++);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(g.ToString());
                        cell.CellStyle = boldCellStyle;

                        recursion(g);

                        var services = session.CreateCriteria<Service>()
                            .Add(Restrictions.Eq("ServiceGroup", g))
                            .AddOrder(Order.Asc("SortId"))
                            .List<Service>();

                        var hasServices = false;

                        foreach (var service in services)
                        {
                            if (servicesIds.Count() > 0 && !servicesIds.Any(i => i.Equals(service.Id)))
                            {
                                continue;
                            }

                            hasServices = true;

                            row = worksheet.CreateRow(rowIndex++);
                            cell = row.CreateCell(4);
                            cell.SetCellValue(service.ToString());

                            if (service.Type != ServiceType.None && settings.IsServiceTypes)
                            {
                                cell.CellStyle = boldCellStyle;

                                var translation = Translation.ServiceType.ResourceManager;

                                foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
                                {
                                    row = worksheet.CreateRow(rowIndex++);
                                    cell = row.CreateCell(4);
                                    cell.SetCellValue(translation.GetString(serviceType.ToString()));

                                    var rating = ratings.FirstOrDefault(r => r.Service.Equals(service)
                                        && r.ServiceType.Equals(serviceType));
                                    renderRating(row, rating != null ? rating : new ServiceYearRating());
                                }
                            }
                            else
                            {
                                var rating = ratings.FirstOrDefault(r => r.Service.Equals(service));
                                renderRating(row, rating != null ? rating : new ServiceYearRating());
                            }
                        };

                        if (!hasServices)
                        {
                            worksheet.RemoveRow(row);
                            rowIndex--;
                        }
                    };
                };
                recursion(null);
            }

            return workbook;
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfYear(settings.StartYear);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfYear(settings.FinishYear);
        }

        protected override ProjectionList GetProjections()
        {
            return GetCommonProjections()
                    .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year");
        }
    }
}