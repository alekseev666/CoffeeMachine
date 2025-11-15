using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    public class MaintenanceTypeDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MaintenanceType.Cleaning => "Стандартная очистка баков от кофейных отходов",
                MaintenanceType.DeepCleaning => "Полная промывка системы с моющими средствами",
                MaintenanceType.Calibration => "Настройка точности дозаторов воды и кофе",
                MaintenanceType.Diagnostic => "Проверка всех систем и датчиков машины",
                _ => "Выберите тип обслуживания"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
