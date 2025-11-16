using CoffeeMachineWPF.Models;

namespace CoffeeMachineWPF.Services
{
    public class CoffeeBrewingValidator
    {
        private readonly CoffeeMachine _coffeeMachine;

        public CoffeeBrewingValidator(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
        }

        public BrewingValidationResult Validate(CoffeeType coffeeType, int sugarLevel, bool addMilk)
        {
            var recipe = CoffeeRecipe.GetRecipe(coffeeType);

            var conditions = new Dictionary<string, bool>
            {
                ["Машина не сломана"] = !_coffeeMachine.IsBroken,
                ["Машина не готовит напиток"] = !_coffeeMachine.IsMakingCoffee,
                ["Уровень отходов не критический"] = _coffeeMachine.WasteLevel < 90,
                ["Достаточно воды"] = _coffeeMachine.Water >= recipe.RequiredWater,
                ["Достаточно кофе"] = _coffeeMachine.Coffee >= recipe.RequiredCoffee,
                ["Достаточно молока"] = !addMilk || _coffeeMachine.Milk >= recipe.RequiredMilk,
                ["Есть стаканчики"] = _coffeeMachine.Cups > 0,
                ["Достаточно сахара"] = _coffeeMachine.Sugar >= sugarLevel
            };

            return new BrewingValidationResult(
                conditions.Values.All(x => x),
                conditions
            );
        }
    }

    public record BrewingValidationResult(bool IsValid, Dictionary<string, bool> Conditions);
}