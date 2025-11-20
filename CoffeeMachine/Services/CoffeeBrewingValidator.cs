using CoffeeMachineWPF.Models;

namespace CoffeeMachineWPF.Services
{
    /// <summary>
    /// Валидатор проверки возможности приготовления кофе
    /// </summary>
    public class CoffeeBrewingValidator
    {
        private readonly CoffeeMachine _coffeeMachine;

        /// <summary>
        /// Создание валидатора для указанной кофемашины
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для проверки условий</param>
        public CoffeeBrewingValidator(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
        }

        /// <summary>
        /// Проверка возможности приготовления кофе с заданными параметрами
        /// </summary>
        /// <param name="coffeeType">Тип кофе для приготовления</param>
        /// <param name="sugarLevel">Уровень сахара</param>
        /// <returns>Результат проверки с детализацией условий</returns>
        public BrewingValidationResult Validate(CoffeeType coffeeType, int sugarLevel)
        {
            var recipe = CoffeeRecipe.GetRecipe(coffeeType);

            var conditions = new Dictionary<string, bool>
            {
                ["Машина не сломана"] = !_coffeeMachine.IsBroken,
                ["Машина не готовит напиток"] = !_coffeeMachine.IsMakingCoffee,
                ["Уровень отходов не критический"] = _coffeeMachine.WasteLevel < 90,
                ["Достаточно воды"] = _coffeeMachine.Water >= recipe.RequiredWater,
                ["Достаточно кофе"] = _coffeeMachine.Coffee >= recipe.RequiredCoffee,
                ["Достаточно молока"] = _coffeeMachine.Milk >= recipe.RequiredMilk,
                ["Есть стаканчики"] = _coffeeMachine.Cups > 0,
                ["Достаточно сахара"] = _coffeeMachine.Sugar >= sugarLevel,
            };

            return new BrewingValidationResult(
                conditions.Values.All(x => x),
                conditions
            );
        }

      
    }

    /// <summary>
    /// Результат проверки возможности приготовления кофе
    /// </summary>
    /// <param name="IsValid">Признак успешной проверки всех условий</param>
    /// <param name="Conditions">Словарь проверенных условий с результатами</param>
    public record BrewingValidationResult(bool IsValid, Dictionary<string, bool> Conditions);
}