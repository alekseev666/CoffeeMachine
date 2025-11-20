namespace CoffeeMachineWPF.Models.StateController
{
    /// <summary>
    /// Текущее состояние кофемашины и её компонентов
    /// </summary>
    public class MachineStatus
    {
        /// <summary>
        /// Наличие достаточного количества воды для приготовления кофе
        /// </summary>
        public bool HasWater { get; set; }
        /// <summary>
        /// Наличие достаточного количества кофейных зерен
        /// </summary>
        public bool HasCoffee { get; set; }
        /// <summary>
        /// Наличие стаканчиков для подачи кофе
        /// </summary>
        public bool HasCups { get; set; }
        /// <summary>
        /// Наличие молока для приготовления кофейных напитков
        /// </summary>
        public bool HasMilk { get; set; }
        /// <summary>
        /// Термоблок нагрет до рабочей температуры
        /// </summary>
        public bool IsHeated { get; set; }
        /// <summary>
        /// Система чиста и готова к работе
        /// </summary>
        public bool IsClean { get; set; }
        /// <summary>
        /// Кофемашина готова к выполнению операций
        /// </summary>
        public bool CanOperate { get; set; }
        /// <summary>
        /// Текущий режим работы кофемашины
        /// </summary>
        public OperationMode CurrentMode { get; set; }
        /// <summary>
        /// Список уведомлений и предупреждений
        /// </summary>
        public List<string> Notifications { get; set; } = new List<string>();
    }
}
