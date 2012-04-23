using System;
using System.Windows.Data;
using System.Windows.Media;
using Domainr7.Model;

namespace Domainr7.Converters
{
    public class AvailabilityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string availability = (string)value;
            Color color = Colors.Green;
            switch (availability)
            {
                case Constants.AvailabilityAvailable:
                    color = Colors.Green;
                    break;
                case Constants.AvailabilityMaybe:
                    color = Colors.Yellow;
                    break;
                case Constants.AvailabilityTaken:
                    color = Colors.Transparent;
                    break;
                case Constants.AvailabilityUnavailable:
                    color = Colors.Transparent;
                    break;
                case Constants.AvailabilityTLD:
                    color = Colors.Transparent;
                    break;
                default:
                    color = Colors.Transparent;
                    break;
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
