using CoffeeMachineWPF.Models;
using System.Windows.Threading;

namespace CoffeeMachineWPF.Services
{
    /// <summary>
    /// Сервис управления процессом приготовления кофе
    /// </summary>
    public class CoffeeBrewingService
    {

        private readonly CoffeeMachine _coffeeMachine;
        private readonly DispatcherTimer _brewTimer;

        /// <summary>
        /// Событие изменения прогресса приготовления кофе
        /// </summary>
        public event Action<double>? BrewProgressChanged;

        /// <summary>
        /// Событие изменения статуса приготовления
        /// </summary>
        public event Action<string>? BrewStatusChanged;

        /// <summary>
        /// Событие завершения процесса приготовления
        /// </summary>
        public event Action? BrewCompleted;

        /// <summary>
        /// Создание сервиса приготовления кофе для указанной кофемашины
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для управления процессом</param>
        public CoffeeBrewingService(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
            _brewTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _brewTimer.Tick += OnBrewTimerTick;
        }

        /// <summary>
        /// Запуск асинхронного процесса приготовления кофе
        /// </summary>
        /// <param name="coffeeType">Тип кофе для приготовления</param>
        /// <param name="sugarLevel">Уровень сахара</param>
        /// <returns>Задача, представляющая процесс приготовления</returns>
        public async Task BrewCoffeeAsync(CoffeeType coffeeType, int sugarLevel)
        {
            double brewTime = _coffeeMachine.CalculateBrewTime(coffeeType, sugarLevel);
            _coffeeMachine.StartBrewing(brewTime);
            _brewTimer.Start();

            await WaitForBrewCompletionAsync();

            BrewCompleted?.Invoke();
        }

        /// <summary>
        /// Обработчик тика таймера приготовления кофе
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void OnBrewTimerTick(object sender, EventArgs e)
        {
            _coffeeMachine.UpdateBrewProgress();

            if (_coffeeMachine.TotalBrewTime > 0)
            {
                double progress = _coffeeMachine.CurrentBrewTime / _coffeeMachine.TotalBrewTime * 100;
                BrewProgressChanged?.Invoke(progress);
            }

            BrewStatusChanged?.Invoke(
                $"Приготовление... {_coffeeMachine.CurrentBrewTime:F1}/{_coffeeMachine.TotalBrewTime:F1} сек");

            if (!_coffeeMachine.IsMakingCoffee)
            {
                _brewTimer.Stop();
            }
        }

        /// <summary>
        /// Ожидание завершения процесса приготовления кофе
        /// </summary>
        /// <returns>Задача, завершающаяся при окончании приготовления</returns>
        private async Task WaitForBrewCompletionAsync()
        {
            while (_coffeeMachine.IsMakingCoffee)
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// Освобождение ресурсов сервиса
        /// </summary>
        public void Dispose()
        {
            _brewTimer.Stop();
            _brewTimer.Tick -= OnBrewTimerTick;
        }
    }
}