using Junte.Configuration;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.UI.WPF;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Display
{
    public partial class App : RichApplication
    {
        protected override void RegistrateTypes(IUnityContainer container)
        {
            container.RegisterInstance(container);
            container.RegisterInstance(new ConfigurationManager(Product.Display.AppName, SpecialFolder.ApplicationData));
        }
    }
}