using Junte.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Queue.Reports.OperatorRatingReport
{
    public abstract class BaseDetailedReport<T> : BaseReport
    {
        protected OperatorRatingReportSettings settings;

        private Lazy<Operator[]> allOperators;

        protected override int ColumnCount { get { return 20; } }

        public BaseDetailedReport(OperatorRatingReportSettings settings)
            : base()
        {
            this.settings = settings;

            allOperators = new Lazy<Operator[]>(() =>
            {
                using (var session = SessionProvider.OpenSession())
                {
                    return session.QueryOver<Operator>()
                                    .List()
                                    .OrderBy(o => o.ToString())
                                    .ToArray();
                }
            });
        }

        protected override HSSFWorkbook InternalGenerate()
        {
            var startDate = GetStartDate();
            var finishDate = GetFinishDate();

            if (startDate > finishDate)
            {
                throw new FaultException("Начальная дата не может быть больше чем конечная");
            }

            var conjunction = Restrictions.Conjunction();
            if (settings.Operators.Length > 0)
            {
                conjunction.Add(Restrictions.In("Operator.Id", settings.Operators));
            }
            else
            {
                conjunction.Add(Restrictions.IsNotNull("Operator.Id"));
            }

            conjunction.Add(Restrictions.Ge("RequestDate", startDate));
            conjunction.Add(Restrictions.Le("RequestDate", finishDate));

            var projections = GetProjections();

            using (var session = SessionProvider.OpenSession())
            {
                T[] data = session.CreateCriteria<ClientRequest>()
                                   .Add(conjunction)
                                   .SetProjection(projections)
                                   .SetResultTransformer(Transformers.AliasToBean(typeof(T)))
                                   .List<T>()
                                   .ToArray();

                if (data.Length == 0)
                {
                    throw new FaultException("Пустой отчет");
                }

                var workbook = new HSSFWorkbook(new MemoryStream(Templates.OperatorRating));
                var worksheet = workbook.GetSheetAt(0);

                styles = new StandardCellStyles(workbook);

                WriteCell(worksheet.GetRow(0), 0, c => c.SetCellValue(GetTitle()), styles[StandardCellStyles.BoldStyle]);

                RenderData(worksheet, data);

                return workbook;
            }
        }

        private string GetTitle()
        {
            return String.Format("Период с {0} по {1}", GetStartDate().ToShortDateString(), GetFinishDate().ToShortDateString());
        }

        protected abstract DateTime GetStartDate();

        protected abstract DateTime GetFinishDate();

        protected abstract ProjectionList GetProjections();

        protected abstract void RenderData(ISheet worksheet, T[] data);

        protected ProjectionList GetCommonProjections()
        {
            var projections = Projections.ProjectionList();

            projections
                .Add(Projections.GroupProperty("Operator"))
                .Add(Projections.Property("Operator"), "Operator")

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
                    Projections.Conditional(Restrictions.Le("Subjects", 0),
                        Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) as RenderTime", new[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                        Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) / Subjects as RenderTime", new[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan })),
                    Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                    Projections.SqlProjection("({alias}.RenderStartTime - {alias}.WaitingStartTime) as WaitingTime", new[] { "WaitingTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                    Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "WaitingTime")

                .Add(Projections.Sum("Subjects"), "SubjectsTotal")
                .Add(Projections.Avg("Rating"), "RatingAvg")
                .Add(Projections.Min("Rating"), "RatingMin")
                .Add(Projections.Max("Rating"), "RatingMax")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                    Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsLive")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                    Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsEarly");

            return projections;
        }

        protected void RenderRating(IRow row, OperatorRating rating)
        {
            var cell = row.CreateCell(5);
            cell.SetCellValue(rating.Total);
            cell = row.CreateCell(6);
            cell.SetCellValue(rating.Live);
            cell = row.CreateCell(7);
            cell.SetCellValue(rating.Early);
            cell = row.CreateCell(8);
            cell.SetCellValue(rating.Waiting);
            cell = row.CreateCell(9);
            cell.SetCellValue(rating.Absence);
            cell = row.CreateCell(10);
            cell.SetCellValue(rating.Rendered);
            cell = row.CreateCell(11);
            cell.SetCellValue(rating.Canceled);
            cell = row.CreateCell(12);
            cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.RenderTime.TotalMinutes / rating.Rendered) : 0);
            cell = row.CreateCell(13);
            cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.WaitingTime.TotalMinutes / rating.Rendered) : 0);
            cell = row.CreateCell(14);
            cell.SetCellValue(rating.SubjectsTotal);
            cell = row.CreateCell(15);
            cell.SetCellValue(rating.SubjectsLive);
            cell = row.CreateCell(16);
            cell.SetCellValue(rating.SubjectsEarly);
            cell = row.CreateCell(17);
            cell.SetCellValue(rating.RatingMin);
            cell = row.CreateCell(18);
            cell.SetCellValue(rating.RatingMax);
            cell = row.CreateCell(19);
            cell.SetCellValue(Math.Round(rating.RatingAvg * 100) / 100);
        }

        internal OperatorRating[] GetOperatorsRatings(OperatorRating[] input)
        {
            var result = new List<OperatorRating>();

            foreach (var op in allOperators.Value)
            {
                var rating = input.SingleOrDefault(o => o.Operator.Equals(op));
                if (rating == null)
                {
                    rating = new OperatorRating()
                    {
                        Operator = op
                    };
                }

                result.Add(rating);
            }

            return result.ToArray();
        }
    }
}