using CoffeeMachineWPF.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace CoffeeMachineWPF.ViewModels;

/// <summary>
/// Модель представления для технического обслуживания кофемашины
/// </summary>
public partial class MaintenanceServiceVM : OperationViewModelBase
{
    /// <summary>
    /// Название операции технического обслуживания
    /// </summary>
    public override string OperationName => "Техническое обслуживание";

    /// <summary>
    /// Выбранный тип технического обслуживания
    /// </summary>
    [ObservableProperty]
    private MaintenanceType _selectedMaintenanceType = MaintenanceType.Cleaning;

    /// <summary>
    /// Слив воды при обслуживании
    /// </summary>
    [ObservableProperty]
    private bool _drainWater;

    /// <summary>
    /// Сброс статистики при обслуживании
    /// </summary>
    [ObservableProperty]
    private bool _resetStatistics;

    /// <summary>
    /// Выполнение постусловий операции обслуживания
    /// </summary>
    [ObservableProperty]
    private bool _postConditionMet;

    /// <summary>
    /// Признак наличия отходов для очистки
    /// </summary>
    public bool HasWaste => _coffeeMachine.WasteLevel > 0;

    /// <summary>
    /// Признак активации ключа обслуживания
    /// </summary>
    public bool IsMaintenanceKeyActivated => true;

    /// <summary>
    /// Признак наличия расходных материалов для обслуживания
    /// </summary>
    public bool HasCleaningSupplies => true;

    /// <summary>
    /// Признак выполнения всех предусловий операции обслуживания
    /// </summary>
    public bool PreConditionsMet =>
        IsNotMakingCoffee &&
        HasWaste &&
        IsMaintenanceKeyActivated &&
        HasCleaningSupplies;

    /// <summary>
    /// Доступные типы технического обслуживания
    /// </summary>
    public IEnumerable<MaintenanceType> AvailableMaintenanceTypes =>
        Enum.GetValues(typeof(MaintenanceType)).Cast<MaintenanceType>();

    /// <summary>
    /// Создание модели представления для технического обслуживания
    /// </summary>
    /// <param name="coffeeMachine">Кофемашина для обслуживания</param>
    public MaintenanceServiceVM(CoffeeMachine coffeeMachine) : base(coffeeMachine)
    {
    }

    /// <summary>
    /// Команда выполнения технического обслуживания
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteMaintenance))]
    private void PerformMaintenance()
    {
        if (!CanExecuteMaintenance()) return;

        var oldWasteLevel = _coffeeMachine.WasteLevel;
        var oldTemperature = _coffeeMachine.Temperature;
        var oldMaintenanceCount = _coffeeMachine.MaintenanceCount;
        var oldWater = _coffeeMachine.Water;
        var oldWearLevel = _coffeeMachine.WearLevel;
        var oldComponentsHealth = _coffeeMachine.ComponentsHealth;
        var oldIsBroken = _coffeeMachine.IsBroken;

        try
        {
            _coffeeMachine.WasteLevel = 0;
            _coffeeMachine.Temperature = 25;
            _coffeeMachine.MaintenanceCount++;

            ReduceMachineWear();

            RepairMachine();

            switch (SelectedMaintenanceType)
            {
                case MaintenanceType.DeepCleaning:
                    _coffeeMachine.Water = DrainWater ? 0 : _coffeeMachine.Water;
                    break;
            }

            if (ResetStatistics)
            {
                _coffeeMachine.DrinksMade = 0;
            }

            PostConditionMet = CheckPostConditions(oldWasteLevel, oldTemperature, oldMaintenanceCount,
                                                 oldWater, oldWearLevel, oldComponentsHealth, oldIsBroken);
        }
        finally
        {
            UpdateAllProperties();
        }
    }

    /// <summary>
    /// Уменьшение износа оборудования в зависимости от типа обслуживания
    /// </summary>
    private void ReduceMachineWear()
    {
        double reductionAmount = SelectedMaintenanceType switch
        {
            MaintenanceType.Cleaning => 15,
            MaintenanceType.DeepCleaning => 30,
            MaintenanceType.Calibration => 20,
            MaintenanceType.Diagnostic => 10,
            _ => 15
        };

        _coffeeMachine.ReduceWear(reductionAmount);
    }

    /// <summary>
    /// Выполнение ремонта кофемашины при необходимости
    /// </summary>
    private void RepairMachine()
    {
        if (_coffeeMachine.IsBroken && _coffeeMachine.WearLevel < 100)
        {
            
        }

        if (_coffeeMachine.WearLevel <= 70)
        {
           
        }
    }

    /// <summary>
    /// Проверка возможности выполнения команды обслуживания
    /// </summary>
    /// <returns>true, если команда может быть выполнена</returns>
    private bool CanExecuteMaintenance() => PreConditionsMet;

    /// <summary>
    /// Проверка выполнения постусловий операции обслуживания
    /// </summary>
    /// <param name="oldWasteLevel">Уровень отходов до обслуживания</param>
    /// <param name="oldTemperature">Температура до обслуживания</param>
    /// <param name="oldMaintenanceCount">Счетчик обслуживаний до операции</param>
    /// <param name="oldWater">Количество воды до обслуживания</param>
    /// <param name="oldWearLevel">Уровень износа до обслуживания</param>
    /// <param name="oldComponentsHealth">Здоровье компонентов до обслуживания</param>
    /// <param name="oldIsBroken">Состояние поломки до обслуживания</param>
    /// <returns>true, если постусловия выполнены</returns>
    private bool CheckPostConditions(int oldWasteLevel, double oldTemperature, int oldMaintenanceCount,
                                int oldWater, double oldWearLevel, int oldComponentsHealth, bool oldIsBroken)
    {
        var wasteCleaned = _coffeeMachine.WasteLevel == 0;
        var temperatureReset = _coffeeMachine.Temperature <= 30;
        var maintenanceCountIncreased = _coffeeMachine.MaintenanceCount == oldMaintenanceCount + 1;
        var waterDrained = !DrainWater || _coffeeMachine.Water == 0;

        var wearReduced = _coffeeMachine.WearLevel <= oldWearLevel;

        var healthMaintainedOrImproved = _coffeeMachine.ComponentsHealth >= oldComponentsHealth;
        var machineNotBrokenAfterMaintenance = !_coffeeMachine.IsBroken;

        return wasteCleaned &&
               temperatureReset &&
               maintenanceCountIncreased &&
               waterDrained &&
               wearReduced &&
               healthMaintainedOrImproved &&
               machineNotBrokenAfterMaintenance;
    }

    /// <summary>
    /// Обработчик изменения выбранного типа обслуживания
    /// </summary>
    /// <param name="value">Новый тип обслуживания</param>
    partial void OnSelectedMaintenanceTypeChanged(MaintenanceType value) => UpdatePreConditions();

    /// <summary>
    /// Обработчик изменения флага слива воды
    /// </summary>
    /// <param name="value">Новое значение флага</param>
    partial void OnDrainWaterChanged(bool value) => UpdatePreConditions();

    /// <summary>
    /// Обработчик изменения флага сброса статистики
    /// </summary>
    /// <param name="value">Новое значение флага</param>
    partial void OnResetStatisticsChanged(bool value) => UpdatePreConditions();

    /// <summary>
    /// Обновление состояния предусловий операции
    /// </summary>
    private void UpdatePreConditions()
    {
        OnPropertyChanged(nameof(HasWaste));
        OnPropertyChanged(nameof(IsMaintenanceKeyActivated));
        OnPropertyChanged(nameof(HasCleaningSupplies));
        OnPropertyChanged(nameof(PreConditionsMet));

        PerformMaintenanceCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Обновление всех свойств модели представления
    /// </summary>
    private void UpdateAllProperties()
    {
        UpdatePreConditions();
        UpdateCommonProperties();
    }
}