using CoffeeMachineWPF.Analysis;
using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels;

public partial class MakeCoffeeVM : OperationViewModelBase
{
    private const double BASE_WEAR_AMOUNT = 0.5;
    private const double SUGAR_WEAR_MULTIPLIER = 0.1;
    private const double MILK_WEAR_AMOUNT = 0.2;

    private readonly CoffeeBrewingService _brewingService;
    private readonly CoffeeBrewingValidator _validator;
    private readonly WPAnalyzer _wpAnalyzer;
    private CoffeeRecipe _currentRecipe;

    [ObservableProperty]
    private string _analysisReport = "Нажмите 'Анализировать' для WP-анализа";

    [ObservableProperty]
    private double _estimatedBrewTime;

    [ObservableProperty]
    private double _currentBrewProgress;

    [ObservableProperty]
    private bool _isBrewingInProgress;

    [ObservableProperty]
    private string _brewStatus = "Готов к приготовлению";

    [ObservableProperty]
    private CoffeeType _selectedCoffeeType = CoffeeType.Espresso;

    [ObservableProperty]
    private int _sugarLevel;

    [ObservableProperty]
    private bool _addMilk;

    [ObservableProperty]
    private bool _postConditionMet;

    public override string OperationName => "Приготовление напитка";
    public IEnumerable<CoffeeType> AvailableCoffeeTypes => Enum.GetValues<CoffeeType>();
    public CoffeeRecipe CurrentRecipe => _currentRecipe;

    public bool HasEnoughWater => _coffeeMachine.Water >= _currentRecipe.RequiredWater;
    public bool HasEnoughCoffee => _coffeeMachine.Coffee >= _currentRecipe.RequiredCoffee;
    public bool HasEnoughMilk => !AddMilk || _coffeeMachine.Milk >= _currentRecipe.RequiredMilk;
    public bool HasCups => _coffeeMachine.Cups > 0;
    public bool HasSugar => _coffeeMachine.Sugar >= SugarLevel;

    public bool PreConditionsMet =>
        IsNotBroken &&
        IsNotMakingCoffee &&
        WasteLevelNotCritical &&
        HasEnoughWater &&
        HasEnoughCoffee &&
        HasEnoughMilk &&
        HasCups &&
        HasSugar;

    public MakeCoffeeVM(
        CoffeeMachine coffeeMachine,
        CoffeeBrewingService brewingService,
        CoffeeBrewingValidator validator,
        WPAnalyzer wpAnalyzer) : base(coffeeMachine)
    {
        _brewingService = brewingService;
        _validator = validator;
        _wpAnalyzer = wpAnalyzer;

        SetupEventHandlers();
        UpdateCurrentRecipe();
    }

    private void SetupEventHandlers()
    {
        _brewingService.BrewProgressChanged += progress => CurrentBrewProgress = progress;
        _brewingService.BrewStatusChanged += status => BrewStatus = status;
        _brewingService.BrewCompleted += OnBrewCompleted;

        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(SelectedCoffeeType) or nameof(SugarLevel) or nameof(AddMilk))
        {
            UpdateEstimatedTime();
            UpdateCurrentRecipe();
            NotifyPreConditionsChanged();
        }
    }

    private void UpdateCurrentRecipe()
    {
        _currentRecipe = CoffeeRecipe.GetRecipe(SelectedCoffeeType);
        OnPropertyChanged(nameof(CurrentRecipe));
        OnPropertyChanged(nameof(HasEnoughWater));
        OnPropertyChanged(nameof(HasEnoughCoffee));
        OnPropertyChanged(nameof(HasEnoughMilk));
    }

    private void UpdateEstimatedTime() => EstimatedBrewTime = _coffeeMachine.CalculateBrewTime(SelectedCoffeeType, SugarLevel, AddMilk);

    [RelayCommand]
    private void AnalyzeAlgorithm()
    {
        var validation = _validator.Validate(SelectedCoffeeType, SugarLevel, AddMilk);
        var resources = GetResourcesInfo();
        var prices = GetPricesInfo();
        var postConditions = GetPostConditionsInfo();

        AnalysisReport = _wpAnalyzer.GenerateFullReport(
            validation.Conditions,
            postConditions,
            SelectedCoffeeType,
            SugarLevel,
            AddMilk,
            resources,
            prices
        );
    }

    [RelayCommand(CanExecute = nameof(CanMakeCoffee))]
    private async Task MakeCoffeeAsync()
    {
        if (!CanMakeCoffee()) return;

        var oldState = new MachineState(_coffeeMachine);

        try
        {
            IsBrewingInProgress = true;
            BrewStatus = "Начало приготовления...";

            _coffeeMachine.IsMakingCoffee = true;
            UpdateCommonProperties();
            UpdateAllProperties();

            await _brewingService.BrewCoffeeAsync(SelectedCoffeeType, SugarLevel, AddMilk);

            ConsumeResources();
            UpdateMachineAfterBrewing();
            PostConditionMet = CheckPostConditions(oldState);
        }
        finally
        {
            IsBrewingInProgress = false;
            _coffeeMachine.IsMakingCoffee = false;
            UpdateAllProperties();
            AnalyzeAlgorithmCommand.Execute(null);
        }
    }

    private void ConsumeResources()
    {
        _coffeeMachine.Water -= _currentRecipe.RequiredWater;
        _coffeeMachine.Coffee -= _currentRecipe.RequiredCoffee;

        if (AddMilk)
            _coffeeMachine.Milk -= _currentRecipe.RequiredMilk;

        _coffeeMachine.Sugar -= SugarLevel;
        _coffeeMachine.Cups--;
    }

    private void UpdateMachineAfterBrewing()
    {
        _coffeeMachine.DrinksMade++;
        _coffeeMachine.TotalDrinksMade++;
        _coffeeMachine.WasteLevel = Math.Min(100, _coffeeMachine.WasteLevel + 2);

        IncreaseMachineWear();
    }

    private void IncreaseMachineWear()
    {
        double wearAmount = CalculateWearAmount();
        _coffeeMachine.IncreaseWear(wearAmount);
    }

    private double CalculateWearAmount()
    {
        double wearAmount = BASE_WEAR_AMOUNT;

        wearAmount += SelectedCoffeeType switch
        {
            CoffeeType.Espresso => 0.1,
            CoffeeType.Americano => 0.2,
            CoffeeType.Cappuccino => 0.3,
            CoffeeType.Latte => 0.4,
            _ => 0.2
        };

        if (SugarLevel > 0)
            wearAmount += SUGAR_WEAR_MULTIPLIER * SugarLevel;

        if (AddMilk)
            wearAmount += MILK_WEAR_AMOUNT;

        return wearAmount;
    }

    private bool CanMakeCoffee()
    {
        var validation = _validator.Validate(SelectedCoffeeType, SugarLevel, AddMilk);
        return validation.IsValid;
    }

    private bool CheckPostConditions(MachineState oldState)
    {
        return _coffeeMachine.Water == oldState.Water - _currentRecipe.RequiredWater &&
               _coffeeMachine.Coffee == oldState.Coffee - _currentRecipe.RequiredCoffee &&
               _coffeeMachine.Cups == oldState.Cups - 1 &&
               _coffeeMachine.DrinksMade == oldState.DrinksMade + 1 &&
               _coffeeMachine.WasteLevel > oldState.WasteLevel &&
               (!AddMilk || _coffeeMachine.Milk == oldState.Milk - _currentRecipe.RequiredMilk);
    }

    private void OnBrewCompleted()
    {
        BrewStatus = "Напиток готов!";
    }

    private void NotifyPreConditionsChanged()
    {
        OnPropertyChanged(nameof(PreConditionsMet));
        MakeCoffeeCommand.NotifyCanExecuteChanged();
    }

    private void UpdateAllProperties()
    {
        NotifyPreConditionsChanged();
        UpdateCommonProperties();
    }

    private Dictionary<string, (int current, int required, bool isMet)> GetResourcesInfo()
    {
        return new()
        {
            ["Вода"] = (_coffeeMachine.Water, _currentRecipe.RequiredWater, HasEnoughWater),
            ["Кофе"] = (_coffeeMachine.Coffee, _currentRecipe.RequiredCoffee, HasEnoughCoffee),
            ["Молоко"] = (_coffeeMachine.Milk, _currentRecipe.RequiredMilk, HasEnoughMilk),
            ["Стаканчики"] = (_coffeeMachine.Cups, 1, HasCups),
            ["Сахар"] = (_coffeeMachine.Sugar, SugarLevel, HasSugar)
        };
    }

    private Dictionary<string, double> GetPricesInfo() => new() { ["base"] = 20.0 };

    private Dictionary<string, bool> GetPostConditionsInfo() => new()
    {
        ["Напиток приготовлен"] = PostConditionMet,
        ["Ресурсы израсходованы"] = PostConditionMet,
        ["Счетчик напитков увеличен"] = PostConditionMet,
        ["Уровень отходов увеличен"] = PostConditionMet
    };
}