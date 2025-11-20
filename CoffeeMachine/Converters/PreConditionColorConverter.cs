using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация результата проверки предусловия в цветовую индикацию
    /// </summary>
    public class PreConditionColorConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование результата проверки предусловия в цветную кисть
        /// </summary>
        /// <param name="value">Результат проверки предусловия для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается Brush)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Зеленая кисть для выполненных условий, красная для невыполненных</returns>
        /// <exception cref="InvalidCastException">Исключение, если value не является bool</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
