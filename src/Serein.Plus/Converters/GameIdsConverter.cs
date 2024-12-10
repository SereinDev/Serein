using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class GameIdsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is List<string> gameIds
            ? string.Join(",", gameIds)
            : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
