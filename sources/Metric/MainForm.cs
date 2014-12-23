using Junte.Data.NHibernate;
using Junte.UI.WinForms.NHibernate;
using Junte.UI.WinForms.NHibernate.Configuration;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Queue.Metric
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private static Properties.Settings settings = Properties.Settings.Default;

        private SessionProvider sessionProvider;

        private Timer updateTimer;
        private int UpdateInterval = 60000;
        private long iterations;

        public MainForm()
            : base()
        {
            InitializeComponent();

            updateTimer = new Timer();
            updateTimer.Elapsed += updateTimer_Elapsed;

            settings.Profiles = settings.Profiles ?? new DatabaseSettingsProfiles();
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
                using (var session = sessionProvider.OpenSession())
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

                    var queuePlanMetrics = session.CreateCriteria<ClientRequest>()
                        .Add(Expression.Eq("RequestDate", requestDate.Date))
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

                        var errors = m.Validate();
                        if (errors.Length > 0)
                        {
                            logger.Error(ValidationError.ToException(errors));
                        }

                        session.Save(m);
                    }

                    projections = Projections.ProjectionList()
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
                        .Add(Expression.Eq("RequestDate", requestDate.Date))
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

                        var errors = m.Validate();
                        if (errors.Length > 0)
                        {
                            logger.Error(ValidationError.ToException(errors));
                        }

                        session.Save(m);
                    }

                    projections = Projections.ProjectionList()
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
                        .Add(Expression.Eq("RequestDate", requestDate.Date))
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

                        var errors = m.Validate();
                        if (errors.Length > 0)
                        {
                            logger.Error(ValidationError.ToException(errors));
                        }

                        session.Save(m);
                    }

                    session.Flush();
                    iterations++;

                    Invoke(new MethodInvoker(() =>
                    {
                        iterationsLabel.Text = string.Format("Итераций: {0}", iterations);
                    }));
                }
            }
            catch (Exception exception)
            {
                iterationsLabel.Text = exception.Message;
            }
            finally
            {
                updateTimer.Start();
            }
        }

        private void connectButton_Click(object sender, EventArgs eventArgs)
        {
            using (LoginForm f = new LoginForm(settings.Profiles, DatabaseConnect))
            {
                f.ShowDialog();
            }
        }

        private bool DatabaseConnect(DatabaseSettings s)
        {
            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, s);
            settings.Save();

            databaseGroupBox.Enabled = false;

            updateTimer.Start();

            FormClosing += (sender, eventArgs) =>
            {
                sessionProvider.Dispose();
                updateTimer.Stop();
            };

            return true;
        }
    }
}