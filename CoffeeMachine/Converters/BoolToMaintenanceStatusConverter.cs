using System;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    public class BoolToMaintenanceStatusConverter : IValueConverter
    {
        public static BoolToMaintenanceStatusConverter Instance { get; } = new BoolToMaintenanceStatusConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBroken)
            {
                return isBroken ? "СЛОМАНА (требует ремонта)" : "ИСПРАВНА";
            }
            return "НЕИЗВЕСТНО";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}