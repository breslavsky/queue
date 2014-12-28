using Junte.Data.NHibernate;
using Junte.UI.WinForms.NHibernate;
using Junte.UI.WinForms.NHibernate.Configuration;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.Database
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private static Properties.Settings settings = Properties.Settings.Default;

        private UnityContainer container;
        private ISessionProvider sessionProvider;

        public MainForm()
            : base()
        {
            InitializeComponent();

            container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            settings.Profiles = settings.Profiles ?? new DatabaseSettingsProfiles();
        }

        private void checkPatchesMenuItem_Click(object sender, EventArgs e)
        {
            using (ISession session = sessionProvider.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var schemeConfig = session.Get<SchemeConfig>(ConfigType.Scheme);
                if (schemeConfig != null)
                {
                    Log(string.Format("Текущий патч базы данных = {0}", schemeConfig.Version));

                    int maxPatch = Scheme.Patches.Max(x => x.Key);
                    Log(string.Format("Доступный патч обновления = {0}", maxPatch));
                }
                else
                {
                    Log("Конфигурация модели данных не найдена в базе данных");
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs eventArgs)
        {
            using (LoginForm form = new LoginForm(settings.Profiles, DatabaseConnect))
            {
                form.ShowDialog();
            }
        }

        private void damaskImportMenuItem_Click(object sender, EventArgs e)
        {
            new DamaskForm().ShowDialog();
        }

        private bool DatabaseConnect(DatabaseSettings s)
        {
            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, s);
            container.RegisterInstance<ISessionProvider>(sessionProvider);

            settings.Save();

            schemaMenu.Enabled = dataMenu.Enabled = true;
            connectButton.Enabled = false;

            var model = Assembly.Load("Queue.Model");
            Log(string.Format("Версия модели данных: {0}", model.GetName().Version));

            return true;
        }

        private void demoDataMenuItem_Click(object sender, EventArgs e)
        {
            Log("Загрузка демонстрационных данных");

            using (ISession session = sessionProvider.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                Log("Загрузка рабочих мест");

                int count = session.CreateCriteria<Workplace>()
                    .SetProjection(Projections.Count(Projections.Id()))
                    .UniqueResult<int>();
                if (count == 0)
                {
                    var workplace1 = new Workplace();
                    workplace1.Type = WorkplaceType.Window;
                    workplace1.Number = 1;
                    session.Save(workplace1);

                    var workplace2 = new Workplace();
                    workplace2.Type = WorkplaceType.Window;
                    workplace2.Number = 2;
                    session.Save(workplace2);

                    var workplace3 = new Workplace();
                    workplace3.Type = WorkplaceType.Window;
                    workplace3.Number = 3;
                    session.Save(workplace3);
                }

                Log("Загрузка операторов");

                count = session.CreateCriteria<Operator>()
                    .SetProjection(Projections.Count(Projections.Id()))
                    .UniqueResult<int>();
                if (count == 0)
                {
                    var queueOperator1 = new Operator()
                    {
                        Name = "Денис",
                        Surname = "Сидоров"
                    };
                    session.Save(queueOperator1);

                    var queueOperator2 = new Operator()
                    {
                        Name = "Андрей",
                        Surname = "Шитиков"
                    };
                    session.Save(queueOperator2);

                    var queueOperator3 = new Operator()
                    {
                        Name = "Ирина",
                        Surname = "Меньшова"
                    };
                    session.Save(queueOperator3);
                }

                transaction.Commit();
            }

            Log("Загрузка демонстрационных данных завершена");
        }

        private void initDataMenuItem_Click(object sender, EventArgs e)
        {
            Log("Инициализация данных");

            using (var session = sessionProvider.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                Log("Инициализация конфигурации");

                int maxPatch = Scheme.Patches.Max(x => x.Key);

                var schemeConfig = session.Get<SchemeConfig>(ConfigType.Scheme);
                if (schemeConfig == null)
                {
                    schemeConfig = new SchemeConfig()
                    {
                        Version = maxPatch
                    };
                    session.Save(schemeConfig);
                }

                var defaultConfig = session.Get<DefaultConfig>(ConfigType.Default);
                if (defaultConfig == null)
                {
                    defaultConfig = new DefaultConfig()
                    {
                        QueueName = "Электронная очередь",
                        WorkStartTime = new TimeSpan(10, 0, 0),
                        WorkFinishTime = new TimeSpan(18, 0, 0),
                        MaxClientRequests = 100,
                        MaxRenderingTime = 90
                    };
                    session.Save(defaultConfig);
                }

                var designConfig = session.Get<DesignConfig>(ConfigType.Design);
                if (designConfig == null)
                {
                    designConfig = new DesignConfig();
                    designConfig.LogoSmall = new byte[] { };
                    session.Save(designConfig);
                }

                var couponConfig = session.Get<CouponConfig>(ConfigType.Coupon);
                if (couponConfig == null)
                {
                    couponConfig = new CouponConfig()
                    {
                        Template = Templates.Coupon
                    };
                    session.Save(couponConfig);
                }

                var SMTPConfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                if (SMTPConfig == null)
                {
                    SMTPConfig = new SMTPConfig()
                    {
                        Server = "smtp.junte.ru",
                        User = "user@junte.ru",
                        Password = string.Empty,
                        From = "user@junte.ru"
                    };
                    session.Save(SMTPConfig);
                }

                var portalConfig = session.Get<PortalConfig>(ConfigType.Portal);
                if (portalConfig == null)
                {
                    portalConfig = new PortalConfig()
                    {
                        Header = Templates.PortalHeader,
                        Footer = Templates.PortalFooter,
                        CurrentDayRecording = true
                    };
                    session.Save(portalConfig);
                }

                var mediaConfig = session.Get<MediaConfig>(ConfigType.Media);
                if (mediaConfig == null)
                {
                    mediaConfig = new MediaConfig()
                    {
                        ServiceUrl = "http://queue:4506/",
                        Ticker = "Добро пожаловать! Вы для нас очень важны!",
                        TickerSpeed = 5
                    };
                    session.Save(mediaConfig);

                    var mediaConfigFile = new MediaConfigFile()
                    {
                        Name = "Демонстрационый файл (файл не загружен)"
                    };
                    session.Save(mediaConfigFile);
                }

                var terminalConfig = session.Get<TerminalConfig>(ConfigType.Terminal);
                if (terminalConfig == null)
                {
                    terminalConfig = new TerminalConfig()
                    {
                        PIN = 1001,
                        CurrentDayRecording = true,
                        Columns = 2,
                        Rows = 5
                    };
                    session.Save(terminalConfig);
                }

                var notificationConfig = session.Get<NotificationConfig>(ConfigType.Notification);
                if (notificationConfig == null)
                {
                    notificationConfig = new NotificationConfig()
                    {
                        ClientRequestsLength = 5
                    };
                    session.Save(notificationConfig);
                }

                Log("Инициализация администратора");

                int count = session.CreateCriteria<Administrator>()
                    .SetProjection(Projections.Count(Projections.Id()))
                    .UniqueResult<int>();
                if (count == 0)
                {
                    var administrator = new Administrator()
                    {
                        Surname = "Администратор"
                    };
                    session.Save(administrator);
                }

                Log("Инициализация расписания по уполчанию");

                #region monday

                var schedule1 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Monday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule1 == null)
                {
                    schedule1 = new DefaultWeekdaySchedule();
                    schedule1.DayOfWeek = DayOfWeek.Monday;
                    session.Save(schedule1);

                    Log("Создано расписание на понедельник");
                }

                #endregion monday

                #region tuesday

                var schedule2 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Tuesday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule2 == null)
                {
                    schedule2 = new DefaultWeekdaySchedule();
                    schedule2.DayOfWeek = DayOfWeek.Tuesday;
                    session.Save(schedule2);
                }

                #endregion tuesday

                #region wednesday

                var schedule3 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Wednesday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule3 == null)
                {
                    schedule3 = new DefaultWeekdaySchedule();
                    schedule3.DayOfWeek = DayOfWeek.Wednesday;
                    session.Save(schedule3);
                }

                #endregion wednesday

                #region thursday

                var schedule4 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Thursday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule4 == null)
                {
                    schedule4 = new DefaultWeekdaySchedule();
                    schedule4.DayOfWeek = DayOfWeek.Thursday;
                    session.Save(schedule4);
                }

                #endregion thursday

                #region friday

                var schedule5 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Friday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule5 == null)
                {
                    schedule5 = new DefaultWeekdaySchedule();
                    schedule5.DayOfWeek = DayOfWeek.Friday;
                    session.Save(schedule5);
                }

                #endregion friday

                #region saturday

                var schedule6 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Saturday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule6 == null)
                {
                    schedule6 = new DefaultWeekdaySchedule();
                    schedule6.DayOfWeek = DayOfWeek.Saturday;
                    session.Save(schedule6);
                }

                #endregion saturday

                #region sunday

                var schedule7 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Sunday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule7 == null)
                {
                    schedule7 = new DefaultWeekdaySchedule();
                    schedule7.DayOfWeek = DayOfWeek.Sunday;
                    session.Save(schedule7);
                }

                #endregion sunday

                transaction.Commit();
            }

            Log("Данные инициализорованы");
        }

        private void installPatchesMenuItem_Click(object sender, EventArgs e)
        {
            using (var session = sessionProvider.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var schemeConfig = session.Get<SchemeConfig>(ConfigType.Scheme);
                if (schemeConfig != null)
                {
                    Log(string.Format("Текущий патч базы данных = {0}", schemeConfig.Version));

                    int maxPatch = Scheme.Patches.Max(x => x.Key);
                    Log(string.Format("Доступный патч обновления = {0}", maxPatch));

                    for (int currentPatch = schemeConfig.Version + 1;
                        currentPatch <= maxPatch; currentPatch++)
                    {
                        if (Scheme.Patches.ContainsKey(currentPatch))
                        {
                            string sql = Scheme.Patches[currentPatch];

                            Log(string.Format("Приминение патча [{0}]", sql));

                            try
                            {
                                session.CreateSQLQuery(sql).ExecuteUpdate();
                            }
                            catch (Exception exception)
                            {
                                Log(exception.Message);
                                return;
                            }
                        }
                    }

                    schemeConfig.Version = maxPatch;
                    session.Save(schemeConfig);

                    transaction.Commit();

                    Log("Обновление завершено");
                }
                else
                {
                    Log("Конфигурация модели данных не найдена в базе данных");
                }
            }
        }

        private void Log(string message)
        {
            logTextBox.AppendText(string.Format("{0:T} {1}", DateTime.Now, message));
            logTextBox.AppendText(Environment.NewLine);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sessionProvider != null)
            {
                sessionProvider.Dispose();
            }
        }

        private void schemaUpdateMenuItem_Click(object sender, EventArgs e)
        {
            Log("Обновление структуры базы данных");

            try
            {
                sessionProvider.SchemaUpdate();
                Log("Структура базы данных обновлена");
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        private void schemaValidateMenu_Click(object sender, EventArgs e)
        {
            Log("Проверка структуры базы данных");

            try
            {
                sessionProvider.SchemaValidate();
                Log("Структура базы данных верна");
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }
    }
}