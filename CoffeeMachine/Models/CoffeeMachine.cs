namespace CoffeeMachineWPF.Models
{
    /// <summary>
    /// Модель кофемашины с ресурсами и состоянием
    /// </summary>
    public class CoffeeMachine
    {
        /// <summary>
        /// Количество воды в миллилитрах
        /// </summary>
        public int Water { get; set; } = 1000;
        /// <summary>
        /// Количество кофе в граммах
        /// </summary>
        public int Coffee { get; set; } = 500;
        /// <summary>
        /// Количество стаканчиков
        /// </summary>
        public int Cups { get; set; } = 50;
        /// <summary>
        /// Количество молока в миллилитрах
        /// </summary>
        public int Milk { get; set; } = 1000;
        /// <summary>
        /// Количество сахара в граммах
        /// </summary>
        public int Sugar { get; set; } = 200;

        /// <summary>
        /// Температура воды в градусах Цельсия
        /// </summary>
        public double Temperature { get; set; } = 95;
        /// <summary>
        /// Процесс приготовления кофе
        /// </summary>
        public bool IsMakingCoffee { get; set; }
        /// <summary>
        /// Количество приготовленных напитков за текущую сессию
        /// </summary>
        public int DrinksMade { get; set; }
        /// <summary>
        /// Уровень отходов в процентах
        /// </summary>
        public int WasteLevel { get; set; } = 15;
        /// <summary>
        /// Счетчик проведенных обслуживаний
        /// </summary>
        public int MaintenanceCount { get; set; }
        /// <summary>
        /// Общее количество приготовленных напитков
        /// </summary>
        public int TotalDrinksMade { get; set; }
        /// <summary>
        /// Уровень износа оборудования 
        /// </summary>
        public double WearLevel { get; set; } = 0;
        /// <summary>
        /// Здоровье компонентов 
        /// </summary>
        public int ComponentsHealth { get; set; } = 100;
        /// <summary>
        /// Требуется ли техническое обслуживание
        /// </summary>
        public bool NeedsMaintenance => WearLevel > 70;
        /// <summary>
        /// Сломана ли кофемашина
        /// </summary>
        public bool IsBroken => WearLevel >= 100;
        /// <summary>
        /// Текущее время приготовления в секундах
        /// </summary>
        public double CurrentBrewTime { get; set; }
        /// <summary>
        /// Общее время приготовления в секундах
        /// </summary>
        public double TotalBrewTime { get; set; }
        /// <summary>
        /// Время начала процесса приготовления
        /// </summary>
        public DateTime? BrewStartTime { get; set; }

        /// <summary>
        /// Увеличение уровня износа оборудования
        /// </summary>
        /// <param name="amount">Величина увеличения износа</param>
        public void IncreaseWear(double amount)
        {
            WearLevel = Math.Min(100, WearLevel + amount);
            ComponentsHealth = Math.Max(0, 100 - (int)WearLevel);
        }

        /// <summary>
        /// Уменьшение уровня износа оборудования
        /// </summary>
        /// <param name="amount">Величина уменьшения износа</param>
        public void ReduceWear(double amount)
        {
            WearLevel = Math.Max(0, WearLevel - amount);
            ComponentsHealth = Math.Max(0, 100 - (int)WearLevel);
        }

        /// <summary>
        /// Вычисление время приготовления кофе
        /// </summary>
        /// <param name="coffeeType">Тип кофе</param>
        /// <param name="sugarLevel">Уровень сахара</param>
        /// <returns>Время приготовления в секундах</returns>
        public double CalculateBrewTime(CoffeeType coffeeType, int sugarLevel)
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

        /// <summary>
        /// Начало процесса приготовления кофе
        /// </summary>
        /// <param name="totalTime">Общее время приготовления</param>
        public void StartBrewing(double totalTime)
        {
            IsMakingCoffee = true;
            TotalBrewTime = totalTime;
            CurrentBrewTime = 0;
            BrewStartTime = DateTime.Now;
        }

        /// <summary>
        /// Обновление прогресса приготовления кофе
        /// </summary>
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

        /// <summary>
        /// Завершение процесса приготовления кофе
        /// </summary>
        public void CompleteBrewing()
        {
            IsMakingCoffee = false;
            CurrentBrewTime = TotalBrewTime;
            BrewStartTime = null;
        }
    }
}