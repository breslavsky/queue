using Queue.Model.Common;
using System.Windows;
using System.Windows.Controls;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator.Silverlight
{
    public partial class MainControl : UserControl
    {
        private Settings settings;

        public MainControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs eventArgs)
        {
            settings = IsolatedStorage<Settings>.Get();
            if (settings == null)
            {
                settings = new Settings()
                {
                    Endpoint = "net.tcp://queue:4505"
                };
                settings.Save();
            }

            Login();
        }

        private void Login()
        {
            var loginPage = new LoginPage(UserRole.Operator)
            {
                Endpoint = settings.Endpoint,
                UserId = settings.UserId,
                Password = settings.Password,
                IsRemember = settings.IsRemember
            };
            loginPage.OnLogined += (s1, e1) =>
            {
                settings.Endpoint = loginPage.Endpoint;
                settings.UserId = loginPage.UserId;
                settings.Password = loginPage.Password;
                settings.IsRemember = loginPage.IsRemember;
                settings.Save();

                var homePage = new HomePage(loginPage.ChannelBuilder, (QueueOperator)loginPage.User);
                homePage.OnLogout += (s2, e2) =>
                {
                    settings.Password = string.Empty;
                    settings.IsRemember = false;
                    settings.Save();
                    Login();
                };

                content.Child = homePage;
            };

            content.Child = loginPage;
        }
    }
}