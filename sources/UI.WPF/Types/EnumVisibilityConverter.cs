using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Queue.UI.WPF
{
    public class EnumVisibilityConverter : IValueConverter
    {
        public const char Delimiter = ',';

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is Enum))
            {
                return Visibility.Collapsed;
            }

            var currentFlags = value.ToString()
                .Split(Delimiter);
            var findFlags = parameter.ToString()
                .Split(Delimiter);

            var finded = currentFlags.Any(x => findFlags.Any(f => f.Trim() == x.Trim()));
            return finded ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion IValueConverter Members
    }
}