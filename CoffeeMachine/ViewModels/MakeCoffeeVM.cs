using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels;

public partial class MakeCoffeeVM : OperationViewModelBase
{
    public override string OperationName => "Приготовление напитка";

    public IEnumerable<CoffeeType> AvailableCoffeeTypes =>
        Enum.GetValues(typeof(CoffeeType)).Cast<CoffeeType>();

    [ObservableProperty]
    private CoffeeType _selectedCoffeeType;

    [ObservableProperty]
    private int _sugarLevel;

    [ObservableProperty]
    private bool _addMilk;

    [ObservableProperty]
    private bool _postConditionMet;

    public CoffeeRecipe CurrentRecipe => CoffeeRecipe.GetRecipe(SelectedCoffeeType);

    public bool HasEnoughWater => _coffeeMachine.Water >= CurrentRecipe.RequiredWater;
    public bool HasEnoughCoffee => _coffeeMachine.Coffee >= CurrentRecipe.RequiredCoffee;
    public bool HasEnoughMilk => !AddMilk || _coffeeMachine.Milk >= CurrentRecipe.RequiredMilk;
    public bool HasCups => _coffeeMachine.Cups > 0;
    public bool HasSugar => _coffeeMachine.Sugar >= SugarLevel;

    public bool PreConditionsMet =>
        MachineIsOperational &&
        HasEnoughWater &&
        HasEnoughCoffee &&
        HasEnoughMilk &&
        HasCups &&
        HasSugar &&
        SelectedCoffeeType != null;

    public MakeCoffeeVM(CoffeeMachine coffeeMachine) : base(coffeeMachine)
    {
    }

    [RelayCommand(CanExecute = nameof(CanMakeCoffee))]
    private void MakeCoffee()
    {
        if (!CanMakeCoffee()) return;

        var oldWater = _coffeeMachine.Water;
        var oldCoffee = _coffeeMachine.Coffee;
        var oldMilk = _coffeeMachine.Milk;
        var oldCups = _coffeeMachine.Cups;
        var oldDrinksMade = _coffeeMachine.DrinksMade;

        try
        {
            _coffeeMachine.IsMakingCoffee = true;

            _coffeeMachine.Water -= CurrentRecipe.RequiredWater;
            _coffeeMachine.Coffee -= CurrentRecipe.RequiredCoffee;

            if (AddMilk)
                _coffeeMachine.Milk -= CurrentRecipe.RequiredMilk;

            _coffeeMachine.Sugar -= SugarLevel;
            _coffeeMachine.Cups -= 1;
            _coffeeMachine.DrinksMade += 1;
            _coffeeMachine.TotalDrinksMade += 1;

            IncreaseMachineWear();
            _coffeeMachine.WasteLevel = Math.Min(100, _coffeeMachine.WasteLevel + 2);


            PostConditionMet = CheckPostConditions(oldWater, oldCoffee, oldMilk, oldCups, oldDrinksMade);
        }
        finally
        {
            _coffeeMachine.IsMakingCoffee = false;
            UpdateAllProperties();
        }
    }

    private void IncreaseMachineWear()
    {
        double wearAmount = 0.5;

        wearAmount += SelectedCoffeeType switch
        {
            CoffeeType.Espresso => 0.1,
            CoffeeType.Americano => 0.2,
            CoffeeType.Cappuccino => 0.3,
            CoffeeType.Latte => 0.4,
            _ => 0.2
        };

        if (SugarLevel > 0)
            wearAmount += 0.1 * SugarLevel;

        if (AddMilk)
            wearAmount += 0.2;

        _coffeeMachine.IncreaseWear(wearAmount);
    }

    private bool CanMakeCoffee() => PreConditionsMet;

    private bool CheckPostConditions(int oldWater, int oldCoffee, int oldMilk, int oldCups, int oldDrinksMade)
    {
        return _coffeeMachine.Water == oldWater - CurrentRecipe.RequiredWater &&
               _coffeeMachine.Coffee == oldCoffee - CurrentRecipe.RequiredCoffee &&
               _coffeeMachine.Cups == oldCups - 1 &&
               _coffeeMachine.DrinksMade == oldDrinksMade + 1 &&
               _coffeeMachine.WasteLevel > 0 &&
               (!AddMilk || _coffeeMachine.Milk == oldMilk - CurrentRecipe.RequiredMilk);
    }

    partial void OnSelectedCoffeeTypeChanged(CoffeeType value) => UpdatePreConditions();
    partial void OnSugarLevelChanged(int value) => UpdatePreConditions();
    partial void OnAddMilkChanged(bool value) => UpdatePreConditions();

    private void UpdatePreConditions()
    {
        OnPropertyChanged(nameof(HasEnoughWater));
        OnPropertyChanged(nameof(HasEnoughCoffee));
        OnPropertyChanged(nameof(HasEnoughMilk));
        OnPropertyChanged(nameof(HasCups));
        OnPropertyChanged(nameof(HasSugar));
        OnPropertyChanged(nameof(PreConditionsMet));

        MakeCoffeeCommand.NotifyCanExecuteChanged();
    }

    private void UpdateAllProperties()
    {
        UpdatePreConditions();
        UpdateCommonProperties();
    }
}