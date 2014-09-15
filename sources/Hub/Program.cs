using Queue.UI.WinForms;
using System;
using System.Windows.Forms;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Queue.Hub
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Properties.Settings settings = Properties.Settings.Default;

            while (true)
            {
                using (ConnectForm connectForm = new ConnectForm()
                {
                    Endpoint = settings.Endpoint,
                    IsRemember = settings.IsRemember
                })
                {
                    if (connectForm.ShowDialog() == DialogResult.OK)
                    {
                        settings.Endpoint = connectForm.Endpoint;
                        settings.Save();

                        var mainForm = new MainForm(connectForm.ChannelBuilder);
                        Application.Run(mainForm);

                        if (mainForm.IsLogout)
                        {
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