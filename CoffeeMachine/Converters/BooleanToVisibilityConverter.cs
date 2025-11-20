using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Конвертация логического значения в значение видимости элемента
/// </summary>
namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Преобразование логического значения в значение видимости
    /// </summary>
    /// <param name="value">Логическое значение для преобразования</param>
    /// <param name="targetType">Целевой тип (ожидается Visibility)</param>
    /// <param name="parameter">Дополнительный параметр</param>
    /// <param name="culture">Культура для преобразования</param>
    /// <returns>Visibility.Visible для true, Visibility.Collapsed для false</returns>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Преобразование значения видимости обратно в логическое значение
        /// </summary>
        /// <param name="value">Значение видимости для обратного преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается bool)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>true если Visibility.Visible, false в других случаях</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}