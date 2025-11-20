using CoffeeMachineWPF.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Модель представления для пополнения ингредиентов кофемашины
    /// </summary>
    public partial class AdditionIngredientsVM : OperationViewModelBase
    {
        /// <summary>
        /// Название операции пополнения ингредиентов
        /// </summary>
        public override string OperationName => "Пополнение ингредиентов";

        /// <summary>
        /// Выбранный тип ингредиента для пополнения
        /// </summary>
        [ObservableProperty]
        private IngredientType _selectedIngredientType = IngredientType.Water;

        /// <summary>
        /// Количество для пополнения
        /// </summary>
        [ObservableProperty]
        private int _refillAmount = 100;

        /// <summary>
        /// Пополнение до максимальной емкости
        /// </summary>
        [ObservableProperty]
        private bool _refillToMax = false;

        /// <summary>
        /// Выполнение постусловий операции
        /// </summary>
        [ObservableProperty]
        private bool _postConditionMet;

        /// <summary>
        /// Признак корректного выбора типа ингредиента
        /// </summary>
        public bool IsValidIngredientType => Enum.IsDefined(typeof(IngredientType), SelectedIngredientType);

        /// <summary>
        /// Признак корректного количества для пополнения
        /// </summary>
        public bool IsValidAmount => RefillAmount > 0;

        /// <summary>
        /// Признак допустимости количества в пределах максимальной емкости
        /// </summary>
        public bool IsWithinMaxCapacity => RefillAmount <= AvailableCapacity;

        /// <summary>
        /// Признак наличия доступной емкости для пополнения
        /// </summary>
        public bool HasAvailableCapacity => AvailableCapacity > 0;

        /// <summary>
        /// Признак выполнения всех предусловий операции
        /// </summary>
        public bool PreConditionsMet =>
                   MachineIsOperational &&
                   IsValidIngredientType &&
                   ((!RefillToMax && IsValidAmount && IsWithinMaxCapacity) ||
                    (RefillToMax && HasAvailableCapacity));

        /// <summary>
        /// Текущее количество выбранного ингредиента
        /// </summary>
        public int CurrentAmount => GetCurrentAmount();

        /// <summary>
        /// Максимальная емкость для выбранного ингредиента
        /// </summary>
        public int MaxCapacity => GetMaxCapacity();

        /// <summary>
        /// Доступная емкость для пополнения
        /// </summary>
        public int AvailableCapacity => MaxCapacity - CurrentAmount;

        /// <summary>
        /// Доступные типы ингредиентов для пополнения
        /// </summary>
        public IEnumerable<IngredientType> AvailableIngredientTypes =>
            Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>();

        /// <summary>
        /// Создание модели представления для пополнения ингредиентов
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для управления ингредиентами</param>
        public AdditionIngredientsVM(CoffeeMachine coffeeMachine) : base(coffeeMachine)
        {
        }

        /// <summary>
        /// Команда пополнения ингредиента
        /// </summary>
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

        /// <summary>
        /// Проверка возможности выполнения команды пополнения
        /// </summary>
        /// <returns>true, если команда может быть выполнена</returns>
        private bool CanExecuteRefill() => PreConditionsMet;

        /// <summary>
        /// Получение текущего количества выбранного ингредиента
        /// </summary>
        /// <returns>Текущее количество ингредиента</returns>
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

        /// <summary>
        /// Получение максимальной емкости для выбранного ингредиента
        /// </summary>
        /// <returns>Максимальная емкость ингредиента</returns>
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

        /// <summary>
        /// Проверка выполнения постусловий операции пополнения
        /// </summary>
        /// <param name="oldAmount">Количество ингредиента до пополнения</param>
        /// <param name="refillAmount">Количество добавленного ингредиента</param>
        /// <returns>true, если постусловия выполнены</returns>
        private bool CheckPostConditions(int oldAmount, int refillAmount)
        {
            var newAmount = GetCurrentAmount();
            return newAmount == oldAmount + refillAmount &&
                   newAmount <= GetMaxCapacity();
        }

        /// <summary>
        /// Обработчик изменения выбранного типа ингредиента
        /// </summary>
        /// <param name="value">Новый тип ингредиента</param>
        partial void OnSelectedIngredientTypeChanged(IngredientType value) => UpdatePreConditions();

        /// <summary>
        /// Обработчик изменения количества для пополнения
        /// </summary>
        /// <param name="value">Новое количество</param>
        partial void OnRefillAmountChanged(int value) => UpdatePreConditions();

        /// <summary>
        /// Обработчик изменения пополнения до максимума
        /// </summary>
        /// <param name="value">Новое значение флага</param>
        partial void OnRefillToMaxChanged(bool value) => UpdatePreConditions();

        /// <summary>
        /// Обновление состояний предусловий операции
        /// </summary>
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

        /// <summary>
        /// Обновление всех свойств модели представления
        /// </summary>
        private void UpdateAllProperties()
        {
            UpdatePreConditions();
            UpdateCommonProperties();
        }
    }
}