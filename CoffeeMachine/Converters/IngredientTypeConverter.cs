using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters;
/// <summary>
/// Конвертация перечисления типа ингредиента в локализованное строковое представление
/// </summary>
public class IngredientTypeConverter : IValueConverter
{
    /// <summary>
    /// Преобразование значения перечисления IngredientType в локализованное строковое представление
    /// </summary>
    /// <param name="value">Значение перечисления IngredientType для преобразования</param>
    /// <param name="targetType">Целевой тип (ожидается string)</param>
    /// <param name="parameter">Дополнительный параметр</param>
    /// <param name="culture">Культура для преобразования</param>
    /// <returns>Локализованное название ингредиента или строковое представление значения</returns>
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

    /// <summary>
    /// Обратное преобразование не поддерживается
    /// </summary>
    /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
