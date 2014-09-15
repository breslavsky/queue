using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.Models;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Queue.Terminal
{
    public partial class TerminalWindow : RichPage
    {
        private const string PageFrameName = "pageFrame";

        private const string DefaultMarkup = "<Border  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
        "<Grid>" +
            "<Grid>" +
                "<Grid.RowDefinitions>" +
                    "<RowDefinition Height=\"auto\" />" +
                    "<RowDefinition Height=\"auto\" />" +
                    "<RowDefinition />" +
                "</Grid.RowDefinitions>" +
                "<Grid>" +
                    "<Grid.ColumnDefinitions>" +
                        "<ColumnDefinition Width=\"auto\" />" +
                        "<ColumnDefinition />" +
                        "<ColumnDefinition Width=\"200\" />" +
                    "</Grid.ColumnDefinitions>" +
                    "<StackPanel Orientation=\"Horizontal\">" +
                        "<Button Height=\"80\" Margin=\"5\" VerticalAlignment=\"Top\" BorderThickness=\"4,4,4,4\" Command=\"{Binding HomeCommand}\">" +
                            "<StackPanel Margin=\"5,0,0,0\" Orientation=\"Horizontal\">" +
                                "<Rectangle Width=\"50\" Height=\"40\">" +
                                    "<Rectangle.Fill>" +
                                        "<VisualBrush Stretch=\"Fill\" Visual=\"{StaticResource appbar_home}\" />" +
                                    "</Rectangle.Fill>" +
                                "</Rectangle>" +
                                "<TextBlock Margin=\"10\" VerticalAlignment=\"Center\" FontSize=\"25\" FontWeight=\"Bold\" Text=\"Выбор услуг\" />" +
                            "</StackPanel>" +
                        "</Button>" +
                        "<Button Height=\"80\" Margin=\"5\" VerticalAlignment=\"Top\" BorderThickness=\"4,4,4,4\" Command=\"{Binding SearchServiceCommand}\">" +
                            "<StackPanel Margin=\"5,0,0,0\" Orientation=\"Horizontal\">" +
                                "<Rectangle Width=\"40\" Height=\"40\">" +
                                    "<Rectangle.Fill>" +
                                        "<VisualBrush Stretch=\"Fill\" Visual=\"{StaticResource appbar_magnify}\" />" +
                                    "</Rectangle.Fill>" +
                                "</Rectangle>" +
                                "<TextBlock Margin=\"10\" VerticalAlignment=\"Center\" FontSize=\"25\" FontWeight=\"Bold\" Text=\"Поиск\" />" +
                            "</StackPanel>" +
                        "</Button>" +
                    "</StackPanel>" +

                    "<StackPanel Grid.Column=\"2\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Top\" Orientation=\"Horizontal\">" +
                        "<Border Width=\"25\" Height=\"25\" Margin=\"5\" HorizontalAlignment=\"Right\">" +
                            "<Border.Style>" +
                                "<Style>" +
                                    "<Style.Triggers>" +
                                        "<DataTrigger Binding=\"{Binding ServerState}\" Value=\"Request\">" +
                                            "<Setter Property=\"Border.Background\" Value=\"Yellow\" />" +
                                        "</DataTrigger>" +
                                        "<DataTrigger Binding=\"{Binding ServerState}\" Value=\"Available\">" +
                                            "<Setter Property=\"Border.Background\" Value=\"Green\" />" +
                                        "</DataTrigger>" +
                                        "<DataTrigger Binding=\"{Binding ServerState}\" Value=\"Unavailable\">" +
                                            "<Setter Property=\"Border.Background\" Value=\"Red\" />" +
                                        "</DataTrigger>" +
                                    "</Style.Triggers>" +
                                "</Style>" +
                            "</Border.Style>" +
                        "</Border>" +
                        "<TextBlock FontSize=\"30\" FontWeight=\"Bold\" Padding=\"5\" Text=\"{Binding CurrentDateTime, StringFormat=HH:mm:ss}\" />" +
                    "</StackPanel>" +
                "</Grid>" +
                "<TextBlock Grid.Row=\"1\" Margin=\"50,20,50,0\" TextAlignment=\"Center\" VerticalAlignment=\"Center\" FontSize=\"25\" FontWeight=\"Bold\" Text=\"{Binding Title}\" TextWrapping=\"Wrap\" />" +

                "<Viewbox Grid.Row=\"2\" Grid.Column=\"1\" Margin=\"5\" Stretch=\"Uniform\">" +
                    "<Grid Width=\"540\" Height=\"400\">" +
                        "<Frame x:Name=\"pageFrame\" NavigationUIVisibility=\"Hidden\" />" +
                    "</Grid>" +
                "</Viewbox>" +
            "</Grid>" +
        "</Grid>" +
    "</Border>";

        public TerminalWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DependencyObject rootObject = GetTerminalPageContent();

                Frame pageFrame = LogicalTreeHelper.FindLogicalNode(rootObject, PageFrameName) as Frame;

                if (pageFrame == null)
                {
                    throw new ApplicationException("Элемент \"pageFrame\" не найден или тип элемента с данным именем не Frame");
                }

                Content = rootObject;

                ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterInstance<IRichPage>(this);
                DataContext = new TerminalWindowVM();

                ServiceLocator.Current.GetInstance<Navigator>().SetNavigationService(pageFrame.NavigationService);

                (DataContext as TerminalWindowVM).Initialize();
            }
            catch (Exception ex)
            {
                UIHelper.Error(null, ex);
            }
        }

        private DependencyObject GetTerminalPageContent()
        {
            ServiceLocator.Current.GetInstance<DefaultConfig>();
            try
            {
                return XamlReader.Parse(DefaultMarkup) as DependencyObject;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Невалидная разметка окна терминала", e);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as TerminalWindowVM).Dispose();
        }
    }
}