using System;
using System.Globalization;
using System.Windows.Data;

namespace StorecfgGenerator
{
    public class IntOrNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.ToString() ?? string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (value as string)?.Trim();
            if (string.IsNullOrEmpty(s)) return null;
            if (int.TryParse(s, out var n)) return n;
            return Binding.DoNothing;
        }
    }
}
