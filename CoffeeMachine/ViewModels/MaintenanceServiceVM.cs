using CoffeeMachineWPF.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace CoffeeMachineWPF.ViewModels;

public partial class MaintenanceServiceVM : OperationViewModelBase
{
    public override string OperationName => "Техническое обслуживание";

    [ObservableProperty]
    private MaintenanceType _selectedMaintenanceType = MaintenanceType.Cleaning;

    [ObservableProperty]
    private bool _drainWater;

    [ObservableProperty]
    private bool _resetStatistics;

    [ObservableProperty]
    private bool _postConditionMet;

    // Предусловия
    public bool HasWaste => _coffeeMachine.WasteLevel > 0;
    public bool IsMaintenanceKeyActivated => true;
    public bool HasCleaningSupplies => true;

    public bool PreConditionsMet =>
        IsNotMakingCoffee &&
        HasWaste &&
        IsMaintenanceKeyActivated &&
        HasCleaningSupplies;

    public IEnumerable<MaintenanceType> AvailableMaintenanceTypes =>
        Enum.GetValues(typeof(MaintenanceType)).Cast<MaintenanceType>();

    public MaintenanceServiceVM(CoffeeMachine coffeeMachine) : base(coffeeMachine)
    {
    }

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

    private void RepairMachine()
    {
        if (_coffeeMachine.IsBroken && _coffeeMachine.WearLevel < 100)
        {
            // машинка сама всё сделает
        }

        if (_coffeeMachine.WearLevel <= 70)
        {
            // тож сама
        }
    }

    private bool CanExecuteMaintenance() => PreConditionsMet;

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

    partial void OnSelectedMaintenanceTypeChanged(MaintenanceType value) => UpdatePreConditions();
    partial void OnDrainWaterChanged(bool value) => UpdatePreConditions();
    partial void OnResetStatisticsChanged(bool value) => UpdatePreConditions();

    private void UpdatePreConditions()
    {
        OnPropertyChanged(nameof(HasWaste));
        OnPropertyChanged(nameof(IsMaintenanceKeyActivated));
        OnPropertyChanged(nameof(HasCleaningSupplies));
        OnPropertyChanged(nameof(PreConditionsMet));

        PerformMaintenanceCommand.NotifyCanExecuteChanged();
    }

    private void UpdateAllProperties()
    {
        UpdatePreConditions();
        UpdateCommonProperties();
    }
}