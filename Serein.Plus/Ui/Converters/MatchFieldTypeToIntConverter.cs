using System;
using System.Globalization;
using System.Windows.Data;

using Serein.Core.Models.Commands;

namespace Serein.Plus.Ui.Converters;

public class MatchFieldTypeToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToInt32(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is int i && i < 6 && i >= 0 ? (MatchFieldType)i : MatchFieldType.Disabled;
    }
}
