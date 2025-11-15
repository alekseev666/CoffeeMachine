using System;
using System.Globalization;
using System.Windows.Data;
using CoffeeMachineWPF.Models;

namespace CoffeeMachineWPF.Converters
{
    public class CoffeeTypeConverter : IValueConverter
    {
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