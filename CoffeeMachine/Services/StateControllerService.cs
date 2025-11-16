using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Models.StateController;
using System.Text;

namespace CoffeeMachineWPF.Services
{
    public class StateControllerService
    {
        private readonly CoffeeMachine _coffeeMachine;
        private MachineStatus _currentStatus;

        public StateControllerService(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
            _currentStatus = new MachineStatus();
        }

        public MachineStatus GetCurrentStatus()
        {
            return _currentStatus;
        }

        public void SimulateStateChange()
        {
            _currentStatus.HasWater = !_currentStatus.HasWater;
        }

        public string GenerateStateAnalysisReport(MachineStatus status)
        {
            var report = new StringBuilder();

            report.AppendLine("АНАЛИЗ СОСТОЯНИЯ КОФЕЙНОЙ МАШИНЫ");
            report.AppendLine("══════════════════════════════════");
            report.AppendLine();

            report.AppendLine($"ТЕКУЩИЙ РЕЖИМ: {GetModeDisplay(status.CurrentMode)}");
            report.AppendLine($"Время анализа: {DateTime.Now:HH:mm:ss}");
            report.AppendLine();

            report.AppendLine("ЛОГИЧЕСКИЕ ПЕРЕМЕННЫЕ:");
            report.AppendLine($"HasWater: {status.HasWater}");
            report.AppendLine($"HasCoffee: {status.HasCoffee}");
            report.AppendLine($"HasCups: {status.HasCups}");
            report.AppendLine($"HasMilk: {status.HasMilk}");
            report.AppendLine($"IsHeated: {status.IsHeated}");
            report.AppendLine($"IsClean: {status.IsClean}");
            report.AppendLine();

            report.AppendLine($"ФОРМУЛА CanOperate: {status.CanOperate}");
            report.AppendLine($"HasWater ∧ HasCoffee ∧ HasCups ∧ IsHeated ∧ IsClean = {status.CanOperate}");
            report.AppendLine();

            report.AppendLine("УВЕДОМЛЕНИЯ:");
            foreach (var notification in status.Notifications)
            {
                report.AppendLine($"• {notification}");
            }

            return report.ToString();
        }

        private string GetModeDisplay(OperationMode mode)
        {
            return mode switch
            {
                OperationMode.NormalMode => "Нормальный режим",
                OperationMode.MaintenanceMode => "Режим обслуживания",
                OperationMode.ErrorMode => "Аварийный режим",
                OperationMode.StandbyMode => "Режим ожидания",
                _ => "Неизвестный режим"
            };
        }
    }
}