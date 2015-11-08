using Microsoft.Practices.Unity;
using Queue.Display.ViewModels;
using Queue.UI.WPF;
using System.Windows.Controls;

namespace Queue.Display
{
    public partial class MainWindow : RichWindow, IMainWindow
    {
        protected override Panel RootElement { get { return mainGrid; } }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        public MainWindow()
            : base()
        {
            InitializeComponent();

            UnityContainer.RegisterInstance<IMainWindow>(this);

            DataContext = new MainWindowViewModel();
        }

        public void Navigate(Page page)
        {
            content.NavigationService.Navigate(page);
        }
    }
}