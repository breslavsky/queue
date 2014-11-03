using Queue.Model.Common;
using Queue.UI.WinForms;
using System;
using System.Globalization;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //var culture = CultureInfo.CreateSpecificCulture("en-US");
            var culture = CultureInfo.CreateSpecificCulture("zh-CN");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Properties.Settings settings = Properties.Settings.Default;

            while (true)
            {
                using (var loginForm = new LoginForm(UserRole.Administrator)
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

                        var mainForm = new MainForm(loginForm.ChannelBuilder, (QueueAdministrator)loginForm.User);
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