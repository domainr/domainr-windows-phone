using System;
using System.Windows;
using System.Windows.Data;
using DomainrSharp.WindowsPhone;

namespace Domainr7.Converters
{
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DomainrInfo info = (DomainrInfo)value;
            return info == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
