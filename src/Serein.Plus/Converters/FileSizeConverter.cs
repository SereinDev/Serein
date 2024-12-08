using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Utils.Extensions;

namespace Serein.Plus.Converters;

public class FileSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is long l ? l.ToSizeString() : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
