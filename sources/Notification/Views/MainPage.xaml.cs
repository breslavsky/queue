using Junte.UI.WPF;
using Microsoft.Practices.Unity;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace Queue.Notification.Views
{
    public partial class MainPage : RichPage
    {
        [Dependency]
        public ITemplateManager TemplateManager { get; set; }

        public MainPage()
            : base()
        {
            InitializeComponent();

            try
            {
                Content = TemplateManager.GetTemplate("main-page.xaml");
            }
            catch (Exception e)
            {
                UIHelper.Error(null, String.Format("Не удалось инициализировать главную страницу: {0}", e.Message));
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Mouse.OverrideCursor = Cursors.None;
        }
    }
}