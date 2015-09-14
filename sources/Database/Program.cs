using Junte.Configuration;
using Junte.UI.WinForms.NHibernate.Configuration;
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
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static DatabaseSettingsProfiles profiles;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Database.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);

            profiles = configuration.GetSection<DatabaseSettingsProfiles>(DatabaseSettingsProfiles.SectionKey);
            container.RegisterInstance(profiles);

            Application.Run(new MainForm());
        }
    }
}