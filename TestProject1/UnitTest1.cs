using CoffeeMachineWPF.Analysis;
using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Services;
using CoffeeMachineWPF.ViewModels;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test_PreConditions_WhenIngredientSelected()
        {
            var machine = new CoffeeMachine();
            var vm = new AdditionIngredientsVM(machine);
            vm.SelectedIngredientType = IngredientType.Water;
            vm.RefillAmount = 100;
            Assert.True(vm.PreConditionsMet);
        }

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

        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var vm = new AdditionIngredientsVM(machine);
            Assert.Equal("Пополнение ингредиентов", vm.OperationName);
        }
    }

    public class UnitTest2
    {
        [Fact]
        public void Test_InitialState_HasDefaultValues()
        {
            var machine = new CoffeeMachine();
            var analyzer = new CycleAnalyzerService();
            var vm = new CycleAnalysisVM(machine, analyzer);
            Assert.Equal("Выберите процесс для анализа", vm.AnalysisReport);
            Assert.Equal(CycleProcessType.WaterHeating, vm.SelectedProcess);
        }

        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var analyzer = new CycleAnalyzerService();
            var vm = new CycleAnalysisVM(machine, analyzer);
            Assert.Equal("Анализ циклов", vm.OperationName);
        }
    }

    public class UnitTest3
    {
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

        [Fact]
        public void Test_OperationName_IsCorrect()
        {
            var machine = new CoffeeMachine();
            var vm = new MaintenanceServiceVM(machine);

            Assert.Equal("Техническое обслуживание", vm.OperationName);
        }
    }
    public class UnitTest4
    {
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
    public class UnitTest5
    {
        [Fact]
        public void Test_StateControllerVM_Creation()
        {
            var machine = new CoffeeMachine();
            var stateController = new StateControllerService(machine);
            var vm = new StateControllerVM(machine, stateController);
            Assert.NotNull(vm);
            Assert.Equal("Логический контроллер", vm.OperationName);
        }

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