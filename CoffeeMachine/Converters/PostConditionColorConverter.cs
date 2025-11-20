using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация результата проверки постусловия в цветовую индикацию
    /// </summary>
    public class PostConditionColorConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование результата проверки постусловия в цветную кисть
        /// </summary>
        /// <param name="value">Результат проверки постусловия для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается Brush)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Зеленая кисть для выполненных условий, красная для невыполненных, серая для некорректных значений</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            return new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
