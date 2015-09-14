using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class PrintCouponPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(PrintCouponPageViewModel); } }

        public PrintCouponPage()
            : base()
        {
            InitializeComponent();
        }
    }
}