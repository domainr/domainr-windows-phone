using System;
using System.Windows.Data;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityStringFormat : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            string retVal = "";
            switch (availability)
            {
                case Constants.AvailabilityAvailable:
                    retVal = "This domain is available";
                    break;
                case Constants.AvailabilityMaybe:
                    retVal = "This domain might be available";
                    break;
                case Constants.AvailabilityTaken:
                    retVal = "This domain is already taken";
                    break;
                case Constants.AvailabilityUnavailable:
                    retVal = "This domain is unavailable";
                    break;
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
