using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Главная модель представления окна приложения кофемашины
    /// </summary>
    public partial class MainWindowVM : ObservableObject
    {
        /// <summary>
        /// Выбранная операция кофемашины
        /// </summary>
        [ObservableProperty]
        private IOperationViewModel _selectedOperation;

        /// <summary>
        /// Коллекция доступных операций кофемашины
        /// </summary>
        public ObservableCollection<IOperationViewModel> Operations { get; }

        /// <summary>
        /// Создание главной модели представления с доступными операциями
        /// </summary>
        /// <param name="makeCoffeeVM">Модель представления приготовления кофе</param>
        /// <param name="additionIngredientsVM">Модель представления пополнения ингредиентов</param>
        /// <param name="maintenanceServiceVM">Модель представления технического обслуживания</param>
        /// <param name="cycleAnalysisVM">Модель представления анализа циклов</param>
        /// <param name="stateControllerVM">Модель представления контроллера состояния</param>
        public MainWindowVM(
            MakeCoffeeVM makeCoffeeVM,
            AdditionIngredientsVM additionIngredientsVM,
            MaintenanceServiceVM maintenanceServiceVM,
            CycleAnalysisVM cycleAnalysisVM,
            StateControllerVM stateControllerVM,
            AppInfoVM appInfoVM)
        {
            Operations = new ObservableCollection<IOperationViewModel>
            {
                
                makeCoffeeVM,
                additionIngredientsVM,
                maintenanceServiceVM,
                cycleAnalysisVM,
                stateControllerVM,
                appInfoVM
            };

            SelectedOperation = Operations.FirstOrDefault();
        }
    }
}