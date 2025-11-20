using CoffeeMachineWPF.Models;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация типа обслуживания в текстовое описание процедуры
    /// </summary>
    public class MaintenanceTypeDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование типа обслуживания в детальное описание процедуры
        /// </summary>
        /// <param name="value">Тип обслуживания для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается string)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Описание процедуры обслуживания или сообщение по умолчанию</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MaintenanceType.Cleaning => "Стандартная очистка баков от кофейных отходов",
                MaintenanceType.DeepCleaning => "Полная промывка системы с моющими средствами",
                MaintenanceType.Calibration => "Настройка точности дозаторов воды и кофе",
                MaintenanceType.Diagnostic => "Проверка всех систем и датчиков машины",
                _ => "Выберите тип обслуживания"
            };
        }

        /// <summary>
        /// Обратное преобразование не поддерживается
        /// </summary>
        /// <exception cref="NotImplementedException">Всегда выбрасывается исключение</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
