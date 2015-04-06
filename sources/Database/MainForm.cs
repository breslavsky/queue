using Junte.Data.NHibernate;
using Junte.UI.WinForms.NHibernate;
using Junte.UI.WinForms.NHibernate.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Queue.Common;
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
        private const string SectionKey = "profiles";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IConfigurationManager configuration;
        private DatabaseSettingsProfiles profiles;

        private IUnityContainer container;
        private ISessionProvider sessionProvider;

        public MainForm()
            : base()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            profiles = configuration.GetSection<DatabaseSettingsProfiles>(SectionKey);

            container = ServiceLocator.Current.GetInstance<IUnityContainer>();
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

                    int maxPatch = SchemeState.Patches.Max(x => x.Key);
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
            using (LoginForm form = new LoginForm(profiles, DatabaseConnect))
            {
                form.ShowDialog();
                configuration.Save();
            }
        }

        private void damaskImportMenuItem_Click(object sender, EventArgs e)
        {
            new DamaskForm().ShowDialog();
        }

        private void сonstraintsUpdateMenuItem_Click(object sender, EventArgs e)
        {
            Log("Начало обновления ограничений");

            using (var session = sessionProvider.OpenSession())
            {
                foreach (var c in SchemeState.Constraints)
                {
                    var sql = c.Trim();

                    try
                    {
                        session.CreateSQLQuery(sql).ExecuteUpdate();
                    }
                    catch (Exception exception)
                    {
                        Log(sql);
                        Log(exception.Message);
                        return;
                    }
                }
            }

            Log("Обновление ограничений завершено");
        }

        private bool DatabaseConnect(DatabaseSettings s)
        {
            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, s);
            container.RegisterInstance<ISessionProvider>(sessionProvider);

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
                    var o1 = new Operator()
                    {
                        IsActive = true,
                        Name = "Денис",
                        Surname = "Сидоров"
                    };
                    session.Save(o1);

                    var o2 = new Operator()
                    {
                        IsActive = true,
                        Name = "Андрей",
                        Surname = "Шитиков"
                    };
                    session.Save(o2);

                    var o3 = new Operator()
                    {
                        IsActive = true,
                        Name = "Ирина",
                        Surname = "Меньшова"
                    };
                    session.Save(o3);

                    foreach (var o in new[] { o1, o2, o3 })
                    {
                        foreach (var d in new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday })
                        {
                            var s = session.CreateCriteria<DefaultWeekdaySchedule>()
                                .Add(Expression.Eq("DayOfWeek", d))
                                .UniqueResult<DefaultWeekdaySchedule>();
                            if (s != null)
                            {
                                session.Save(new ServiceRendering()
                                {
                                    Mode = ServiceRenderingMode.AllRequests,
                                    Operator = o,
                                    Schedule = s
                                });
                            }
                        }
                    }
                }

                Log("Загрузка услуг");

                var all = ClientRequestRegistrator.Terminal
                    | ClientRequestRegistrator.Manager
                    | ClientRequestRegistrator.Portal;

                for (int i = 1; i < 10; i++)
                {
                    session.Save(new Service()
                    {
                        IsActive = true,
                        Code = string.Format("{0}.0", i),
                        Name = string.Format("Новая услуга {0}", i),
                        LiveRegistrator = all,
                        EarlyRegistrator = all,
                        SortId = i
                    });
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

                int maxPatch = SchemeState.Patches.Max(x => x.Key);

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
                        Template = Templates.ClientRequestCoupon
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
                        Rows = 5,
                        WindowTemplate = Templates.TerminalWindow
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
                        IsActive = true,
                        Surname = "Администратор",
                        Permissions = AdministratorPermissions.Config
                            | AdministratorPermissions.Clients
                            | AdministratorPermissions.ClientsRequests
                            | AdministratorPermissions.Users
                            | AdministratorPermissions.DefaultSchedule
                            | AdministratorPermissions.Workplaces
                            | AdministratorPermissions.Services
                            | AdministratorPermissions.CurrentSchedule
                            | AdministratorPermissions.QueuePlan
                            | AdministratorPermissions.Reports
                            | AdministratorPermissions.Offices
                            | AdministratorPermissions.AdditionalServices
                    };
                    session.Save(administrator);
                }

                Log("Инициализация расписания по уполчанию");

                foreach (var d in new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                        DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday })
                {
                    var s = session.CreateCriteria<DefaultWeekdaySchedule>()
                        .Add(Expression.Eq("DayOfWeek", d))
                        .UniqueResult<DefaultWeekdaySchedule>();
                    if (s == null)
                    {
                        session.Save(new DefaultWeekdaySchedule()
                        {
                            DayOfWeek = d,
                            IsWorked = true,
                            ClientInterval = TimeSpan.FromMinutes(10),
                            StartTime = new TimeSpan(9, 0, 0),
                            FinishTime = new TimeSpan(18, 0, 0),
                            MaxClientRequests = 10,
                            RenderingMode = ServiceRenderingMode.AllRequests,
                            EarlyStartTime = new TimeSpan(9, 0, 0),
                            EarlyFinishTime = new TimeSpan(18, 0, 0),
                            EarlyReservation = 50
                        });
                    }
                }

                transaction.Commit();
            }

            Log("Данные инициализированы");
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

                    int maxPatch = SchemeState.Patches.Max(x => x.Key);
                    Log(string.Format("Доступный патч обновления = {0}", maxPatch));

                    for (int currentPatch = schemeConfig.Version + 1;
                        currentPatch <= maxPatch; currentPatch++)
                    {
                        if (SchemeState.Patches.ContainsKey(currentPatch))
                        {
                            string sql = SchemeState.Patches[currentPatch];

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
                    Log("Конфигурация модели не найдена в базе данных");
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