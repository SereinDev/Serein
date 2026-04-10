using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Network.Web.WebAuthentication;

namespace Serein.Gui.Converters;

public sealed class WebAuthenticationValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            TokenAuthentication token => string.IsNullOrEmpty(token.Token)
                ? string.Empty
                : new('*', token.Token.Length),
            UserAuthentication user => user.Username,
            _ => string.Empty,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
