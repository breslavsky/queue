using Queue.Terminal.UserControls;
using System;
using System.Collections.Generic;

namespace Queue.Terminal
{
    public class RenderServicesEventArgs : EventArgs
    {
        public List<SelectServiceButton> Services { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }
    }
}