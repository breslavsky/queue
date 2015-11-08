using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class SelectRequestTypePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectRequestTypePageViewModel); } }

        public SelectRequestTypePage()
            : base()
        {
            InitializeComponent();
        }
    }
}