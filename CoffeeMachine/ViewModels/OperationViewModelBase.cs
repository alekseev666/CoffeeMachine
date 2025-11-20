using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
/// <summary>
/// Класс модели представления для операций кофемашины
/// </summary>
public abstract class OperationViewModelBase : ObservableObject, IOperationViewModel
{
    /// <summary>
    /// Название операции кофемашины
    /// </summary>
    public abstract string OperationName { get; }

    /// <summary>
    /// Экземпляр кофемашины для выполнения операций
    /// </summary>
    protected readonly CoffeeMachine _coffeeMachine;

    /// <summary>
    /// Создание базовой модели представления операции
    /// </summary>
    /// <param name="coffeeMachine">Кофемашина для выполнения операций</param>
    protected OperationViewModelBase(CoffeeMachine coffeeMachine)
    {
        _coffeeMachine = coffeeMachine;
    }

    /// <summary>
    /// Признак исправности кофемашины
    /// </summary>
    public bool IsNotBroken => !_coffeeMachine.IsBroken;

    /// <summary>
    /// Признак отсутствия процесса приготовления кофе
    /// </summary>
    public bool IsNotMakingCoffee => !_coffeeMachine.IsMakingCoffee;

    /// <summary>
    /// Признак некритического уровня отходов
    /// </summary>
    public bool WasteLevelNotCritical => _coffeeMachine.WasteLevel < 90;

    /// <summary>
    /// Признак готовности кофемашины к работе
    /// </summary>
    public bool MachineIsOperational => IsNotBroken && IsNotMakingCoffee && WasteLevelNotCritical;

    /// <summary>
    /// Признак возможности выполнения обслуживания
    /// </summary>
    public bool CanPerformMaintenance => IsNotMakingCoffee;

    /// <summary>
    /// Текущий уровень отходов 
    /// </summary>
    public int CurrentWasteLevel => _coffeeMachine.WasteLevel;

    /// <summary>
    /// Уровень износа оборудования 
    /// </summary>
    public double WearLevel => _coffeeMachine.WearLevel;

    /// <summary>
    /// Здоровье компонентов 
    /// </summary>
    public int ComponentsHealth => _coffeeMachine.ComponentsHealth;

    /// <summary>
    /// Признак необходимости технического обслуживания
    /// </summary>
    public bool NeedsMaintenance => _coffeeMachine.NeedsMaintenance;

    /// <summary>
    /// Признак поломки кофемашины
    /// </summary>
    public bool IsBroken => _coffeeMachine.IsBroken;

    /// <summary>
    /// Текстовый статус кофемашины
    /// </summary>
    public string MachineStatus => _coffeeMachine.IsBroken ? "СЛОМАНА" :
                                 _coffeeMachine.NeedsMaintenance ? "ТРЕБУЕТ ОБСЛУЖИВАНИЯ" :
                                 _coffeeMachine.WasteLevel >= 90 ? "ПЕРЕПОЛНЕНА ОТХОДАМИ" : "ИСПРАВНА";

    /// <summary>
    /// Цвет индикации статуса кофемашины
    /// </summary>
    public string StatusColor => _coffeeMachine.IsBroken ? "Red" :
                               _coffeeMachine.NeedsMaintenance ? "Orange" :
                               _coffeeMachine.WasteLevel >= 90 ? "DarkOrange" : "Green";

    /// <summary>
    /// Обновление общих свойств состояния кофемашины
    /// </summary>
    protected void UpdateCommonProperties()
    {
        OnPropertyChanged(nameof(IsNotBroken));
        OnPropertyChanged(nameof(IsNotMakingCoffee));
        OnPropertyChanged(nameof(WasteLevelNotCritical));
        OnPropertyChanged(nameof(MachineIsOperational));
        OnPropertyChanged(nameof(CanPerformMaintenance));
        OnPropertyChanged(nameof(CurrentWasteLevel));
        OnPropertyChanged(nameof(WearLevel));
        OnPropertyChanged(nameof(ComponentsHealth));
        OnPropertyChanged(nameof(NeedsMaintenance));
        OnPropertyChanged(nameof(IsBroken));
        OnPropertyChanged(nameof(MachineStatus));
        OnPropertyChanged(nameof(StatusColor));
    }
}