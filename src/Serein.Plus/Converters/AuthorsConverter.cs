using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Serein.Core.Models.Plugins.Info;

namespace Serein.Plus.Converters;

public class AuthorsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Author[] authors
            ? string.Join(',', authors.Select(author => author.Name))
            : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
