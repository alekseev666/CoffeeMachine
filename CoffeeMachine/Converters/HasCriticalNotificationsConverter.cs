using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация наличия критических уведомлений в видимость элемента
    /// </summary>
    public class HasCriticalNotificationsConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование список уведомлений в видимость элемента
        /// </summary>
        /// <param name="value">Список уведомлений для проверки</param>
        /// <param name="targetType">Целевой тип (ожидается Visibility)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Visibility.Collapsed если есть критические уведомления, иначе Visibility.Visible</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.Generic.List<string> notifications)
            {
                return notifications.Any(n => n.StartsWith("CRITICAL")) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
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
