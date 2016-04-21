using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public abstract class BaseDetailedReport<T> : BaseReport
        where T : AdditionalServiceRating
    {
        protected const int StartOperatorsStatisticsCol = 6;

        private Operator[] operators;
        private AdditionalService[] additionalServices;
        protected AdditionalServicesRatingReportSettings settings;
        protected ICellStyle sumCellStyle;
        protected ICellStyle countCellStyle;

        protected override int ColumnCount { get { return 5; } }

        protected BaseDetailedReport(AdditionalServicesRatingReportSettings settings)
        {
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this.GetType(), this);

            this.settings = settings;
        }

        protected override HSSFWorkbook InternalGenerate()
        {
            var startDate = GetStartDate();
            var finishDate = GetFinishDate();

            if (startDate > finishDate)
            {
                throw new FaultException("Начальная дата не может быть больше чем конечная");
            }

            using (var session = SessionProvider.OpenSession())
            {
                ClientRequestAdditionalService service = null;
                ClientRequest request = null;
                AdditionalService additionalService = null;

                var query = session.QueryOver(() => service)
                                .JoinAlias(() => service.ClientRequest, () => request)
                                .JoinAlias(() => service.AdditionalService, () => additionalService)
                                .Where(Restrictions.On(() => request.RequestDate).IsBetween(startDate).And(finishDate))
                                .Where(Restrictions.On(() => request.State).IsIn(new[] { ClientRequestState.Rendered }));

                if (settings.Services.Length > 0)
                {
                    query.Where(Restrictions.On(() => additionalService.Id).IsIn(settings.Services));
                }

                var results = query.SelectList(CreateProjections)
                                          .TransformUsing(Transformers.AliasToBean<T>())
                                          .Where(Restrictions.Gt(Projections.Count<T>(f => f.Quantity), 0))
                                          .List<T>();

                if (results.Count == 0)
                {
                    throw new FaultException("Пустой отчет");
                }

                var workbook = new HSSFWorkbook(new MemoryStream(Templates.AdditionalServiceRating));
                var worksheet = workbook.GetSheetAt(0);
                worksheet.CreateFreezePane(6, 2);

                CreateCellStyles(worksheet);

                WriteCell(worksheet.GetRow(0), 0, c => c.SetCellValue(GetTitle()), styles[StandardCellStyles.BoldStyle]);

                WriteOperatorsHeader(worksheet, session);
                RenderData(session, worksheet, results);

                return workbook;
            }
        }

        private string GetTitle()
        {
            return String.Format("Период с {0} по {1}", GetStartDate().ToShortDateString(), GetFinishDate().ToShortDateString());
        }

        private void CreateCellStyles(ISheet worksheet)
        {
            styles = new StandardCellStyles(worksheet.Workbook);
            countCellStyle = CreateCountCellStyle(worksheet);
            sumCellStyle = CreateSumCellStyle(worksheet);
        }

        private ICellStyle CreateSumCellStyle(ISheet worksheet)
        {
            var style = worksheet.Workbook.CreateCellStyle();
            style.DataFormat = worksheet.GetRow(2).GetCell(5).CellStyle.DataFormat;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            return style;
        }

        private ICellStyle CreateCountCellStyle(ISheet worksheet)
        {
            var style = worksheet.Workbook.CreateCellStyle();
            style.DataFormat = worksheet.GetRow(2).GetCell(4).CellStyle.DataFormat;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            return style;
        }

        private QueryOverProjectionBuilder<ClientRequestAdditionalService> CreateProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder)
        {
            ModifyProjections(builder);

            T dto = null;
            return builder.SelectGroup(m => m.AdditionalService).WithAlias(() => dto.Service)
                            .SelectGroup(m => m.Operator).WithAlias(() => dto.Operator)
                            .SelectSum(m => m.Quantity).WithAlias(() => dto.Quantity);
        }

        protected abstract DateTime GetStartDate();

        protected abstract DateTime GetFinishDate();

        protected abstract void ModifyProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder);

        protected abstract void RenderData(ISession session, ISheet worksheet, IList<T> results);

        protected Operator[] GetOperators(ISession session)
        {
            return operators ?? (operators = session.QueryOver<Operator>()
                                                    .List()
                                                    .OrderBy(o => o.ToString())
                                                    .ToArray());
        }

        protected AdditionalService[] GetAdditionalServices(ISession session)
        {
            return additionalServices ?? (additionalServices = InternalGetAdditionalServices(session));
        }

        private AdditionalService[] InternalGetAdditionalServices(ISession session)
        {
            var query = session.QueryOver<AdditionalService>();
            if (settings.Services.Length > 0)
            {
                query.Where(s => s.Id.IsIn(settings.Services));
            }

            return query.List()
                        .OrderBy(o => o.ToString())
                         .ToArray();
        }

        private void WriteOperatorsHeader(ISheet worksheet, ISession session)
        {
            var nameRow = worksheet.GetRow(0);
            var statRow = worksheet.GetRow(1);
            var formulaRow = worksheet.GetRow(2);
            var statStyle = statRow.GetCell(4).CellStyle;

            var font = worksheet.Workbook.CreateFont();
            font.Boldweight = 1000;

            int col = StartOperatorsStatisticsCol;
            foreach (var oper in GetOperators(session))
            {
                var cell = nameRow.CreateCell(col);
                cell.SetCellValue(oper.ToString());
                cell.CellStyle.SetFont(font);
                cell.CellStyle.Alignment = HorizontalAlignment.Center;
                worksheet.AddMergedRegion(new CellRangeAddress(0, 0, col, col + 1));

                cell = statRow.CreateCell(col);
                cell.SetCellValue("Количество");
                cell.CellStyle = statStyle;

                cell = formulaRow.CreateCell(col);
                cell.CellStyle = formulaRow.GetCell(4).CellStyle;
                cell.CellFormula = string.Format("SUM({0}4:{0}10000)", ReportsUtils.ColumnName(col + 1));

                cell = statRow.CreateCell(col + 1);
                cell.SetCellValue("Сумма, руб");
                cell.CellStyle = statStyle;

                cell = formulaRow.CreateCell(col + 1);
                cell.CellStyle = formulaRow.GetCell(5).CellStyle;
                cell.CellFormula = string.Format("SUM({0}4:{0}10000)", ReportsUtils.ColumnName(col + 2));

                col += 2;
            }
        }

        protected void RenderServiceRating(IRow row, AdditionalService service, AdditionalServiceRating[] ratings)
        {
            float totalCount = 0;
            int col = StartOperatorsStatisticsCol;

            foreach (var oper in operators)
            {
                var rating = ratings.SingleOrDefault(r => r.Operator.Equals(oper) && r.Service.Equals(service));
                if (rating != null)
                {
                    totalCount += rating.Quantity;
                }

                var cntCol = row.CreateCell(col);
                cntCol.CellStyle = countCellStyle;

                var sumCol = row.CreateCell(col + 1);
                sumCol.CellStyle = sumCellStyle;

                if (rating != null)
                {
                    cntCol.SetCellValue(rating.Quantity);
                    sumCol.SetCellValue(rating.Quantity * (double)service.Price);
                }

                col += 2;
            }

            WriteCell(row, 4, c => c.SetCellValue(totalCount), countCellStyle);
            WriteCell(row, 5, c => c.SetCellValue(totalCount * (double)service.Price), sumCellStyle);
        }
    }
}