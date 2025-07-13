using System;
using System.Globalization;
using Xamarin.Forms;

namespace Car_Rentals.Converters
{
    public class UpcomingStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string status && status == "Upcoming";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 