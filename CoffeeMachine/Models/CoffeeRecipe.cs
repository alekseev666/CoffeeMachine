namespace CoffeeMachineWPF.Models
{
    public record CoffeeRecipe
    {
        public int RequiredWater { get; }
        public int RequiredCoffee { get; }
        public int RequiredMilk { get; }

        private CoffeeRecipe(int requiredWater, int requiredCoffee, int requiredMilk)
        {
            RequiredWater = requiredWater;
            RequiredCoffee = requiredCoffee;
            RequiredMilk = requiredMilk;
        }

        public static CoffeeRecipe GetRecipe(CoffeeType type)
        {
            return type switch
            {
                CoffeeType.Espresso => new(50, 15, 0),
                CoffeeType.Americano => new(150, 15, 0),
                CoffeeType.Cappuccino => new(100, 15, 50),
                CoffeeType.Latte => new(50, 15, 100),

                _ => throw new ArgumentException("не знаю такой кофе")
            };
        }

    }
}
