using Queue.Model.Common;
using Queue.UI.WPF.Pages.Models;
using System.ComponentModel;
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
            Model.PropertyChanged += ModelPropertyChanged;

            DataContext = Model;
        }

        public LoginPageViewModel Model { get; private set; }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Password":
                    if (!string.IsNullOrWhiteSpace(Model.Password))
                    {
                        passwordBox.Password = Model.Password;
                    }
                    break;
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            Model.Password = passwordBox.Password;
        }
    }
}