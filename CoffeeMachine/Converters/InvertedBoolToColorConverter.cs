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
    /// Конвертация логического значения в инвертированную цветовую схему
    /// </summary>
    public class InvertedBoolToColorConverter : IValueConverter
    {

        /// <summary>
        /// Преобразование логического значения в цветную кисть с инвертированной логикой
        /// </summary>
        /// <param name="value">Логическое значение для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается Brush)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Зеленая кисть для true, красная для false, серая для некорректных значений</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Brushes.Green : Brushes.Red;
            return Brushes.Gray;
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
