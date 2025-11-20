using System;
using System.Globalization;
using System.Windows.Data;
using CoffeeMachineWPF.Models;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация перечисления типа кофе в локализованное строковое представление и обратно
    /// </summary>
    public class CoffeeTypeConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование значения перечисления CoffeeType в локализованное строковое представление
        /// </summary>
        /// <param name="value">Значение перечисления CoffeeType для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается string)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Локализованное название типа кофе или пустая строка при ошибке</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CoffeeType coffeeType)
            {
                return coffeeType switch
                {
                    CoffeeType.Espresso => "Эспрессо",
                    CoffeeType.Americano => "Американо",
                    CoffeeType.Cappuccino => "Капучино",
                    CoffeeType.Latte => "Латте",
                    _ => coffeeType.ToString()
                };
            }
            return string.Empty;
        }

        /// <summary>
        /// Преобразование строкового представления типа кофе обратно в значение перечисления CoffeeType
        /// </summary>
        /// <param name="value">Строковое представление типа кофе для обратного преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается CoffeeType)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Значение перечисления CoffeeType, соответствующее строке, или Espresso по умолчанию</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str switch
                {
                    "Эспрессо" => CoffeeType.Espresso,
                    "Американо" => CoffeeType.Americano,
                    "Капучино" => CoffeeType.Cappuccino,
                    "Латте" => CoffeeType.Latte,
                    _ => CoffeeType.Espresso
                };
            }
            return CoffeeType.Espresso;
        }
    }
}