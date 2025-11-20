using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Models.StateController;
using CoffeeMachineWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Модель представления для логического контроллера состояния кофемашины
    /// </summary>
    public partial class StateControllerVM : ObservableObject, IOperationViewModel
    {
        private readonly CoffeeMachine _coffeeMachine;
        private readonly StateControllerService _stateController;

        /// <summary>
        /// Отчет анализа состояния кофемашины
        /// </summary>
        [ObservableProperty]
        private string _stateAnalysisReport = "Нажмите 'Анализировать' для проверки состояния";

        /// <summary>
        /// Текущее состояние кофемашины
        /// </summary>
        [ObservableProperty]
        private MachineStatus _currentStatus = null!;

        /// <summary>
        /// Отображаемое название текущего режима
        /// </summary>
        [ObservableProperty]
        private string _currentModeDisplay = "Загрузка...";

        /// <summary>
        /// Цвет индикации текущего режима
        /// </summary>
        [ObservableProperty]
        private string _statusColor = "Gray";

        /// <summary>
        /// Название операции логического контроллера
        /// </summary>
        public string OperationName => "Логический контроллер";

        /// <summary>
        /// Создание модели представления для логического контроллера
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для контроля состояния</param>
        /// <param name="stateController">Сервис управления состоянием</param>
        public StateControllerVM(CoffeeMachine coffeeMachine, StateControllerService stateController)
        {
            _coffeeMachine = coffeeMachine;
            _stateController = stateController;

            UpdateStatus();
            AnalyzeState();
        }

        /// <summary>
        /// Команда анализа текущего состояния кофемашины
        /// </summary>
        [RelayCommand]
        private void AnalyzeState()
        {
            try
            {
                UpdateStatusFromCoffeeMachine();
                StateAnalysisReport = _stateController.GenerateStateAnalysisReport(CurrentStatus);
                UpdateModeDisplay();
            }
            catch (Exception ex)
            {
                StateAnalysisReport = $"Ошибка анализа: {ex.Message}";
            }
        }

        /// <summary>
        /// Обновление статуса из сервиса управления состоянием
        /// </summary>
        private void UpdateStatus()
        {
            CurrentStatus = _stateController.GetCurrentStatus();
            UpdateModeDisplay();
        }

        /// <summary>
        /// Обновление статуса на основе текущего состояния кофемашины
        /// </summary>
        private void UpdateStatusFromCoffeeMachine()
        {
            var status = new MachineStatus
            {
                HasWater = _coffeeMachine.Water > 0,
                HasCoffee = _coffeeMachine.Coffee > 0,
                HasCups = _coffeeMachine.Cups > 0,
                HasMilk = _coffeeMachine.Milk > 0,

                IsHeated = _coffeeMachine.Temperature >= 90,
                IsClean = _coffeeMachine.WasteLevel < 80,

                CurrentMode = DetermineOperationMode(),

                Notifications = GenerateNotifications()
            };

            status.CanOperate = status.HasWater && status.HasCoffee && status.HasCups &&
                              status.IsHeated && status.IsClean &&
                              !_coffeeMachine.IsBroken &&
                              !_coffeeMachine.IsMakingCoffee;

            CurrentStatus = status;
        }

        /// <summary>
        /// Определение текущего режима работы кофемашины
        /// </summary>
        /// <returns>Текущий режим работы</returns>
        private OperationMode DetermineOperationMode()
        {
            if (_coffeeMachine.IsBroken)
                return OperationMode.ErrorMode;

            if (_coffeeMachine.NeedsMaintenance)
                return OperationMode.MaintenanceMode;

            if (_coffeeMachine.IsMakingCoffee)
                return OperationMode.StandbyMode;

            if (_coffeeMachine.WasteLevel >= 90)
                return OperationMode.MaintenanceMode;

            return OperationMode.NormalMode;
        }

        /// <summary>
        /// Генерация уведомления о состоянии кофемашины
        /// </summary>
        /// <returns>Список уведомлений</returns>
        private List<string> GenerateNotifications()
        {
            var notifications = new List<string>();

            if (_coffeeMachine.IsBroken)
                notifications.Add("CRITICAL: Машина сломана! Требуется ремонт");

            if (_coffeeMachine.Water == 0)
                notifications.Add("CRITICAL: Закончилась вода");

            if (_coffeeMachine.Coffee == 0)
                notifications.Add("CRITICAL: Закончился кофе");

            if (_coffeeMachine.Cups == 0)
                notifications.Add("CRITICAL: Закончились стаканчики");

            if (_coffeeMachine.NeedsMaintenance)
                notifications.Add("WARNING: Высокий износ! Требуется обслуживание");

            if (_coffeeMachine.WasteLevel >= 80)
                notifications.Add("WARNING: Высокий уровень отходов");

            if (_coffeeMachine.Water < 200)
                notifications.Add("WARNING: Мало воды");

            if (_coffeeMachine.Coffee < 50)
                notifications.Add("WARNING: Мало кофе");

            if (_coffeeMachine.Cups < 10)
                notifications.Add("WARNING: Мало стаканчиков");

            if (notifications.Count == 0)
                notifications.Add("INFO: Все системы в норме");
            else
                notifications.Add("INFO: Проверьте уведомления выше");

            return notifications;
        }

        /// <summary>
        /// Обновление отображения текущего режима и цвета
        /// </summary>
        private void UpdateModeDisplay()
        {
            if (CurrentStatus == null)
            {
                CurrentModeDisplay = "Данные не загружены";
                StatusColor = "Gray";
                return;
            }

            CurrentModeDisplay = CurrentStatus.CurrentMode switch
            {
                OperationMode.NormalMode => "Нормальный режим",
                OperationMode.MaintenanceMode => "Режим обслуживания",
                OperationMode.ErrorMode => "Аварийный режим",
                OperationMode.StandbyMode => "Режим ожидания",
                _ => "Неизвестный режим"
            };

            StatusColor = CurrentStatus.CurrentMode switch
            {
                OperationMode.NormalMode => "Green",
                OperationMode.MaintenanceMode => "Orange",
                OperationMode.ErrorMode => "Red",
                OperationMode.StandbyMode => "Blue",
                _ => "Gray"
            };
        }

        /// <summary>
        /// Обновление состояния контроллера
        /// </summary>
        public void RefreshState()
        {
            UpdateStatusFromCoffeeMachine();
            UpdateModeDisplay();
            OnPropertyChanged(nameof(CurrentStatus));
        }
    }
}