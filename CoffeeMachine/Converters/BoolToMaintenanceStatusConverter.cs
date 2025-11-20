using System;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация логического значения статуса кофемашины в текстовое описание
    /// </summary>
    public class BoolToMaintenanceStatusConverter : IValueConverter
    {
        /// <summary>
        /// Экземпляр конвертера для переиспользования 
        /// </summary>
        public static BoolToMaintenanceStatusConverter Instance { get; } = new BoolToMaintenanceStatusConverter();

        /// <summary>
        /// Преобразование логического значения статуса поломки в текстовое описание
        /// </summary>
        /// <param name="value">Логическое значение статуса поломки</param>
        /// <param name="targetType">Целевой тип (ожидается string)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Текстовое описание статуса: "СЛОМАНА (требует ремонта)", "ИСПРАВНА" или "НЕИЗВЕСТНО"</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBroken)
            {
                return isBroken ? "СЛОМАНА (требует ремонта)" : "ИСПРАВНА";
            }
            return "НЕИЗВЕСТНО";
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}