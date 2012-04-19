using System;
using System.Windows;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            return availability.Equals(Constants.AvailabilityTLD) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
