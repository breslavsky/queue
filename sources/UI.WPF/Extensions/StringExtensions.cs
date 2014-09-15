using System.Windows.Media;
using Drawing = System.Drawing;

namespace Queue.UI.WPF.Extensions
{
    public static class StringExtensions
    {
        public static SolidColorBrush GetBrushForColor(this string color)
        {
            Drawing.Color c = Drawing.ColorTranslator.FromHtml(color);
            return new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }
    }
}