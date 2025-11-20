namespace CoffeeMachineWPF.Models
{
    /// <summary>
    /// Рецепт приготовления кофе с требуемыми ингредиентами
    /// </summary>
    public record CoffeeRecipe
    {
        /// <summary>
        /// Требуемое количество воды 
        /// </summary>
        public int RequiredWater { get; }

        /// <summary>
        /// Требуемое количество кофе
        /// </summary>
        public int RequiredCoffee { get; }

        /// <summary>
        /// Требуемое количество молока в миллилитрах
        /// </summary>
        public int RequiredMilk { get; }

        /// <summary>
        /// Конструктор для создания рецепта с заданными количествами ингредиентов
        /// </summary>
        private CoffeeRecipe(int requiredWater, int requiredCoffee, int requiredMilk)
        {
            RequiredWater = requiredWater;
            RequiredCoffee = requiredCoffee;
            RequiredMilk = requiredMilk;
        }

        /// <summary>
        /// Получение рецепта для указанного типа кофе
        /// </summary>
        /// <param name="type">Тип кофе для получения рецепта</param>
        /// <returns>Рецепт приготовления кофе</returns>
        /// <exception cref="ArgumentException">Неизвестный тип кофе</exception>
        public static CoffeeRecipe GetRecipe(CoffeeType type)
        {
            return type switch
            {
                CoffeeType.Espresso => new(50, 15, 0),
                CoffeeType.Americano => new(150, 15, 0),
                CoffeeType.Cappuccino => new(100, 15, 50),
                CoffeeType.Latte => new(50, 15, 100),

                _ => throw new ArgumentException("Не знаю такой кофе.")
            };
        }

    }
}
