using CoffeeMachineWPF.ViewModels;
using CoffeeMachineWPF.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CoffeeMachineWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var vm = new MainWindowVM();
            var mainWindow = new MainWindow(vm);

            mainWindow.Show();
        }
    }

}
