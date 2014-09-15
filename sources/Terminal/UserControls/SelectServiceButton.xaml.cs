using System.Windows;
using System.Windows.Controls;

namespace Queue.Terminal.UserControls
{
    public partial class SelectServiceButton : UserControl
    {
        private const int MinFontSize = 5;

        public SelectServiceButton()
        {
            InitializeComponent();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            FrameworkElement parent = (FrameworkElement)tb.Parent;

            while (tb.ActualHeight > parent.ActualHeight & tb.FontSize > MinFontSize)
            {
                tb.FontSize -= 1;
                tb.UpdateLayout();
            }
        }
    }
}