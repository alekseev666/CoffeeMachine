namespace CoffeeMachineWPF.Models
{
    public class CoffeeMachine
    {
        public int Water { get; set; } = 1000;
        public int Coffee { get; set; } = 500;
        public int Cups { get; set; } = 50;
        public int Milk { get; set; } = 1000;
        public int Sugar { get; set; } = 200;
        public double Temperature { get; set; } = 95;
        public int WasteLevel { get; set; } = 10;
        public bool IsMakingCoffee { get; set; }
        public int DrinksMade { get; set; }
    }
}
