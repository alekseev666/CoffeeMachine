using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Models.StateController;
using CoffeeMachineWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class StateControllerVM : ObservableObject, IOperationViewModel
    {
        private readonly CoffeeMachine _coffeeMachine;
        private readonly StateControllerService _stateController;

        [ObservableProperty]
        private string _stateAnalysisReport = "Нажмите 'Анализировать' для проверки состояния";

        [ObservableProperty]
        private MachineStatus _currentStatus = null!;

        [ObservableProperty]
        private string _currentModeDisplay = "Загрузка...";

        [ObservableProperty]
        private string _statusColor = "Gray";

        public string OperationName => "Логический контроллер";

        public StateControllerVM(CoffeeMachine coffeeMachine, StateControllerService stateController)
        {
            _coffeeMachine = coffeeMachine;
            _stateController = stateController;

            UpdateStatus();
            AnalyzeState();
        }

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
        private void UpdateStatus()
        {
            CurrentStatus = _stateController.GetCurrentStatus();
            UpdateModeDisplay();
        }

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

        public void RefreshState()
        {
            UpdateStatusFromCoffeeMachine();
            UpdateModeDisplay();
            OnPropertyChanged(nameof(CurrentStatus));
        }
    }
}