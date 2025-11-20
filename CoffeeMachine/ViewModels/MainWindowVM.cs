using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        private IOperationViewModel _selectedOperation;

        public ObservableCollection<IOperationViewModel> Operations { get; }

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