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
    /// <summary>
    /// Конвертация типа обслуживания в текстовое сообщение о эффектах
    /// </summary>
    public class MaintenanceEffectConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование типа обслуживания в описание его эффектов
        /// </summary>
        /// <param name="value">Тип обслуживания для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается string)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Описание эффектов выбранного типа обслуживания или сообщение по умолчанию</returns>
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

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
