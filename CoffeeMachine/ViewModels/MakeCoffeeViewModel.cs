using CoffeeMachineWPF.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class MakeCoffeeViewModel : ObservableObject
    {
        [ObservableProperty]
        private CoffeeType _selectedCoffeeType;

        [ObservableProperty] // Сколько добавить сахара
        private int _sugarLevel;

        [ObservableProperty] // Типа чекбокса, опциональный выбор
        private bool _addMilk;

        [ObservableProperty]
        private bool _postConditionMet;

        public CoffeeRecipe CurrentRecipe => CoffeeRecipe.GetRecipe(SelectedCoffeeType);

        // Предусловия
        public bool HasEnoughWater => _coffeMachine.Water >= CurrentRecipe.RequiredWater;
        public bool HasEnoughCoffee => _coffeMachine.Coffee >= CurrentRecipe.RequiredCoffee;
        public bool HasEnoughMilk => !AddMilk || _coffeMachine.Milk >= CurrentRecipe.RequiredMilk;
        public bool HasCups => _coffeMachine.Cups > 0;
        public bool HasSugar => _coffeMachine.Sugar >= SugarLevel;
        public bool IsNotBusy => !_coffeMachine.IsMakingCoffee;

        // Общее предусловие
        public bool PreConditionsMet =>
            HasEnoughWater && HasEnoughCoffee && HasEnoughMilk &&
            HasCups && HasSugar && IsNotBusy;

        private readonly CoffeeMachine _coffeMachine;

        public MakeCoffeeViewModel(CoffeeMachine coffeMachine)
        {
            _coffeMachine = coffeMachine;
        }

        [RelayCommand]
        public void MakeCoffee()
        {
            if (SelectedCoffeeType == null)
            {
                // Месседж
                return;
            }
        }
    }
}
