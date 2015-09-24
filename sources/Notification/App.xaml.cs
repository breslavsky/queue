using Junte.Configuration;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
using Queue.UI.WPF;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Notification
{
    public partial class App : RichApplication
    {
        protected override void RegistrateTypes(IUnityContainer container)
        {
            container.RegisterInstance(new ConfigurationManager(Product.Notification.AppName, SpecialFolder.ApplicationData));
            container.RegisterType<AppSettings>(new InjectionFactory(c => c.Resolve<ConfigurationManager>()
                                                                .GetSection<AppSettings>(AppSettings.SectionKey)));
            container.RegisterType<HubSettings>(new InjectionFactory(c => c.Resolve<ConfigurationManager>()
                                                                .GetSection<HubSettings>(HubSettings.SectionKey)));
        }
    }
}