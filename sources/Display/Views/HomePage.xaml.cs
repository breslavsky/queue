using Queue.UI.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Display.Views
{
    public partial class HomePage : RichPage
    {
        private const int MinFontSize = 5;

        public HomePage()
            : base()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.None;
        }

        private void OnCommentTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBlock;
            var parent = tb.Parent as FrameworkElement;

            while (tb.ActualHeight > parent.ActualHeight & tb.FontSize > MinFontSize)
            {
                tb.FontSize -= 1;
                tb.UpdateLayout();
            }
        }
    }
}