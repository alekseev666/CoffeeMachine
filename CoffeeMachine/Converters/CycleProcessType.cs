using System;
using System.Globalization;
using System.Windows.Data;
using CoffeeMachineWPF.ViewModels;

namespace CoffeeMachineWPF.Converters
{
    public class CycleProcessTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CycleProcessType processType)
            {
                return processType switch
                {
                    CycleProcessType.WaterHeating => "Нагрев воды",
                    CycleProcessType.TankCleaning => "Очистка баков",
                    CycleProcessType.DispenserTesting => "Тестирование дозаторов",
                    _ => value.ToString()
                };
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue switch
                {
                    "Нагрев воды" => CycleProcessType.WaterHeating,
                    "Очистка баков" => CycleProcessType.TankCleaning,
                    "Тестирование дозаторов" => CycleProcessType.DispenserTesting,
                    _ => CycleProcessType.WaterHeating
                };
            }
            return CycleProcessType.WaterHeating;
        }
    }
}