using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    public class NotificationVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string notification && parameter is string type)
            {
                return notification.StartsWith(type) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
