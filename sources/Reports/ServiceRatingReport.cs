using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Reports
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
            using (ISession session = SessionProvider.OpenSession())
            {
                HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ServiceRating));
                ISheet worksheet = workbook.GetSheetAt(0);

                IDataFormat format = workbook.CreateDataFormat();
                ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

                IRow row;
                ICell cell;
                int rowIndex = worksheet.LastRowNum + 1;

                DateTime startDate, finishDate;

                ProjectionList projections = GetProjections();

                Action<IRow, ServiceRating> renderRating = (r, rating) =>
                {
                    cell = r.CreateCell(5);
                    cell.SetCellValue(rating.Total);
                    cell = r.CreateCell(6);
                    cell.SetCellValue(rating.Live);
                    cell = r.CreateCell(7);
                    cell.SetCellValue(rating.Early);
                    cell = r.CreateCell(8);
                    cell.SetCellValue(rating.Waiting);
                    cell = r.CreateCell(9);
                    cell.SetCellValue(rating.Absence);
                    cell = r.CreateCell(10);
                    cell.SetCellValue(rating.Rendered);
                    cell = r.CreateCell(11);
                    cell.SetCellValue(rating.Canceled);
                    cell = r.CreateCell(12);
                    cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.RenderTime.TotalMinutes / rating.Rendered) : 0);
                    cell = r.CreateCell(13);
                    cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.WaitingTime.TotalMinutes / rating.Rendered) : 0);
                    cell = r.CreateCell(14);
                    cell.SetCellValue(rating.SubjectsTotal);
                    cell = r.CreateCell(15);
                    cell.SetCellValue(rating.SubjectsLive);
                    cell = r.CreateCell(16);
                    cell.SetCellValue(rating.SubjectsEarly);
                };

                Conjunction conjunction = Expression.Conjunction();
                if (servicesIds.Length > 0)
                {
                    conjunction.Add(Expression.In("Service.Id", servicesIds));
                }

                switch (detailLavel)
                {
                    case ServiceRatingReportDetailLavel.Year:

                        startDate = DateTimeUtils.BeginOfYear(settings.StartYear);
                        finishDate = DateTimeUtils.EndOfYear(settings.FinishYear);

                        if (startDate > finishDate)
                        {
                            throw new FaultException("Начальная дата не может быть больше чем конечная");
                        }

                        conjunction.Add(Expression.Ge("RequestDate", startDate));
                        conjunction.Add(Expression.Le("RequestDate", finishDate));

                        #region filldata

                        Dictionary<int, List<ServiceYearRating>> yearsData = new Dictionary<int, List<ServiceYearRating>>();

                        projections
                            .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year");

                        IList<ServiceYearRating> yearRows = session.CreateCriteria<ClientRequest>()
                            .Add(conjunction)
                            .SetProjection(projections)
                            .SetResultTransformer(Transformers.AliasToBean(typeof(ServiceYearRating)))
                            .List<ServiceYearRating>();

                        if (yearRows.Count == 0)
                        {
                            throw new FaultException("Отчет является пустым");
                        }

                        foreach (var year in yearRows
                            .OrderBy(y => y.Year)
                            .GroupBy(r => r.Year))
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

                        #endregion filldata

                        #region create header

                        row = worksheet.GetRow(0);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(string.Format("Период с {0} по {1}", startDate.ToShortDateString(), finishDate.ToShortDateString()));
                        cell.CellStyle = boldCellStyle;

                        worksheet.SetColumnHidden(1, true);
                        worksheet.SetColumnHidden(2, true);
                        worksheet.SetColumnHidden(3, true);

                        #endregion create header

                        #region render data

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

                        #endregion render data

                        break;

                    case ServiceRatingReportDetailLavel.Month:

                        startDate = DateTimeUtils.BeginOfMonth(settings.StartYear, settings.StartMonth);
                        finishDate = DateTimeUtils.EndOfMonth(settings.FinishYear, settings.FinishMonth);

                        if (startDate > finishDate)
                        {
                            throw new FaultException("Начальная дата не может быть больше чем конечная");
                        }

                        conjunction.Add(Expression.Ge("RequestDate", startDate.Date));
                        conjunction.Add(Expression.Le("RequestDate", finishDate.Date));

                        var monthsData = new Dictionary<int, Dictionary<int, List<ServiceMonthRating>>>();

                        projections
                            .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year")
                            .Add(Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Month");

                        #region fill data

                        IList<ServiceMonthRating> monthRows = session.CreateCriteria<ClientRequest>()
                            .Add(conjunction)
                            .SetProjection(projections)
                            .SetResultTransformer(Transformers.AliasToBean(typeof(ServiceMonthRating)))
                            .List<ServiceMonthRating>();

                        if (monthRows.Count == 0)
                        {
                            throw new FaultException("Отчет является пустым");
                        }

                        foreach (var year in monthRows.OrderBy(r => r.Year).GroupBy(r => r.Year))
                        {
                            if (!monthsData.ContainsKey(year.Key))
                            {
                                monthsData[year.Key] = new Dictionary<int, List<ServiceMonthRating>>();
                            }

                            var months = monthsData[year.Key];

                            foreach (var month in year.OrderBy(r => r.Month).GroupBy(r => r.Month))
                            {
                                if (!months.ContainsKey(month.Key))
                                {
                                    months[month.Key] = new List<ServiceMonthRating>();
                                }

                                var ratings = months[month.Key];
                                foreach (var rating in month)
                                {
                                    ratings.Add(rating);
                                }
                            }
                        }

                        #endregion fill data

                        #region create header

                        row = worksheet.CreateRow(0);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(string.Format("Период с {0} по {1}", startDate.ToShortDateString(), finishDate.ToShortDateString()));
                        cell.CellStyle = boldCellStyle;

                        worksheet.SetColumnHidden(2, true);
                        worksheet.SetColumnHidden(3, true);

                        #endregion create header

                        #region render data

                        foreach (var year in monthsData)
                        {
                            row = worksheet.CreateRow(rowIndex++);
                            cell = row.CreateCell(0);
                            cell.SetCellValue(year.Key);
                            cell.CellStyle = boldCellStyle;

                            foreach (var month in year.Value)
                            {
                                var ratings = month.Value;

                                row = worksheet.CreateRow(rowIndex++);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Key));
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
                                        cell = row.CreateCell(1);
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
                                                    if (service.Type.HasFlag(serviceType))
                                                    {
                                                        row = worksheet.CreateRow(rowIndex++);
                                                        cell = row.CreateCell(4);
                                                        cell.SetCellValue(translation.GetString(serviceType.ToString()));

                                                        var rating = ratings.FirstOrDefault(r => r.Service.Equals(service)
                                                            && r.ServiceType.Equals(serviceType));
                                                        renderRating(row, rating != null ? rating : new ServiceDayRating());
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var rating = ratings.FirstOrDefault(r => r.Service.Equals(service));
                                                renderRating(row, rating != null ? rating : new ServiceDayRating());
                                            }
                                        }

                                        if (!hasServices)
                                        {
                                            worksheet.RemoveRow(row);
                                            rowIndex--;
                                        }
                                    }
                                };

                                recursion(null);
                            }
                        }

                        #endregion render data

                        break;

                    case ServiceRatingReportDetailLavel.Day:

                        startDate = DateTimeUtils.BeginOfDay(settings.StartYear, settings.StartMonth, settings.StartDay);
                        finishDate = DateTimeUtils.EndOfDay(settings.FinishYear, settings.FinishMonth, settings.FinishDay);

                        if (startDate > finishDate)
                        {
                            throw new FaultException("Начальная дата не может быть больше чем конечная");
                        }

                        conjunction.Add(Expression.Ge("RequestDate", startDate));
                        conjunction.Add(Expression.Le("RequestDate", finishDate));

                        var daysData = new Dictionary<int, Dictionary<int, Dictionary<int, List<ServiceDayRating>>>>();

                        projections
                            .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year")
                            .Add(Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Month")
                            .Add(Projections.GroupProperty(Projections.SqlFunction("day", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Day");

                        #region fill data

                        IList<ServiceDayRating> dayRows = session.CreateCriteria<ClientRequest>()
                            .Add(conjunction)
                            .SetProjection(projections)
                            .SetResultTransformer(Transformers.AliasToBean(typeof(ServiceDayRating)))
                            .List<ServiceDayRating>();

                        if (dayRows.Count == 0)
                        {
                            throw new FaultException("Отчет является пустым");
                        }

                        foreach (var year in dayRows.OrderBy(r => r.Year).GroupBy(r => r.Year))
                        {
                            if (!daysData.ContainsKey(year.Key))
                            {
                                daysData[year.Key] = new Dictionary<int, Dictionary<int, List<ServiceDayRating>>>();
                            }

                            var months = daysData[year.Key];

                            foreach (var month in year.OrderBy(r => r.Month).GroupBy(r => r.Month))
                            {
                                if (!months.ContainsKey(month.Key))
                                {
                                    months[month.Key] = new Dictionary<int, List<ServiceDayRating>>();
                                }

                                var days = months[month.Key];

                                foreach (var day in month.OrderBy(r => r.Day).GroupBy(r => r.Day))
                                {
                                    if (!days.ContainsKey(day.Key))
                                    {
                                        days[day.Key] = new List<ServiceDayRating>();
                                    }

                                    var ratings = days[day.Key];

                                    foreach (var rating in day)
                                    {
                                        ratings.Add(rating);
                                    }
                                }
                            }
                        }

                        #endregion fill data

                        #region create header

                        row = worksheet.GetRow(0);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(string.Format("Период с {0} по {1}", startDate.ToShortDateString(), finishDate.ToShortDateString()));
                        cell.CellStyle = boldCellStyle;

                        worksheet.SetColumnHidden(3, true);

                        #endregion create header

                        #region render data

                        foreach (var year in daysData)
                        {
                            row = worksheet.CreateRow(rowIndex++);
                            cell = row.CreateCell(0);
                            cell.SetCellValue(year.Key);
                            cell.CellStyle = boldCellStyle;

                            foreach (var month in year.Value)
                            {
                                row = worksheet.CreateRow(rowIndex++);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Key));
                                cell.CellStyle = boldCellStyle;

                                foreach (var day in month.Value)
                                {
                                    var ratings = day.Value;

                                    row = worksheet.CreateRow(rowIndex++);
                                    cell = row.CreateCell(2);
                                    cell.SetCellValue(day.Key);
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
                                            var lastRowIndex = rowIndex++;

                                            row = worksheet.CreateRow(lastRowIndex);
                                            cell = row.CreateCell(2);
                                            cell.SetCellValue(g.ToString());
                                            cell.CellStyle = boldCellStyle;

                                            recursion(g);

                                            var services = session.CreateCriteria<Service>()
                                                .Add(Restrictions.Eq("ServiceGroup", g))
                                                .AddOrder(Order.Asc("SortId"))
                                                .List<Service>();

                                            foreach (var service in services)
                                            {
                                                if (servicesIds.Count() > 0 && !servicesIds.Any(i => i.Equals(service.Id)))
                                                {
                                                    continue;
                                                }

                                                row = worksheet.CreateRow(rowIndex++);
                                                cell = row.CreateCell(4);
                                                cell.SetCellValue(service.ToString());

                                                if (service.Type != ServiceType.None && settings.IsServiceTypes)
                                                {
                                                    cell.CellStyle = boldCellStyle;

                                                    var translation = Translation.ServiceType.ResourceManager;

                                                    foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
                                                    {
                                                        if (service.Type.HasFlag(serviceType))
                                                        {
                                                            row = worksheet.CreateRow(rowIndex++);
                                                            cell = row.CreateCell(4);
                                                            cell.SetCellValue(translation.GetString(serviceType.ToString()));

                                                            var rating = ratings.FirstOrDefault(r => r.Service.Equals(service)
                                                                && r.ServiceType.Equals(serviceType));
                                                            renderRating(row, rating != null ? rating : new ServiceDayRating());
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var rating = ratings.FirstOrDefault(r => r.Service.Equals(service));
                                                    renderRating(row, rating != null ? rating : new ServiceDayRating());
                                                }
                                            }

                                            if (rowIndex <= lastRowIndex)
                                            {
                                                worksheet.RemoveRow(row);
                                                rowIndex--;
                                            }
                                        }
                                    };

                                    recursion(null);
                                }
                            }
                        }

                        #endregion render data

                        break;

                    case ServiceRatingReportDetailLavel.Hour:
                        break;
                }

                return workbook;
            }
        }

        private ProjectionList GetProjections()
        {
            ProjectionList projections = Projections.ProjectionList()
                       .Add(Projections.GroupProperty("Service"))
                       .Add(Projections.Property("Service"), "Service")

                       .Add(Projections.RowCount(), "Total")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Live")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Early")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Waiting),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Waiting")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Absence),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Absence")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Rendered")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Canceled),
                            Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Canceled")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                            Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) / Subjects as RenderTime", new string[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                            Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                            Projections.SqlProjection("({alias}.RenderStartTime - {alias}.WaitingStartTime) as WaitingTime", new string[] { "WaitingTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                            Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "WaitingTime")

                       .Add(Projections.Sum("Subjects"), "SubjectsTotal")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                            Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsLive")

                       .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                            Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsEarly");

            if (settings.IsServiceTypes)
            {
                projections
                    .Add(Projections.GroupProperty("ServiceType"))
                    .Add(Projections.Property("ServiceType"), "ServiceType");
            }

            return projections;
        }

        private class ServiceRating
        {
            public Service Service { get; set; }

            public ServiceType ServiceType { get; set; }

            public int Total { get; set; }

            public int Live { get; set; }

            public int Early { get; set; }

            public int Waiting { get; set; }

            public int Absence { get; set; }

            public int Rendered { get; set; }

            public int Canceled { get; set; }

            public TimeSpan RenderTime { get; set; }

            public TimeSpan WaitingTime { get; set; }

            public int SubjectsTotal { get; set; }

            public int SubjectsLive { get; set; }

            public int SubjectsEarly { get; set; }
        };

        private class ServiceYearRating : ServiceRating
        {
            public int Year { get; set; }
        };

        private class ServiceMonthRating : ServiceYearRating
        {
            public int Month { get; set; }
        };

        private class ServiceDayRating : ServiceMonthRating
        {
            public int Day { get; set; }
        };
    }
}