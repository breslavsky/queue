using Queue.Common;
using Queue.Model.Common;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator
{
    internal static class Program
    {
        private string private const string AppName = "Queue.Operator";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConfigurationManager configuration = new ConfigurationManager(AppName);
            settings = configuration.GetSection<ServerConnectionSettings>("connection", () =>
                                                                new ServerConnectionSettings()
                                                                {
                                                                    Database = GetDefaultDatabaseSettings(),
                                                                    Services = GetDefaultServicesConfig(),
                                                                    Debug = true
                                                                });

            Properties.Settings settings = Properties.Settings.Default;

            while (true)
            {
                using (LoginForm loginForm = new LoginForm(UserRole.Operator)
                {
                    Endpoint = settings.Endpoint,
                    UserId = settings.UserId,
                    Password = settings.Password,
                    IsRemember = settings.IsRemember
                })
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        settings.Endpoint = loginForm.Endpoint;
                        settings.UserId = loginForm.UserId;
                        settings.Password = loginForm.Password;
                        settings.IsRemember = loginForm.IsRemember;
                        settings.Save();

                        var mainForm = new OperatorForm(loginForm.ChannelBuilder, (QueueOperator)loginForm.User);
                        Application.Run(mainForm);

                        if (mainForm.IsLogout)
                        {
                            settings.Password = string.Empty;
                            settings.IsRemember = false;
                            settings.Save();

                            continue;
                        }
                    }

                    break;
                }
            }
        }
    }
}