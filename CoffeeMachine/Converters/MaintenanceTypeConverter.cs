using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters;
public class MaintenanceTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            MaintenanceType.Cleaning => "Очистка баков",
            MaintenanceType.DeepCleaning => "Глубокая очистка",
            MaintenanceType.Calibration => "Калибровка дозаторов",
            MaintenanceType.Diagnostic => "Полная диагностика",
            _ => value?.ToString()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}