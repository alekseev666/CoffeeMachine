using CoffeeMachineWPF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация типа обслуживания в признак глубокой очистки
    /// </summary>
    public class IsDeepCleaningConverter : IValueConverter
    {
        /// <summary>
        /// Проверка, является ли тип обслуживания глубокой очисткой
        /// </summary>
        /// <param name="value">Тип обслуживания для проверки</param>
        /// <param name="targetType">Целевой тип (ожидается bool)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>true, если тип обслуживания - DeepCleaning, иначе false</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MaintenanceType maintenanceType)
            {
                return maintenanceType == MaintenanceType.DeepCleaning;
            }
            return false;
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}