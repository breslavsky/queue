using Queue.Common;
using Queue.Model.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Queue.Terminal.Converters
{
    public class ClientRequestTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Translater.Enum((ClientRequestType)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}