using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Terminal.Core;
using Queue.Terminal.ViewModels;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.Terminal
{
    public partial class TerminalWindow : RichPage
    {
        private const string PageFrameName = "pageFrame";
        private TerminalWindowViewModel model;

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public ITemplateManager TemplateManager { get; set; }

        public TerminalWindow()
            : base()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var rootObject = TemplateManager.GetTemplate("main-page.xaml");

                var pageFrame = LogicalTreeHelper.FindLogicalNode(rootObject, PageFrameName) as Frame;
                if (pageFrame == null)
                {
                    throw new QueueException("Элемент \"pageFrame\" не найден или тип элемента с данным именем не Frame");
                }

                pageFrame.Navigated += (s, args) => pageFrame.NavigationService.RemoveBackEntry();

                Content = rootObject;

                ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

                model = new TerminalWindowViewModel();

                DataContext = model;

                Navigator.SetNavigationService(pageFrame.NavigationService);

                model.Initialize();
            }
            catch (Exception ex)
            {
                UIHelper.Error(null, ex);
            }
        }
    }
}