using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CoffeeMachineWPF.ViewModels;

namespace CoffeeMachineWPF.Converters
{
    /// <summary>
    /// Конвертация типа циклического процесса в видимость элемента на основе параметра
    /// </summary>
    public class CycleProcessToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование типа циклического процесса в видимость элемента
        /// </summary>
        /// <param name="value">Текущий выбранный тип процесса (CycleProcessType)</param>
        /// <param name="targetType">Целевой тип (ожидается Visibility)</param>
        /// <param name="parameter">Имя целевого типа процесса для сравнения (строка)</param>
        /// <param name="culture">Культура для преобразования</param>
        /// <returns>Visibility.Visible если выбранный процесс совпадает с целевым, иначе Visibility.Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CycleProcessType selectedProcess && parameter is string targetProcess)
            {
                if (Enum.TryParse<CycleProcessType>(targetProcess, out var targetProcessType))
                {
                    return selectedProcess == targetProcessType ? Visibility.Visible : Visibility.Collapsed;
                }
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