using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Models.StateController;
using System.Text;

namespace CoffeeMachineWPF.Services
{
    /// <summary>
    /// Сервис управления состоянием кофемашины
    /// </summary>
    public class StateControllerService
    {
        private readonly CoffeeMachine _coffeeMachine;
        private MachineStatus _currentStatus;

        /// <summary>
        /// Создание сервиса управления состоянием для указанной кофемашины
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для управления состоянием</param>
        public StateControllerService(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
            _currentStatus = new MachineStatus();
        }

        /// <summary>
        /// Получение текущего состояния кофемашины
        /// </summary>
        /// <returns>Текущее состояние кофемашины</returns>
        public MachineStatus GetCurrentStatus()
        {
            return _currentStatus;
        }

        /// <summary>
        /// Генерирация отчета анализа состояния кофемашины
        /// </summary>
        /// <param name="status">Состояние кофемашины для анализа</param>
        /// <returns>Отчет анализа состояния</returns>
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

        /// <summary>
        /// Получение отображаемого названия режима работы
        /// </summary>
        /// <param name="mode">Режим работы кофемашины</param>
        /// <returns>Локализованное название режима</returns>
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