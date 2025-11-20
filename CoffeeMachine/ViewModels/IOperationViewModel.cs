namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Интерфейс модели представления операции кофемашины
    /// </summary>
    public interface IOperationViewModel
    {
        /// <summary>
        /// Название операции кофемашины
        /// </summary>
        string OperationName { get; }
    }
}