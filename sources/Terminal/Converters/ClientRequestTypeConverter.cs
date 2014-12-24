using Queue.Model.Common;
using System;
using System.Windows.Data;

namespace Queue.Terminal.Converters
{
    public class ClientRequestTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Translate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}