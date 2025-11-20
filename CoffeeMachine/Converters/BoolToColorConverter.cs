using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация логического значения в цвет кисти
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование логического значения в цветную кисть
        /// </summary>
        /// <param name="value">Логическое значение для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается Brush)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Красная кисть для true, зеленая кисть для false</returns>
        /// <exception cref="InvalidCastException">Исключение, если value не является bool</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
