using Queue.UI.WPF.Models;

namespace Queue.UI.WPF
{
    public partial class ConnectPage : RichPage
    {
        public ConnectPageViewModel Model { get; private set; }

        public ConnectPage()
            : base()
        {
            InitializeComponent();

            Model = new ConnectPageViewModel(this);

            DataContext = Model;
        }
    }
}