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
                ["Достаточно сахара"] = _coffeeMachine.Sugar >= sugarLevel,
                ["Молоко совместимо с типом кофе"] = IsMilkCompatibleWithCoffeeType(coffeeType, addMilk)
            };

            return new BrewingValidationResult(
                conditions.Values.All(x => x),
                conditions
            );
        }

        private bool IsMilkCompatibleWithCoffeeType(CoffeeType coffeeType, bool addMilk)
        {
            // Для Espresso и Americano молоко не допускается
            if ((coffeeType == CoffeeType.Espresso || coffeeType == CoffeeType.Americano) && addMilk)
            {
                return false;
            }

            return true;
        }
    }

    public record BrewingValidationResult(bool IsValid, Dictionary<string, bool> Conditions);
}