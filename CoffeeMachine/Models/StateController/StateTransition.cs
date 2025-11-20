namespace CoffeeMachineWPF.Models.StateController
{
    /// <summary>
    /// Переход между состояниями кофемашины
    /// </summary>
    public class StateTransition
    {
        /// <summary>
        /// Исходное состояние кофемашины
        /// </summary>
        public OperationMode FromState { get; set; }
        /// <summary>
        /// Целевое состояние кофемашины
        /// </summary>
        public OperationMode ToState { get; set; }
        /// <summary>
        /// Условие перехода между состояниями
        /// </summary>
        public Func<MachineStatus, bool> Condition { get; set; } = null!;
        /// <summary>
        /// Описание перехода и его назначения
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

}
