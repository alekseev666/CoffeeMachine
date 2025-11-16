using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    public class HasInfoNotificationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.Generic.List<string> notifications)
            {
                return notifications.Any(n => n.StartsWith("INFO")) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
