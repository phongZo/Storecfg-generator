using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace StorecfgGenerator
{
    public class StringToTagsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return new ObservableCollection<string>(str.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));
            }
            return new ObservableCollection<string>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> tags)
            {
                return string.Join("|", tags.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));
            }
            return string.Empty;
        }
    }
}


