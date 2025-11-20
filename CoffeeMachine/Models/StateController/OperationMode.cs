namespace CoffeeMachineWPF.Models.StateController
{
    /// <summary>
    /// Режим работы кофемашины
    /// </summary>
    public enum OperationMode
    {
        NormalMode, // Стандартный режим приготовления кофе
        MaintenanceMode, // Режим технического обслуживания и очистки
        ErrorMode, // Аварийный режим при возникновении ошибок
        StandbyMode //Режим ожидания с пониженным энергопотреблением
    }
}
