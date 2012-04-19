using System;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            string check = (string)parameter;
            if (string.IsNullOrEmpty(check))
            {
                return !availability.Equals(Constants.AvailabilityTLD);
            }
            else
            {
                return availability.Equals(Constants.AvailabilityAvailable) || availability.Equals(Constants.AvailabilityMaybe);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
