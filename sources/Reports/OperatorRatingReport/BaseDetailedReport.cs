using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Linq;
using System.ServiceModel;

namespace Queue.Reports.OperatorRatingReport
{
    public abstract class BaseDetailedReport<T>
    {
        protected ServiceRatingReportSettings settings;
        protected Guid[] operators;

        protected ISessionProvider SessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public BaseDetailedReport(Guid[] operators, ServiceRatingReportSettings settings)
        {
            this.settings = settings;
            this.operators = operators;
        }

        public HSSFWorkbook Generate()
        {
            DateTime startDate = GetStartDate();
            DateTime finishDate = GetFinishDate();

            if (startDate > finishDate)
            {
                throw new FaultException("Начальная дата не может быть больше чем конечная");
            }

            Conjunction conjunction = Expression.Conjunction();
            if (operators.Length > 0)
            {
                conjunction.Add(Expression.In("Operator.Id", operators));
            }

            conjunction.Add(Expression.Ge("RequestDate", startDate));
            conjunction.Add(Expression.Le("RequestDate", finishDate));

            ProjectionList projections = GetProjections();

            using (ISession session = SessionProvider.OpenSession())
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

                return InternalGenerate(session, data);
            }
        }

        protected abstract DateTime GetStartDate();

        protected abstract DateTime GetFinishDate();

        protected abstract ProjectionList GetProjections();

        protected abstract HSSFWorkbook InternalGenerate(ISession session, T[] data);

        protected ProjectionList GetCommonProjections()
        {
            ProjectionList projections = Projections.ProjectionList();

            projections
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

        protected ICellStyle CreateCellBoldStyle(IWorkbook workBook)
        {
            ICellStyle boldCellStyle = workBook.CreateCellStyle();

            IFont font = workBook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            return boldCellStyle;
        }
    }
}