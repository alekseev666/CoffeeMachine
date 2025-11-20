using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters;
/// <summary>
/// Конвертация перечисления типа обслуживания в локализованное строковое представление
/// </summary>
public class MaintenanceTypeConverter : IValueConverter
{
    /// <summary>
    /// Преобразование значения перечисления MaintenanceType в локализованное строковое представление
    /// </summary>
    /// <param name="value">Значение перечисления MaintenanceType для преобразования</param>
    /// <param name="targetType">Целевой тип (ожидается string)</param>
    /// <param name="parameter">Дополнительный параметр</param>
    /// <param name="culture">Культура для преобразования</param>
    /// <returns>Локализованное название типа обслуживания или строковое представление значения</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            MaintenanceType.Cleaning => "Очистка баков",
            MaintenanceType.DeepCleaning => "Глубокая очистка",
            MaintenanceType.Calibration => "Калибровка дозаторов",
            MaintenanceType.Diagnostic => "Полная диагностика",
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