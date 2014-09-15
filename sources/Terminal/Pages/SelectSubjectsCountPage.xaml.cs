using Queue.Terminal.Models.Pages;
using System;

namespace Queue.Terminal.Pages
{
    public partial class SelectSubjectsCountPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectSubjectsCountPageVM); } }

        public SelectSubjectsCountPage()
            : base()
        {
            InitializeComponent();
        }

        private void TerminalPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SelectSubjectsCountPageVM).Initialize();
        }
    }
}