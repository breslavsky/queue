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
using ClientRequestAdditionalServiceQuery = NHibernate.IQueryOver<Queue.Model.ClientRequestAdditionalService, Queue.Model.ClientRequestAdditionalService>;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public abstract class BaseDetailedReport<T> where T : AdditionalServiceRating
    {
        #region dependency

        [Dependency]
        public SessionProvider SessionProvider { get; set; }

        #endregion dependency

        protected const int StartOperatorsStatisticsCol = 6;

        private Operator[] operators;
        private AdditionalService[] additionalServices;
        protected AdditionalServicesRatingReportSettings settings;
        protected ICellStyle boldCellStyle;
        protected ICellStyle sumCellStyle;
        protected ICellStyle countCellStyle;

        protected BaseDetailedReport(AdditionalServicesRatingReportSettings settings)
        {
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this.GetType(), this);

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

                ClientRequestAdditionalServiceQuery query = session.QueryOver(() => service)
                                                                    .JoinAlias(() => service.ClientRequest, () => request)
                                                                    .JoinAlias(() => service.AdditionalService, () => additionalService)
                                                                    .Where(Restrictions.On(() => request.RequestDate).IsBetween(startDate).And(finishDate))
                                                                    .Where(Restrictions.On(() => request.State).IsIn(new[] { ClientRequestState.Rendered }));

                if (settings.Services.Length > 0)
                {
                    query.Where(Restrictions.On(() => additionalService.Id).IsIn(settings.Services));
                }

                IList<T> results = query.SelectList(CreateProjections)
                                        .TransformUsing(Transformers.AliasToBean<T>())
                                        .Where(Restrictions.Gt(Projections.Count<T>(f => f.Quantity), 0))
                                        .List<T>();

                if (results.Count == 0)
                {
                    throw new FaultException("Пустой отчет");
                }

                HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.AdditionalServiceRating));
                ISheet worksheet = workbook.GetSheetAt(0);
                worksheet.CreateFreezePane(6, 2);

                CreateCellStyles(worksheet);

                IRow row = worksheet.GetRow(0);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(string.Format("Период с {0} по {1}", startDate.ToShortDateString(), finishDate.ToShortDateString()));
                cell.CellStyle = boldCellStyle;

                WriteOperatorsHeader(worksheet, session);
                RenderData(session, worksheet, results);

                return workbook;
            }
        }

        private void CreateCellStyles(ISheet worksheet)
        {
            boldCellStyle = worksheet.Workbook.CreateCellStyle();
            IFont font = worksheet.Workbook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            countCellStyle = worksheet.Workbook.CreateCellStyle();
            countCellStyle.DataFormat = worksheet.GetRow(2).GetCell(4).CellStyle.DataFormat;

            sumCellStyle = worksheet.Workbook.CreateCellStyle();
            sumCellStyle.DataFormat = worksheet.GetRow(2).GetCell(5).CellStyle.DataFormat;
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
            IQueryOver<AdditionalService, AdditionalService> query = session.QueryOver<AdditionalService>();
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
            IRow nameRow = worksheet.GetRow(0);
            IRow statRow = worksheet.GetRow(1);
            IRow formulaRow = worksheet.GetRow(2);
            ICellStyle statStyle = statRow.GetCell(4).CellStyle;

            IFont font = worksheet.Workbook.CreateFont();
            font.Boldweight = 1000;

            int col = StartOperatorsStatisticsCol;
            foreach (Operator oper in GetOperators(session))
            {
                ICell cell = nameRow.CreateCell(col);
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
            Single totalCount = 0;
            int col = StartOperatorsStatisticsCol;

            foreach (Operator oper in operators)
            {
                AdditionalServiceRating rating = ratings.SingleOrDefault(r => r.Operator.Equals(oper) && r.Service.Equals(service));
                if (rating != null)
                {
                    totalCount += rating.Quantity;
                }

                ICell cntCol = row.CreateCell(col);
                cntCol.CellStyle = countCellStyle;

                ICell sumCol = row.CreateCell(col + 1);
                sumCol.CellStyle = sumCellStyle;

                if (rating != null)
                {
                    cntCol.SetCellValue(rating.Quantity);
                    sumCol.SetCellValue(rating.Quantity * (double)service.Price);
                }

                col += 2;
            }

            ICell cell = row.CreateCell(4);
            cell.CellStyle = countCellStyle;
            cell.SetCellValue(totalCount);

            cell = row.CreateCell(5);
            cell.CellStyle = sumCellStyle;
            cell.SetCellValue(totalCount * (double)service.Price);
        }

        protected void WriteBoldCell(IRow row, int cellIndex, Action<ICell> setValue)
        {
            ICell cell = row.CreateCell(cellIndex);
            setValue(cell);
            cell.CellStyle = CreateCellBoldStyle(row.Sheet.Workbook);
        }

        protected ICellStyle CreateCellBoldStyle(IWorkbook workBook)
        {
            ICellStyle style = workBook.CreateCellStyle();

            IFont font = workBook.CreateFont();
            font.Boldweight = 1000;
            style.SetFont(font);

            return style;
        }
    }
}