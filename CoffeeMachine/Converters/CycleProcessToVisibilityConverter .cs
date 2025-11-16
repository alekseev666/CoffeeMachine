using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CoffeeMachineWPF.ViewModels;

namespace CoffeeMachineWPF.Converters
{
    public class CycleProcessToVisibilityConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}