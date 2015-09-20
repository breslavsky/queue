using Junte.Translation;
using Queue.Services.DTO;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace Queue.Notification.UserControls
{
    public partial class ClientRequestStateControl : UserControl
    {
        public string Workplace { get; set; }

        public string State { get; set; }

        public Brush StateBrush { get; set; }

        public ClientRequestStateControl(ClientRequest request)
        {
            InitializeComponent();

            Workplace = request.Operator != null && request.Operator.Workplace != null ?
                request.Operator.Workplace.ToString() : String.Empty;
            State = Translater.Enum(request.State);

            var c = Drawing.ColorTranslator.FromHtml(request.Color);
            StateBrush = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));

            DataContext = this;
        }
    }
}