namespace CoffeeMachineWPF.Models
{
    public record MachineState(
    int Water,
    int Coffee,
    int Milk,
    int Cups,
    int Sugar,
    int DrinksMade,
    double WasteLevel)
    {
        public MachineState(CoffeeMachine machine) : this(
            machine.Water,
            machine.Coffee,
            machine.Milk,
            machine.Cups,
            machine.Sugar,
            machine.DrinksMade,
            machine.WasteLevel)
        {
        }
    }
}
