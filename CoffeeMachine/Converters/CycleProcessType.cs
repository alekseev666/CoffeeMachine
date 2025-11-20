using System;
using System.Globalization;
using System.Windows.Data;
using CoffeeMachineWPF.ViewModels;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация перечисления типа циклического процесса в локализованное строковое представление и обратно
    /// </summary>
    public class CycleProcessTypeConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование значения перечисления CycleProcessType в локализованное строковое представление
        /// </summary>
        /// <param name="value">Значение перечисления CycleProcessType для преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается string)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Локализованное название типа процесса или строковое представление значения</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CycleProcessType processType)
            {
                return processType switch
                {
                    CycleProcessType.WaterHeating => "Нагрев воды",
                    CycleProcessType.TankCleaning => "Очистка баков",
                    CycleProcessType.DispenserTesting => "Тестирование дозаторов",
                    _ => value.ToString()
                };
            }
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Преобразование строкового представления типа процесса обратно в значение перечисления CycleProcessType
        /// </summary>
        /// <param name="value">Строковое представление типа процесса для обратного преобразования</param>
        /// <param name="targetType">Целевой тип (ожидается CycleProcessType)</param>
        /// <param name="parameter">Дополнительный параметр</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Значение перечисления CycleProcessType, соответствующее строке, или WaterHeating по умолчанию</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue switch
                {
                    "Нагрев воды" => CycleProcessType.WaterHeating,
                    "Очистка баков" => CycleProcessType.TankCleaning,
                    "Тестирование дозаторов" => CycleProcessType.DispenserTesting,
                    _ => CycleProcessType.WaterHeating
                };
            }
            return CycleProcessType.WaterHeating;
        }
    }
}