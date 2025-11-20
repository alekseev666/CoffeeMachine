namespace CoffeeMachineWPF.Models
{
    /// <summary>
    /// Тип технического обслуживания кофемашины
    /// </summary>
    public enum MaintenanceType
    {
        Cleaning,       // Очистка баков
        DeepCleaning,   // Глубокая очистка
        Calibration,    // Калибровка дозаторов
        Diagnostic      // Полная диагностика
    }
}