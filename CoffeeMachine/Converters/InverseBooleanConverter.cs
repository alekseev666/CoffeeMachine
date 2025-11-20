using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация логического значения в его инвертированное значение
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Инвертация логического значения
        /// </summary>
        /// <param name="value">Логическое значение для инвертирования</param>
        /// <param name="targetType">Целевой тип (ожидается bool)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Инвертированное логическое значение</returns>
        /// <exception cref="InvalidCastException">Выбрасывается, если value не является bool</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        /// <summary>
        /// Инвертация логического значение 
        /// </summary>
        /// <param name="value">Логическое значение для инвертирования</param>
        /// <param name="targetType">Целевой тип (ожидается bool)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Инвертированное логическое значение</returns>
        /// <exception cref="InvalidCastException">Выбрасывается, если value не является bool</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;
    }
}
