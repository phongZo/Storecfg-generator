using System;
using System.Globalization;
using System.Windows.Data;

namespace StorecfgGenerator
{
    public class MillisecondsToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert milliseconds to seconds for display
            if (value is int milliseconds)
            {
                return (double)milliseconds / 1000.0;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert seconds back to milliseconds for storage
            if (value is string stringValue && double.TryParse(stringValue, out double seconds))
            {
                return (int)(seconds * 1000);
            }
            return value;
        }
    }
}


