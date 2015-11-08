using Junte.Configuration;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.UI.WPF;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Terminal
{
    public partial class App : RichApplication
    {
        protected override void RegistrateTypes(IUnityContainer container)
        {
            container.RegisterInstance(new ConfigurationManager(Product.Terminal.AppName, SpecialFolder.ApplicationData));
            container.RegisterType<AppSettings>(new InjectionFactory(c => c.Resolve<ConfigurationManager>()
                                                                .GetSection<AppSettings>(AppSettings.SectionKey)));
        }
    }
}