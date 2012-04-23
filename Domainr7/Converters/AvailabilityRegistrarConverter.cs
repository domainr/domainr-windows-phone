using System;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityRegistrarConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            if (availability.Equals(Constants.AvailabilityAvailable) ||
                availability.Equals(Constants.AvailabilityMaybe))
            {
                return "register";
            }
            else
            {
                return "domain for sale?";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
