using Queue.Services.DTO;
using System.Windows.Controls;
using System.Windows.Media;
using Drawing = System.Drawing;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Notification.UserControls
{
    public partial class ClientRequestStateUserControl : UserControl
    {
        public string Workplace { get; set; }

        public string State { get; set; }

        public Brush StateBrush { get; set; }

        public ClientRequestStateUserControl(ClientRequest request)
        {
            InitializeComponent();

            this.Workplace = request.Operator.Workplace.ToString();
            this.State = Translation.ClientRequestState.ResourceManager.GetString(request.State.ToString());

            Drawing.Color c = Drawing.ColorTranslator.FromHtml(request.Color);
            StateBrush = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));

            DataContext = this;
        }
    }
}