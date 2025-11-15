using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

public abstract class OperationViewModelBase : ObservableObject, IOperationViewModel
{
    public abstract string OperationName { get; }

    protected readonly CoffeeMachine _coffeeMachine;

    protected OperationViewModelBase(CoffeeMachine coffeeMachine)
    {
        _coffeeMachine = coffeeMachine;
    }

    // Предусловия
    public bool IsNotBroken => !_coffeeMachine.IsBroken;
    public bool IsNotMakingCoffee => !_coffeeMachine.IsMakingCoffee;
    public bool WasteLevelNotCritical => _coffeeMachine.WasteLevel < 90;
    public bool MachineIsOperational => IsNotBroken && IsNotMakingCoffee && WasteLevelNotCritical;

    public bool CanPerformMaintenance => IsNotMakingCoffee;

    public int CurrentWasteLevel => _coffeeMachine.WasteLevel;
    public double WearLevel => _coffeeMachine.WearLevel;
    public int ComponentsHealth => _coffeeMachine.ComponentsHealth;
    public bool NeedsMaintenance => _coffeeMachine.NeedsMaintenance;
    public bool IsBroken => _coffeeMachine.IsBroken;

    public string MachineStatus => _coffeeMachine.IsBroken ? "СЛОМАНА" :
                                 _coffeeMachine.NeedsMaintenance ? "ТРЕБУЕТ ОБСЛУЖИВАНИЯ" :
                                 _coffeeMachine.WasteLevel >= 90 ? "ПЕРЕПОЛНЕНА ОТХОДАМИ" : "ИСПРАВНА";

    public string StatusColor => _coffeeMachine.IsBroken ? "Red" :
                               _coffeeMachine.NeedsMaintenance ? "Orange" :
                               _coffeeMachine.WasteLevel >= 90 ? "DarkOrange" : "Green";

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