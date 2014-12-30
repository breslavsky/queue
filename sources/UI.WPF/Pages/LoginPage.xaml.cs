using Queue.Model.Common;
using Queue.UI.WPF.Pages.Models;
using System.Windows;

namespace Queue.UI.WPF
{
    public partial class LoginPage : RichPage
    {
        public LoginPage(UserRole userRole)
        {
            InitializeComponent();

            passwordBox.PasswordChanged += PasswordChanged;
            Model = new LoginPageViewModel(userRole, this);

            DataContext = Model;
        }

        public LoginPageViewModel Model { get; private set; }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            Model.Password = passwordBox.Password;
        }

        internal void AdjustModel()
        {
            passwordBox.Password = Model.Password;
        }
    }
}