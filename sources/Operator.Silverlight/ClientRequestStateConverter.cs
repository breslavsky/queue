using System;
using System.Globalization;
using System.Windows.Data;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Operator.Silverlight
{
    public class ClientRequestStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var translation = Translation.ClientRequestState.ResourceManager;
            return translation.GetString(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}