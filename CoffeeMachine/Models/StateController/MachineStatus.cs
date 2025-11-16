namespace CoffeeMachineWPF.Models.StateController
{
    public class MachineStatus
    {
        public bool HasWater { get; set; }
        public bool HasCoffee { get; set; }
        public bool HasCups { get; set; }
        public bool HasMilk { get; set; }
        public bool IsHeated { get; set; }
        public bool IsClean { get; set; }
        public bool CanOperate { get; set; }
        public OperationMode CurrentMode { get; set; }
        public List<string> Notifications { get; set; } = new List<string>();
    }
}
