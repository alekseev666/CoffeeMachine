using CoffeeMachineWPF.Analysis;
using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Services;
using CoffeeMachineWPF.ViewModels;
using Xunit;

namespace TestProject1
{
    /// <summary>
    /// Тесты для модели представления пополнения ингредиентов
    /// </summary>
    public class UnitTest1
    {
        /// <summary>
        /// Проверка выполнения предусловий при выборе ингредиента
        /// </summary>
        [Fact]
        public void Test_PreConditions_WhenIngredientSelected()
        {
            var machine = new CoffeeMachine();
            var vm = new AdditionIngredientsVM(machine);
            vm.SelectedIngredientType = IngredientType.Water;
            vm.RefillAmount = 100;
            Assert.True(vm.PreConditionsMet);
        }

        /// <summary>
        /// Проверка выполнения постусловий после успешного пополнения
        /// </summary>
        [Fact]
        public void Test_PostCondition_AfterSuccessfulRefill()
        {
            var machine = new CoffeeMachine();
            var vm = new AdditionIngredientsVM(machine);
            vm.SelectedIngredientType = IngredientType.Water;
            vm.RefillAmount = 100;
            vm.RefillCommand.Execute(null);
            Assert.True(vm.PostConditionMet);
        }

        /// <summary>
        /// Проверка корректности названия операции
        /// </summary>
        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var vm = new AdditionIngredientsVM(machine);
            Assert.Equal("Пополнение ингредиентов", vm.OperationName);
        }
    }

    /// <summary>
    /// Тесты для модели представления анализа циклов
    /// </summary>
    public class UnitTest2
    {
        /// <summary>
        /// Проверка начального состояния модели представления
        /// </summary>
        [Fact]
        public void Test_InitialState_HasDefaultValues()
        {
            var machine = new CoffeeMachine();
            var analyzer = new CycleAnalyzerService();
            var vm = new CycleAnalysisVM(machine, analyzer);
            Assert.Equal("Выберите процесс для анализа", vm.AnalysisReport);
            Assert.Equal(CycleProcessType.WaterHeating, vm.SelectedProcess);
        }

        /// <summary>
        /// Проверка корректности названия операции
        /// </summary>
        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var analyzer = new CycleAnalyzerService();
            var vm = new CycleAnalysisVM(machine, analyzer);
            Assert.Equal("Анализ циклов", vm.OperationName);
        }
    }

    /// <summary>
    /// Тесты для модели представления технического обслуживания
    /// </summary>
    public class UnitTest3
    {
        /// <summary>
        /// Проверка успешного выполнения очистки
        /// </summary>
        [Fact]
        public void Test_PerformCleaning_Success()
        {
            var machine = new CoffeeMachine();
            machine.IncreaseWear(20); 
            var vm = new MaintenanceServiceVM(machine);
            vm.SelectedMaintenanceType = MaintenanceType.Cleaning;

            vm.PerformMaintenanceCommand.Execute(null);

            Assert.Equal(0, machine.WasteLevel);
            Assert.True(machine.MaintenanceCount > 0);
            Assert.True(vm.PostConditionMet); 
        }

        /// <summary>
        /// Проверка корректности названия операции
        /// </summary>
        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var vm = new MaintenanceServiceVM(machine);

            Assert.Equal("Техническое обслуживание", vm.OperationName);
        }
    }

    /// <summary>
    /// Тесты для модели представления приготовления кофе
    /// </summary>
    public class UnitTest4
    {
        /// <summary>
        /// Проверка доступности ресурсов для приготовления эспрессо
        /// </summary>
        [Fact]
        public void Test_MakeEspresso_ResourcesAvailable()
        {

            var machine = new CoffeeMachine();
            var brewingService = new CoffeeBrewingService(machine);
            var validator = new CoffeeBrewingValidator(machine);
            var analyzer = new WPAnalyzer();
            var vm = new MakeCoffeeVM(machine, brewingService, validator, analyzer);
            vm.SelectedCoffeeType = CoffeeType.Espresso;
            vm.SugarLevel = 1;
            Assert.True(vm.HasEnoughWater);
            Assert.True(vm.HasEnoughCoffee);
            Assert.True(vm.HasCups);
            Assert.True(vm.HasSugar);
            Assert.True(vm.PreConditionsMet);
            Assert.Equal("Кофе не готово", vm.BrewResultMessage); 
        }

        /// <summary>
        /// Проверка выполнения предусловий при достаточных ресурсах
        /// </summary>
        [Fact]
        public void Test_PreConditions_WhenEnoughResources()
        {
            var machine = new CoffeeMachine();
            var brewingService = new CoffeeBrewingService(machine);
            var validator = new CoffeeBrewingValidator(machine);
            var analyzer = new WPAnalyzer();
            var vm = new MakeCoffeeVM(machine, brewingService, validator, analyzer);
            vm.SelectedCoffeeType = CoffeeType.Americano;
            Assert.True(vm.PreConditionsMet);
        }

        /// <summary>
        /// Проверка корректности названия операции
        /// </summary>
        [Fact]
        public void Test_OperationName_Correct()
        {
            var machine = new CoffeeMachine();
            var brewingService = new CoffeeBrewingService(machine);
            var validator = new CoffeeBrewingValidator(machine);
            var analyzer = new WPAnalyzer();
            var vm = new MakeCoffeeVM(machine, brewingService, validator, analyzer);
            Assert.Equal("Приготовление напитка", vm.OperationName);
        }
    }

    /// <summary>
    /// Тесты для модели представления логического контроллера
    /// </summary>
    public class UnitTest5
    {
        /// <summary>
        /// Проверка создания модели представления контроллера
        /// </summary>
        [Fact]
        public void Test_StateControllerVM_Creation()
        {
            var machine = new CoffeeMachine();
            var stateController = new StateControllerService(machine);
            var vm = new StateControllerVM(machine, stateController);
            Assert.NotNull(vm);
            Assert.Equal("Логический контроллер", vm.OperationName);
        }

        /// <summary>
        /// Проверка работы команды анализа состояния
        /// </summary>
        [Fact]
        public void Test_AnalyzeStateCommand_Works()
        {
            var machine = new CoffeeMachine();
            var stateController = new StateControllerService(machine);
            var vm = new StateControllerVM(machine, stateController);
            vm.AnalyzeStateCommand.Execute(null);
            Assert.NotNull(vm.StateAnalysisReport);
            Assert.NotEqual("Нажмите 'Анализировать' для проверки состояния", vm.StateAnalysisReport);
        }

        /// <summary>
        /// Проверка инициализации текущего статуса
        /// </summary>
        [Fact]
        public void Test_CurrentStatus_Initialized()
        {
            var machine = new CoffeeMachine();
            var stateController = new StateControllerService(machine);
            var vm = new StateControllerVM(machine, stateController);
            Assert.NotNull(vm.CurrentStatus);
            Assert.NotNull(vm.CurrentModeDisplay);
            Assert.NotNull(vm.StatusColor);
        }
    }
}