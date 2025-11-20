using CoffeeMachineWPF.Models;
using System.Windows.Threading;

namespace CoffeeMachineWPF.Services
{
    public class CoffeeBrewingService
    {
        private readonly CoffeeMachine _coffeeMachine;
        private readonly DispatcherTimer _brewTimer;

        public event Action<double>? BrewProgressChanged;
        public event Action<string>? BrewStatusChanged;
        public event Action? BrewCompleted;

        public CoffeeBrewingService(CoffeeMachine coffeeMachine)
        {
            _coffeeMachine = coffeeMachine;
            _brewTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _brewTimer.Tick += OnBrewTimerTick;
        }

        public async Task BrewCoffeeAsync(CoffeeType coffeeType, int sugarLevel)
        {
            double brewTime = _coffeeMachine.CalculateBrewTime(coffeeType, sugarLevel);
            _coffeeMachine.StartBrewing(brewTime);
            _brewTimer.Start();

            await WaitForBrewCompletionAsync();

            BrewCompleted?.Invoke();
        }

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

        private async Task WaitForBrewCompletionAsync()
        {
            while (_coffeeMachine.IsMakingCoffee)
            {
                await Task.Delay(100);
            }
        }

        public void Dispose()
        {
            _brewTimer.Stop();
            _brewTimer.Tick -= OnBrewTimerTick;
        }
    }
}