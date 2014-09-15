using Queue.Display.Models;
using Queue.UI.WPF;
using System.Windows;

namespace Queue.Display.Pages
{
    public partial class LoginPage : RichPage
    {
        public LoginPageVM Model { get; private set; }

        public LoginPage()
            : base()
        {
            InitializeComponent();

            Model = new LoginPageVM(this);

            DataContext = Model;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.Initialize();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Model.Dispose();
        }
    }
}