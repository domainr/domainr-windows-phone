using System;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityReplaceTLDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availabilty = (string)value;
            return availabilty.Equals(Constants.AvailabilityTLD) ? "invalid domain" : availabilty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
