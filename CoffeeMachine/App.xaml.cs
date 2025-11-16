using CoffeeMachineWPF.Analysis;
using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Services;
using CoffeeMachineWPF.ViewModels;
using CoffeeMachineWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CoffeeMachineWPF
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // модели
            services.AddSingleton<CoffeeMachine>();

            // сервисы
            services.AddTransient<CoffeeBrewingService>();
            services.AddTransient<CoffeeBrewingValidator>();
            services.AddSingleton<WPAnalyzer>();
            services.AddSingleton<CycleAnalyzerService>();

            // ViewModels
            services.AddTransient<MakeCoffeeVM>();
            services.AddTransient<AdditionIngredientsVM>();
            services.AddTransient<MaintenanceServiceVM>();
            services.AddTransient<CycleAnalysisVM>();
            services.AddSingleton<MainWindowVM>();

            // Views
            services.AddSingleton<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}