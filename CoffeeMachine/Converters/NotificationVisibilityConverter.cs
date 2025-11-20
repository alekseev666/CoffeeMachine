using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация уведомления в видимость элемента на основе типа уведомления
    /// </summary>
    public class NotificationVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование уведомления в видимость элемента в зависимости от типа уведомления
        /// </summary>
        /// <param name="value">Текстовое уведомление для проверки</param>
        /// <param name="targetType">Целевой тип (ожидается Visibility)</param>
        /// <param name="parameter">Тип уведомления для фильтрации (например, "CRITICAL", "WARNING", "INFO")</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Visibility.Visible, если уведомление начинается с указанного типа, иначе Visibility.Collapsed</returns
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string notification && parameter is string type)
            {
                return notification.StartsWith(type) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
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
