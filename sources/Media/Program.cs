using Queue.Model.Common;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;

namespace Queue.Media
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Properties.Settings settings = Properties.Settings.Default;

            while (true)
            {
                using (LoginForm loginForm = new LoginForm(UserRole.Administrator)
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

                        var mainForm = new MainForm(loginForm.ChannelBuilder, (Administrator)loginForm.User);
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