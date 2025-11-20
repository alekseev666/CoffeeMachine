namespace CoffeeMachineWPF.Models.StateController
{
    /// <summary>
    /// Уровень важности уведомления кофемашины
    /// </summary>
    public enum NotificationPriority
    {
        Critical, // Критическое уведомление о неисправности или блокирующей проблеме
        Warning, // Предупреждение о потенциальной проблеме или рекомендации
        Info // Информационное сообщение о состоянии или процессе
    }
}
