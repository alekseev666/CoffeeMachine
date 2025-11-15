using CoffeeMachineWPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeMachineWPF.Converters
{
    // MaintenanceEffectConverter.cs
    public class MaintenanceEffectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MaintenanceType.Cleaning => "Стандартная очистка уменьшит износ на 15% и очистит баки",
                MaintenanceType.DeepCleaning => "Глубокая очистка уменьшит износ на 30% и полностью прочистит систему",
                MaintenanceType.Calibration => "Калибровка уменьшит износ на 20% и улучшит точность дозаторов",
                MaintenanceType.Diagnostic => "Диагностика уменьшит износ на 10% и проверит все системы",
                _ => "Выберите тип обслуживания"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
