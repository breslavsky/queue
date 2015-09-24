using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Notification.ViewModels;
using Queue.UI.WPF;
using System.Windows.Controls;

namespace Queue.Notification
{
    public partial class MainWindow : RichWindow, IMainWindow
    {
        protected override Panel RootElement { get { return mainGrid; } }

        public MainWindow()
            : base()
        {
            InitializeComponent();

            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterInstance<IMainWindow>(this);
            DataContext = container.Resolve<MainWindowViewModel>();
        }

        public void Navigate(Page page)
        {
            content.NavigationService.Navigate(page);
        }
    }
}