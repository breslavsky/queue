using Queue.Terminal.Models.Pages;
using System;
using System.Windows;

namespace Queue.Terminal.Pages
{
    public partial class SelectDateTimeShedulerPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SelectDateTimeShedulerPageVM); } }

        private SelectDateTimeShedulerPageVM model;

        public SelectDateTimeShedulerPage()
        {
            InitializeComponent();

            model = DataContext as SelectDateTimeShedulerPageVM;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            model.Initialize(scheduler);
        }
    }
}