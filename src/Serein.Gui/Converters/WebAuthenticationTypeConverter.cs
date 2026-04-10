using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Network.Web.WebAuthentication;

namespace Serein.Gui.Converters;

public sealed class WebAuthenticationTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            TokenAuthentication => "令牌",
            UserAuthentication => "用户",
            _ => "未知",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
