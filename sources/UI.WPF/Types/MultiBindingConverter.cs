using System;
using System.Globalization;
using System.Windows.Data;

namespace Queue.UI.WPF
{
    public class MultiBindingConverter : IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format(parameter.ToString(), values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter
    }
}