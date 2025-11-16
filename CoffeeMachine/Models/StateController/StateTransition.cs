namespace CoffeeMachineWPF.Models.StateController
{
    public class StateTransition
    {
        public OperationMode FromState { get; set; }
        public OperationMode ToState { get; set; }
        public Func<MachineStatus, bool> Condition { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }

}
