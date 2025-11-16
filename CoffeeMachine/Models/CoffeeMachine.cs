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

        public double CurrentBrewTime { get; set; }
        public double TotalBrewTime { get; set; }
        public DateTime? BrewStartTime { get; set; }

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

        public double CalculateBrewTime(CoffeeType coffeeType, int sugarLevel, bool addMilk)
        {
            double baseTime = 10.0;

            switch (coffeeType)
            {
                case CoffeeType.Espresso:
                    baseTime += 5;
                    break;
                case CoffeeType.Americano:
                    baseTime += 10;
                    break;
                case CoffeeType.Cappuccino:
                    baseTime += 15;
                    break;
                case CoffeeType.Latte:
                    baseTime += 18;
                    break;
            }

            if (sugarLevel > 0)
            {
                baseTime += sugarLevel * 0.5;
            }

            if (addMilk)
            {
                baseTime += 3;
            }

            if (WearLevel > 50)
            {
                baseTime += WearLevel * 0.1;
            }

            if (Temperature < 90 || Temperature > 98)
            {
                baseTime += 2;
            }

            return Math.Round(baseTime, 1);
        }

        public void StartBrewing(double totalTime)
        {
            IsMakingCoffee = true;
            TotalBrewTime = totalTime;
            CurrentBrewTime = 0;
            BrewStartTime = DateTime.Now;
        }

        public void UpdateBrewProgress()
        {
            if (IsMakingCoffee && BrewStartTime.HasValue)
            {
                var elapsed = (DateTime.Now - BrewStartTime.Value).TotalSeconds;
                CurrentBrewTime = Math.Min(elapsed, TotalBrewTime);

                if (CurrentBrewTime >= TotalBrewTime)
                {
                    CompleteBrewing();
                }
            }
        }
        public void CompleteBrewing()
        {
            IsMakingCoffee = false;
            CurrentBrewTime = TotalBrewTime;
            BrewStartTime = null;
        }
    }
}