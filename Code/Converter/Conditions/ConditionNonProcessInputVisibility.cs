using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StorecfgGenerator
{
    public class ConditionNonProcessInputVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str))
                return Binding.DoNothing;
            return str == "PROCESS" || str == "WAIT" ? (object)Visibility.Collapsed : (object)Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


