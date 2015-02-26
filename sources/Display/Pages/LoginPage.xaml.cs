using Queue.Display.ViewModels;
using Queue.UI.WPF;

namespace Queue.Display.Pages
{
    public partial class LoginPage : RichPage
    {
        public LoginPageViewModel Model
        {
            get { return DataContext as LoginPageViewModel; }
        }

        public LoginPage()
            : base()
        {
            InitializeComponent();

            DataContext = new LoginPageViewModel(this);
        }
    }
}