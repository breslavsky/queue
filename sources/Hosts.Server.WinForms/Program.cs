using System;
using System.Windows.Forms;

namespace Queue.Hosts.Server.WinForms
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}