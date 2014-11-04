using Junte.Data.NHibernate;
using Junte.UI.WinForms;
using Junte.UI.WinForms.NHibernate;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Resources;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DialogResult = System.Windows.Forms.DialogResult;

namespace Queue.Database
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private static Properties.Settings settings = Properties.Settings.Default;

        private ISessionProvider sessionProvider;

        public MainForm()
            : base()
        {
            InitializeComponent();
        }

        private void log(string message)
        {
            logTextBox.AppendText(message + Environment.NewLine);
        }

        #region menu
        private void schemaValidateMenu_Click(object sender, EventArgs e)
        {
            log("Проверка структуры базы данных");

            try
            {
                sessionProvider.SchemaValidate();
                log("Структура базы данных верна");
            }
            catch (Exception exception)
            {
                log(string.Format("Структура базы данных не верна [{0}]", exception.Message));
            }
        }

        private void schemaUpdateMenuItem_Click(object sender, EventArgs e)
        {
            log("Обновление структуры базы данных");

            try
            {
                sessionProvider.SchemaUpdate();
                log("Структура базы данных обновлена");
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
                return;
            }
        }

        private void damaskMenuItem_Click(object sender, EventArgs e)
        {
            new DamaskForm().ShowDialog();
        }

        #endregion menu

        private void connectButton_Click(object sender, EventArgs eventArgs)
        {
            using (var loginForm = new LoginForm(settings.Database ?? new DatabaseSettings()))
            {
                loginForm.OnLogin += (s, e) =>
                {
                    try
                    {
                        sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, e.Settings);

                        settings.Database = e.Settings;
                        settings.Save();

                        loginForm.DialogResult = DialogResult.OK;
                    }
                    catch (Exception exception)
                    {
                        UIHelper.Error("Ошибка при подключении к базе данных", exception, "Проверьте параметры подключения");
                    }
                };

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            #region установка базы

            using (var session = sessionProvider.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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

                log("Текущий патч базы данных " + schemeConfig.Version);

                #region приминение патчей

                for (int currentPatch = schemeConfig.Version + 1;
                    currentPatch <= maxPatch; currentPatch++)
                {
                    if (Scheme.Patches.ContainsKey(currentPatch))
                    {
                        string sql = Scheme.Patches[currentPatch];

                        log("Приминение патча [" + sql +"]");

                        try
                        {
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                        }
                        catch (Exception exception)
                        {
                            UIHelper.Error(exception);
                            return;
                        }
                    }
                }

                schemeConfig.Version = maxPatch;
                session.Save(schemeConfig);

                #endregion приминение патчей

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
                        MediaConfig = mediaConfig,
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

                    administrator = new Administrator()
                    {
                        Surname = "Терминал"
                    };
                    session.Save(administrator);

                    administrator = new Administrator()
                    {
                        Surname = "Портал"
                    };
                    session.Save(administrator);

                    var manager = new Manager()
                    {
                        Surname = "Менеджер"
                    };
                    session.Save(manager);
                }

                count = session.CreateCriteria<Workplace>()
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

                #region monday

                var schedule1 = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", DayOfWeek.Monday))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (schedule1 == null)
                {
                    schedule1 = new DefaultWeekdaySchedule();
                    schedule1.DayOfWeek = DayOfWeek.Monday;
                    session.Save(schedule1);
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

            #endregion установка базы

            topMenu.Enabled = true;
            connectButton.Enabled = false;

            FormClosing += (e, s) =>
            {
                sessionProvider.Dispose();
            };
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Process.Start(Application.StartupPath);
            }
        }
    }
}