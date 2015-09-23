using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Resources;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.ViewModels;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Queue.Terminal
{
    public partial class TerminalWindow : RichPage
    {
        private const string PageFrameName = "pageFrame";

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        public TerminalWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var rootObject = GetTerminalPageContent();

                var pageFrame = LogicalTreeHelper.FindLogicalNode(rootObject, PageFrameName) as Frame;
                if (pageFrame == null)
                {
                    throw new QueueException("Элемент \"pageFrame\" не найден или тип элемента с данным именем не Frame");
                }

                pageFrame.Navigated += (s, args) => pageFrame.NavigationService.RemoveBackEntry();

                Content = rootObject;

                ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

                DataContext = UnityContainer.Resolve<TerminalWindowViewModel>();

                Navigator.SetNavigationService(pageFrame.NavigationService);

                (DataContext as TerminalWindowViewModel).Initialize();
            }
            catch (Exception ex)
            {
                UIHelper.Error(null, ex);
            }
        }

        private DependencyObject GetTerminalPageContent()
        {
            try
            {
                var config = ServiceLocator.Current.GetInstance<TerminalConfig>();
                string template = String.IsNullOrEmpty(config.WindowTemplate) ? Templates.TerminalWindow : config.WindowTemplate;
                return XamlReader.Parse(template) as DependencyObject;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Невалидная разметка окна терминала", e);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as TerminalWindowViewModel).Dispose();
        }
    }
}