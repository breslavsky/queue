using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using System;
using System.Windows.Forms;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Database
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegisterContainer();

            Application.Run(new MainForm());
        }

        private static void RegisterContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance(new ConfigurationManager(Product.Database.AppName, SpecialFolder.ApplicationData));
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}