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
        public bool IsMakingCoffee { get; set; }
        public int DrinksMade { get; set; }
        public int WasteLevel { get; set; } = 15;
        public int MaintenanceCount { get; set; }

        public int TotalDrinksMade { get; set; }
        public double WearLevel { get; set; } = 0;
        public int ComponentsHealth { get; set; } = 100;
        public bool NeedsMaintenance => WearLevel > 70;
        public bool IsBroken => WearLevel >= 100;

        public void IncreaseWear(double amount)
        {
            WearLevel = Math.Min(100, WearLevel + amount);
            ComponentsHealth = Math.Max(0, 100 - (int)WearLevel);
        }

        public void ReduceWear(double amount)
        {
            WearLevel = Math.Max(0, WearLevel - amount);
            ComponentsHealth = Math.Max(0, 100 - (int)WearLevel);
        }
    }
}
