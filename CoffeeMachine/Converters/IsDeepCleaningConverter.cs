
using CoffeeMachineWPF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeMachineWPF.Converters
{
    public class IsDeepCleaningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MaintenanceType maintenanceType)
            {
                return maintenanceType == MaintenanceType.DeepCleaning;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}