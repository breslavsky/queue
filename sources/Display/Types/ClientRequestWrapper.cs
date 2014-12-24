using Queue.Model.Common;
using Queue.Services.DTO;
using Queue.UI.WPF.Types;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace Queue.Display.Types
{
    public class ClientRequestWrapper : ObservableObject
    {
        private ClientRequest request;

        private string state;
        private Brush stateBrush;

        public int Number { get; set; }

        public Operator Operator { get; set; }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public Brush StateBrush
        {
            get { return stateBrush; }
            set { SetProperty(ref stateBrush, value); }
        }

        public ClientRequest Request
        {
            get { return request; }
            set
            {
                request = value;
                Update();
            }
        }

        private void Update()
        {
            Number = request.Number;
            State = request.State.Translate();

            Drawing.Color c = Drawing.ColorTranslator.FromHtml(request.Color);
            StateBrush = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }
    }
}