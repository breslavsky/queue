using Queue.Terminal.UserControls;
using System;

namespace Queue.Terminal
{
    public class RenderServicesEventArgs : EventArgs
    {
        public SelectServiceButton[] Services { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }
    }
}