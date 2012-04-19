using System;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            string check = (string)parameter;
            if (string.IsNullOrEmpty(check))
            {
                return availability.Equals(Constants.AvailabilityTLD) ? 0.6 : 1;
            }
            else
            {
                return availability.Equals(Constants.AvailabilityAvailable) || availability.Equals(Constants.AvailabilityMaybe) ? 1 : 0.6;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
