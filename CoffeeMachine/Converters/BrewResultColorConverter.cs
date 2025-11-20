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
    /// Конвертация текстового сообщения о результате приготовления кофе в цвет
    /// </summary>
    public class BrewResultColorConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование текстового сообщения о результате приготовления в цвет кисти
        /// </summary>
        /// <param name="value">Текстовое сообщение о результате приготовления</param>
        /// <param name="targetType">Целевой тип (ожидается Brush)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Зеленая кисть для успеха, красная для ошибок, серая для неизвестного статуса</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string message)
            {
                return message switch
                {
                    "Кофе готов!" => Brushes.Green,
                    _ => Brushes.Red
                };
            }
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
