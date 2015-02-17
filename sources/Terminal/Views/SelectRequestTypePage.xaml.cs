using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class SelectRequestTypePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectRequestTypePageVM); } }

        public SelectRequestTypePage()
            : base()
        {
            InitializeComponent();
        }

        private void TerminalPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SelectRequestTypePageVM).Initialize();
        }
    }
}