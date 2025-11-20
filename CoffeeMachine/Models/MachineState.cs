namespace CoffeeMachineWPF.Models
{
    /// <summary>
    /// Состояние кофемашины с текущими уровнями ресурсов
    /// </summary>
    /// <param name="Water">Количество воды</param>
    /// <param name="Coffee">Количество кофе</param>
    /// <param name="Milk">Количество молока</param>
    /// <param name="Cups">Количество стаканчиков</param>
    /// <param name="Sugar">Количество сахара</param>
    /// <param name="DrinksMade">Количество приготовленных напитков</param>
    /// <param name="WasteLevel">Уровень отходов в процентах</param>
    public record MachineState(
    int Water,
    int Coffee,
    int Milk,
    int Cups,
    int Sugar,
    int DrinksMade,
    double WasteLevel)
    {
        /// <summary>
        /// Создание состояния на основе текущего состояния кофемашины
        /// </summary>
        /// <param name="machine">Экземпляр кофемашины для копирования состояния</param>
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
