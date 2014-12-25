using System;
using System.Globalization;
using System.Windows.Data;

namespace Queue.Terminal.Converters
{
    public class RequestDateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
            {
                return Binding.DoNothing;
            }

            DateTime date = (DateTime)values[0];
            TimeSpan time = (TimeSpan)values[1];
            //TODO бееее,но по другому не получилось(((
            return string.Format("{0:dd.MM.yyyy} {1:00}:{2:00}", date, time.Hours, time.Minutes);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}