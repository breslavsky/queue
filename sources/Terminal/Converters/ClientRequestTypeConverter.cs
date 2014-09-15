using System;
using System.Windows.Data;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Terminal.Converters
{
    public class ClientRequestTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Translation.ClientRequestType.ResourceManager.GetString(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}