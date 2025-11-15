using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters;
public class IngredientTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            IngredientType.Water => "Вода",
            IngredientType.Coffee => "Кофе",
            IngredientType.Milk => "Молоко",
            IngredientType.Cups => "Стаканчики",
            IngredientType.Sugar => "Сахар",
            _ => value?.ToString()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
