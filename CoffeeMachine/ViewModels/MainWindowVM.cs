using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        private IOperationViewModel _selectedOperation;

        public ObservableCollection<IOperationViewModel> Operations { get; }

        public MainWindowVM()
        {
            var coffeeMachine = new CoffeeMachine();

            Operations = new ObservableCollection<IOperationViewModel>
            {
                new MakeCoffeeVM(coffeeMachine),
                new AdditionIngredientsVM(coffeeMachine),
                new MaintenanceServiceVM(coffeeMachine)

            };

            SelectedOperation = Operations.FirstOrDefault();
        }
    }
}