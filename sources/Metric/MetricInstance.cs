using Junte.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using NLog;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Queue.Metric
{
    public class MetricInstance : IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private SessionProvider sessionProvider;

        private Timer updateTimer;
        private int UpdateInterval = 60000;
        private bool disposed;

        public MetricInstance(MetricSettings settings)
        {
            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, settings.Database);

            updateTimer = new Timer();
            updateTimer.Elapsed += updateTimer_Elapsed;
        }

        public void Start()
        {
            updateTimer.Start();
        }

        public void Stop()
        {
            updateTimer.Stop();
        }

        private void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            updateTimer.Stop();
            if (updateTimer.Interval < UpdateInterval)
            {
                updateTimer.Interval = UpdateInterval;
            }

            try
            {
                CollectMetric();
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
            finally
            {
                updateTimer.Start();
            }
        }

        private void CollectMetric()
        {
            using (ISession session = sessionProvider.OpenSession())
            {
                CollectQueuePlanMetric(session);
                CollectQueuePlanServiceMetric(session);
                CollectQueuePlanOperatorMetric(session);
            }
        }

        private void CollectQueuePlanMetric(ISession session)
        {
            var requestDate = DateTime.Now;

            var projections = Projections.ProjectionList()
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Waiting),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Waiting")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Rendered")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Live")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Early")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) as RenderTime", new string[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                         Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.SqlProjection("({alias}.RenderStartTime - {alias}.WaitingStartTime) as WaitingTime", new string[] { "WaitingTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                         Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "WaitingTime")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.Property("Productivity"), Projections.Constant(0, NHibernateUtil.Single))), "Productivity");

            IList<QueuePlanMetric> queuePlanMetrics = session.CreateCriteria<ClientRequest>()
                .Add(Restrictions.Eq("RequestDate", requestDate.Date))
                .SetProjection(projections)
                .SetResultTransformer(Transformers.AliasToBean(typeof(QueuePlanMetric)))
                .List<QueuePlanMetric>();

            foreach (var m in queuePlanMetrics)
            {
                m.Year = requestDate.Year;
                m.Month = requestDate.Month;
                m.Day = requestDate.Day;
                m.Hour = requestDate.Hour;
                m.Minute = requestDate.Minute;
                m.Second = requestDate.Second;

                if (m.Rendered > 0)
                {
                    m.Productivity /= m.Rendered;
                }

                ValidationError[] errors = m.Validate();
                if (errors.Length > 0)
                {
                    logger.Error(errors);
                }

                session.Save(m);
            }
        }

        private void CollectQueuePlanServiceMetric(ISession session)
        {
            var requestDate = DateTime.Now;
            var projections = Projections.ProjectionList()
                    .Add(Projections.GroupProperty("Service"))
                    .Add(Projections.Property("Service"), "Service")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Waiting),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Waiting")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Rendered")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Live")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                         Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Early")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) as RenderTime", new string[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                         Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.SqlProjection("({alias}.RenderStartTime - {alias}.WaitingStartTime) as WaitingTime", new string[] { "WaitingTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                         Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "WaitingTime")
                    .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                         Projections.Property("Productivity"), Projections.Constant(0, NHibernateUtil.Single))), "Productivity");

            var queuePlanServiceMetrics = session.CreateCriteria<ClientRequest>()
                                                                            .Add(Restrictions.Eq("RequestDate", requestDate.Date))
                                                                            .SetProjection(projections)
                                                                            .SetResultTransformer(Transformers.AliasToBean(typeof(QueuePlanServiceMetric)))
                                                                            .List<QueuePlanServiceMetric>();

            foreach (var m in queuePlanServiceMetrics)
            {
                m.Year = requestDate.Year;
                m.Month = requestDate.Month;
                m.Day = requestDate.Day;
                m.Hour = requestDate.Hour;
                m.Minute = requestDate.Minute;
                m.Second = requestDate.Second;

                if (m.Rendered > 0)
                {
                    m.Productivity /= m.Rendered;
                }

                var error = m.Validate().FirstOrDefault();
                if (error != null)
                {
                    logger.Error(error.Message);
                }

                session.Save(m);
            }
        }

        private void CollectQueuePlanOperatorMetric(ISession session)
        {
            var requestDate = DateTime.Now;
            var projections = Projections.ProjectionList()
                     .Add(Projections.GroupProperty("Operator"))
                     .Add(Projections.Property("Operator"), "Operator")
                     .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                          Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) as RenderTime", new string[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                          Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")
                     .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                          Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Rendered")
                     .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                          Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Live")
                     .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                          Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Early")
                     .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                          Projections.Property("Productivity"), Projections.Constant(0, NHibernateUtil.Single))), "Productivity");

            var queuePlanOperatorMetrics = session.CreateCriteria<ClientRequest>()
                                                                            .Add(Restrictions.Eq("RequestDate", requestDate.Date))
                                                                            .SetProjection(projections)
                                                                            .SetResultTransformer(Transformers.AliasToBean(typeof(QueuePlanOperatorMetric)))
                                                                            .List<QueuePlanOperatorMetric>();

            foreach (var m in queuePlanOperatorMetrics)
            {
                m.Year = requestDate.Year;
                m.Month = requestDate.Month;
                m.Day = requestDate.Day;
                m.Hour = requestDate.Hour;
                m.Minute = requestDate.Minute;
                m.Second = requestDate.Second;

                if (m.Rendered > 0)
                {
                    m.Productivity /= m.Rendered;
                }

                var error = m.Validate().FirstOrDefault();
                if (error != null)
                {
                    logger.Error(error.Message);
                }

                session.Save(m);
            }

            session.Flush();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (sessionProvider != null)
                {
                    sessionProvider.Dispose();
                }
            }

            disposed = true;
        }

        ~MetricInstance()
        {
            Dispose(false);
        }
    }
}