using Queue.Terminal.ViewModels;
using System;

namespace Queue.Terminal.Views
{
    public partial class SelectSubjectsPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectSubjectsPageViewModel); } }

        public SelectSubjectsPage()
            : base()
        {
            InitializeComponent();
        }

        private void TerminalPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SelectSubjectsPageViewModel).Initialize();
        }
    }
}