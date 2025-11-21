using CoffeeMachineWPF.Analysis;
using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels;
/// <summary>
/// Модель представления для приготовления кофе
/// </summary>
public partial class MakeCoffeeVM : OperationViewModelBase
{
    private const double BASE_WEAR_AMOUNT = 0.5;
    private const double SUGAR_WEAR_MULTIPLIER = 0.1;
    private const double MILK_WEAR_AMOUNT = 0.2;

    private readonly CoffeeBrewingService _brewingService;
    private readonly CoffeeBrewingValidator _validator;
    private readonly WPAnalyzer _wpAnalyzer;
    private CoffeeRecipe _currentRecipe;

    /// <summary>
    /// Отчет WP-анализа алгоритма приготовления
    /// </summary>
    [ObservableProperty]
    private string _analysisReport = "Нажмите 'Анализировать' для WP-анализа";

    /// <summary>
    /// Расчетное время приготовления в секундах
    /// </summary>
    [ObservableProperty]
    private double _estimatedBrewTime = 15;

    /// <summary>
    /// Текущий прогресс приготовления в процентах
    /// </summary>
    [ObservableProperty]
    private double _currentBrewProgress;

    /// <summary>
    /// Выполнение процесса приготовления
    /// </summary>
    [ObservableProperty]
    private bool _isBrewingInProgress;

    /// <summary>
    /// Готовность кофе
    /// </summary>
    [ObservableProperty]
    private bool _isCoffeeReady;

    /// <summary>
    /// Статус процесса приготовления
    /// </summary>
    [ObservableProperty]
    private string _brewStatus = "Готов к приготовлению";

    /// <summary>
    /// Сообщение о результате приготовления
    /// </summary>
    [ObservableProperty]
    private string _brewResultMessage = "Кофе не готово";

    /// <summary>
    /// Выбранный тип кофе
    /// </summary>
    [ObservableProperty]
    private CoffeeType _selectedCoffeeType = CoffeeType.Espresso;

    /// <summary>
    /// Уровень сахара в порциях
    /// </summary>
    [ObservableProperty]
    private int _sugarLevel;


    /// <summary>
    /// Выполнение постусловий операции
    /// </summary>
    [ObservableProperty]
    private bool _postConditionMet;

    /// <summary>
    /// Название операции приготовления напитка
    /// </summary>
    public override string OperationName => "Приготовление напитка";

    /// <summary>
    /// Доступные типы кофе
    /// </summary>
    public IEnumerable<CoffeeType> AvailableCoffeeTypes => Enum.GetValues<CoffeeType>();

    /// <summary>
    /// Текущий рецепт приготовления кофе
    /// </summary>
    public CoffeeRecipe CurrentRecipe => _currentRecipe;

    /// <summary>
    /// Признак достаточного количества воды
    /// </summary>
    public bool HasEnoughWater => _coffeeMachine.Water >= _currentRecipe.RequiredWater;

    /// <summary>
    /// Признак достаточного количества кофе
    /// </summary>
    public bool HasEnoughCoffee => _coffeeMachine.Coffee >= _currentRecipe.RequiredCoffee;

    /// <summary>
    /// Признак достаточного количества молока
    /// </summary>
    public bool HasEnoughMilk => _coffeeMachine.Milk >= _currentRecipe.RequiredMilk;

    /// <summary>
    /// Признак наличия стаканчиков
    /// </summary>
    public bool HasCups => _coffeeMachine.Cups > 0;

    /// <summary>
    /// Признак достаточного количества сахара
    /// </summary>
    public bool HasSugar => _coffeeMachine.Sugar >= SugarLevel;

    /// <summary>
    /// Признак возможности добавления молока для выбранного типа кофе
    /// </summary>
    public bool CanAddMilk => SelectedCoffeeType != CoffeeType.Espresso &&
                            SelectedCoffeeType != CoffeeType.Americano;

    /// <summary>
    /// Признак выполнения всех предусловий операции
    /// </summary>
    public bool PreConditionsMet =>
        IsNotBroken &&
        IsNotMakingCoffee &&
        WasteLevelNotCritical &&
        HasEnoughWater &&
        HasEnoughCoffee &&
        HasCups &&
        HasSugar;

    /// <summary>
    /// Создание модели представления для приготовления кофе
    /// </summary>
    /// <param name="coffeeMachine">Кофемашина для приготовления</param>
    /// <param name="brewingService">Сервис приготовления кофе</param>
    /// <param name="validator">Валидатор проверки условий</param>
    /// <param name="wpAnalyzer">Анализатор weakest precondition</param>
    public MakeCoffeeVM(
        CoffeeMachine coffeeMachine,
        CoffeeBrewingService brewingService,
        CoffeeBrewingValidator validator,
        WPAnalyzer wpAnalyzer) : base(coffeeMachine)
    {
        _brewingService = brewingService;
        _validator = validator;
        _wpAnalyzer = wpAnalyzer;

        BrewResultMessage = "Кофе не готово";
        IsCoffeeReady = false;


        SetupEventHandlers();
        UpdateCurrentRecipe();
    }

    /// <summary>
    /// Настройка обработчиков событий
    /// </summary>
    private void SetupEventHandlers()
    {
        _brewingService.BrewProgressChanged += progress => CurrentBrewProgress = progress;
        _brewingService.BrewStatusChanged += status => BrewStatus = status;
        _brewingService.BrewCompleted += OnBrewCompleted;

        PropertyChanged += OnPropertyChanged;
    }

    /// <summary>
    /// Обработчик изменения выбранного типа кофе
    /// </summary>
    /// <param name="value">Новый тип кофе</param>
    partial void OnSelectedCoffeeTypeChanged(CoffeeType value)
    {
        ResetCoffeeReadyState();
        UpdateAll();
    }

    /// <summary>
    /// Обновление всех свойств модели представления
    /// </summary>
    private void UpdateAll()
    {
        UpdateEstimatedTime();
        UpdateCurrentRecipe();
        NotifyPreConditionsChanged();
        OnPropertyChanged(nameof(CanAddMilk)); 
    }

    /// <summary>
    /// Обработчик изменения свойств модели представления
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="e">Данные события</param>
    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(SelectedCoffeeType) or nameof(SugarLevel))
        {
            UpdateEstimatedTime();
            UpdateCurrentRecipe();
            NotifyPreConditionsChanged();
        }
    }

    /// <summary>
    /// Обновление текущего рецепта приготовления
    /// </summary>
    private void UpdateCurrentRecipe()
    {
        _currentRecipe = CoffeeRecipe.GetRecipe(SelectedCoffeeType);
        OnPropertyChanged(nameof(CurrentRecipe));
        OnPropertyChanged(nameof(HasEnoughWater));
        OnPropertyChanged(nameof(HasEnoughCoffee));
    }

    /// <summary>
    /// Обновление расчетного времени приготовления
    /// </summary>
    private void UpdateEstimatedTime() => EstimatedBrewTime = _coffeeMachine.CalculateBrewTime(SelectedCoffeeType, SugarLevel);

    /// <summary>
    /// Команда анализа алгоритма приготовления
    /// </summary>
    [RelayCommand]
    private void AnalyzeAlgorithm()
    {
        var validation = _validator.Validate(SelectedCoffeeType, SugarLevel);
        var resources = GetResourcesInfo();
        var prices = GetPricesInfo();
        var postConditions = GetPostConditionsInfo();

        AnalysisReport = _wpAnalyzer.GenerateFullReport(
            validation.Conditions,
            postConditions,
            SelectedCoffeeType,
            SugarLevel,
            resources,
            prices
        );
    }

    /// <summary>
    /// Команда приготовления кофе
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanMakeCoffee))]
    private async Task MakeCoffeeAsync()
    {
        if (!CanMakeCoffee()) return;

        var oldState = new MachineState(_coffeeMachine);

        try
        {
            IsBrewingInProgress = true;
            IsCoffeeReady = false; 
            BrewStatus = "Начало приготовления...";

            _coffeeMachine.IsMakingCoffee = true;
            UpdateCommonProperties();
            UpdateAllProperties();


            await _brewingService.BrewCoffeeAsync(SelectedCoffeeType, SugarLevel);

            ConsumeResources();
            UpdateMachineAfterBrewing();
            PostConditionMet = CheckPostConditions(oldState);

            BrewResultMessage = "Кофе готов!";
            IsCoffeeReady = true; 
        }
        finally
        {
            IsBrewingInProgress = false;
            _coffeeMachine.IsMakingCoffee = false;
            UpdateAllProperties();
            AnalyzeAlgorithmCommand.Execute(null);
        }
    }

    /// <summary>
    /// Расход ресурсов для приготовления кофе
    /// </summary>
    private void ConsumeResources()
    {
        _coffeeMachine.Water -= _currentRecipe.RequiredWater;
        _coffeeMachine.Coffee -= _currentRecipe.RequiredCoffee;
        _coffeeMachine.Milk -= _currentRecipe.RequiredMilk;
        _coffeeMachine.Sugar -= SugarLevel;
        _coffeeMachine.Cups--;
    }

    /// <summary>
    /// Обновление состояния кофемашины после приготовления
    /// </summary>
    private void UpdateMachineAfterBrewing()
    {
        _coffeeMachine.DrinksMade++;
        _coffeeMachine.TotalDrinksMade++;
        _coffeeMachine.WasteLevel = Math.Min(100, _coffeeMachine.WasteLevel + 2);

        IncreaseMachineWear();
    }

    /// <summary>
    /// Увеличение износа оборудования
    /// </summary>
    private void IncreaseMachineWear()
    {
        double wearAmount = CalculateWearAmount();
        _coffeeMachine.IncreaseWear(wearAmount);
    }

    /// <summary>
    /// Вычисление величины износа оборудования
    /// </summary>
    /// <returns>Величина износа</returns>
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


        return wearAmount;
    }

    /// <summary>
    /// Проверка возможности приготовления кофе
    /// </summary>
    /// <returns>true, если кофе может быть приготовлено</returns>
    private bool CanMakeCoffee()
    {
        var validation = _validator.Validate(SelectedCoffeeType, SugarLevel);


        return validation.IsValid;
    }

    /// <summary>
    /// Проверка выполнения постусловий операции приготовления
    /// </summary>
    /// <param name="oldState">Состояние кофемашины до приготовления</param>
    /// <returns>true, если постусловия выполнены</returns>
    private bool CheckPostConditions(MachineState oldState)
    {
        return _coffeeMachine.Water == oldState.Water - _currentRecipe.RequiredWater &&
               _coffeeMachine.Coffee == oldState.Coffee - _currentRecipe.RequiredCoffee &&
               _coffeeMachine.Cups == oldState.Cups - 1 &&
               _coffeeMachine.DrinksMade == oldState.DrinksMade + 1 &&
               _coffeeMachine.WasteLevel > oldState.WasteLevel &&
               (_coffeeMachine.Milk == oldState.Milk - _currentRecipe.RequiredMilk);
    }

    /// <summary>
    /// Обработчик завершения процесса приготовления
    /// </summary>
    private void OnBrewCompleted()
    {
        BrewStatus = "Напиток готов!";
    }

    /// <summary>
    /// Уведомление об изменении предусловий
    /// </summary>
    private void NotifyPreConditionsChanged()
    {
        OnPropertyChanged(nameof(PreConditionsMet));
        MakeCoffeeCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Обновление всех свойств модели представления
    /// </summary>
    private void UpdateAllProperties()
    {
        
        NotifyPreConditionsChanged();
        UpdateCommonProperties();
    }

    /// <summary>
    /// Получение информации о ресурсах для анализа
    /// </summary>
    /// <returns>Словарь с информацией о ресурсах</returns>
    private Dictionary<string, (int current, int required, bool isMet)> GetResourcesInfo()
    {
        return new()
        {
            ["Вода"] = (_coffeeMachine.Water, _currentRecipe.RequiredWater, HasEnoughWater),
            ["Кофе"] = (_coffeeMachine.Coffee, _currentRecipe.RequiredCoffee, HasEnoughCoffee),
            ["Стаканчики"] = (_coffeeMachine.Cups, 1, HasCups),
            ["Сахар"] = (_coffeeMachine.Sugar, SugarLevel, HasSugar)
        };
    }

    /// <summary>
    /// Получение информации о ценах для анализа
    /// </summary>
    /// <returns>Словарь с информацией о ценах</returns>
    private Dictionary<string, double> GetPricesInfo() => new() { ["base"] = 20.0 };

    /// <summary>
    /// Получение информации о постусловиях для анализа
    /// </summary>
    /// <returns>Словарь с информацией о постусловиях</returns>
    private Dictionary<string, bool> GetPostConditionsInfo() => new()
    {
        ["Напиток приготовлен"] = PostConditionMet,
        ["Ресурсы израсходованы"] = PostConditionMet,
        ["Счетчик напитков увеличен"] = PostConditionMet,
        ["Уровень отходов увеличен"] = PostConditionMet
    };

    /// <summary>
    /// Обработчик изменения уровня сахара
    /// </summary>
    /// <param name="value">Новый уровень сахара</param>
    partial void OnSugarLevelChanged(int value)
    {
        ResetCoffeeReadyState();
        UpdateEstimatedTime();
        NotifyPreConditionsChanged();
    }

    /// <summary>
    /// Сброс состояния готовности кофе
    /// </summary>
    private void ResetCoffeeReadyState()
    {
        BrewResultMessage = "Кофе не готово";
        IsCoffeeReady = false;
    }
}