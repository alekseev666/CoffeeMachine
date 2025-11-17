using CoffeeMachineWPF.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class AdditionIngredientsVM : OperationViewModelBase
    {
        public override string OperationName => "Пополнение ингредиентов";

        [ObservableProperty]
        private IngredientType _selectedIngredientType;

        [ObservableProperty]
        private int _refillAmount = 100;

        [ObservableProperty]
        private bool _refillToMax = false;

        [ObservableProperty]
        private bool _postConditionMet;

        public bool IsValidIngredientType => SelectedIngredientType != null;
        public bool IsValidAmount => RefillAmount > 0;
        public bool IsWithinMaxCapacity => RefillAmount <= AvailableCapacity;
        public bool HasAvailableCapacity => AvailableCapacity > 0;

        public bool PreConditionsMet =>
                   MachineIsOperational &&
                   IsValidIngredientType &&
                   ((!RefillToMax && IsValidAmount && IsWithinMaxCapacity) ||
                    (RefillToMax && HasAvailableCapacity));

        public int CurrentAmount => GetCurrentAmount();
        public int MaxCapacity => GetMaxCapacity();
        public int AvailableCapacity => MaxCapacity - CurrentAmount;

        public IEnumerable<IngredientType> AvailableIngredientTypes =>
            Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>();

        public AdditionIngredientsVM(CoffeeMachine coffeeMachine) : base(coffeeMachine)
        {
        }

        [RelayCommand(CanExecute = nameof(CanExecuteRefill))]
        private void Refill()
        {
            if (!CanExecuteRefill()) return;

            var oldAmount = GetCurrentAmount();
            var calculatedRefillAmount = RefillToMax ? AvailableCapacity : RefillAmount;

            try
            {
                switch (SelectedIngredientType)
                {
                    case IngredientType.Water:
                        _coffeeMachine.Water += calculatedRefillAmount;
                        break;
                    case IngredientType.Coffee:
                        _coffeeMachine.Coffee += calculatedRefillAmount;
                        break;
                    case IngredientType.Milk:
                        _coffeeMachine.Milk += calculatedRefillAmount;
                        break;
                    case IngredientType.Cups:
                        _coffeeMachine.Cups += calculatedRefillAmount;
                        break;
                    case IngredientType.Sugar:
                        _coffeeMachine.Sugar += calculatedRefillAmount;
                        break;
                }

                PostConditionMet = CheckPostConditions(oldAmount, calculatedRefillAmount);
            }
            finally
            {
                UpdateAllProperties();
            }
        }

        private bool CanExecuteRefill() => PreConditionsMet;

        private int GetCurrentAmount()
        {
            return SelectedIngredientType switch
            {
                IngredientType.Water => _coffeeMachine.Water,
                IngredientType.Coffee => _coffeeMachine.Coffee,
                IngredientType.Milk => _coffeeMachine.Milk,
                IngredientType.Cups => _coffeeMachine.Cups,
                IngredientType.Sugar => _coffeeMachine.Sugar,
                _ => 0
            };
        }

        private int GetMaxCapacity()
        {
            return SelectedIngredientType switch
            {
                IngredientType.Water => 2000,    // мл
                IngredientType.Coffee => 500,    // грамм
                IngredientType.Milk => 1000,     // мл
                IngredientType.Cups => 50,       // штук
                IngredientType.Sugar => 200,     // порций
                _ => 0
            };
        }

        private bool CheckPostConditions(int oldAmount, int refillAmount)
        {
            var newAmount = GetCurrentAmount();
            return newAmount == oldAmount + refillAmount &&
                   newAmount <= GetMaxCapacity();
        }

        partial void OnSelectedIngredientTypeChanged(IngredientType value) => UpdatePreConditions();
        partial void OnRefillAmountChanged(int value) => UpdatePreConditions();
        partial void OnRefillToMaxChanged(bool value) => UpdatePreConditions();

        private void UpdatePreConditions()
        {
            OnPropertyChanged(nameof(IsValidIngredientType));
            OnPropertyChanged(nameof(IsValidAmount));
            OnPropertyChanged(nameof(IsWithinMaxCapacity));
            OnPropertyChanged(nameof(PreConditionsMet));
            OnPropertyChanged(nameof(CurrentAmount));
            OnPropertyChanged(nameof(MaxCapacity));
            OnPropertyChanged(nameof(AvailableCapacity));

            RefillCommand.NotifyCanExecuteChanged();
        }

        private void UpdateAllProperties()
        {
            UpdatePreConditions();
            UpdateCommonProperties();
        }
    }
}