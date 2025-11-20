using CoffeeMachineWPF.Models;
using CoffeeMachineWPF.Models.CycleAnalysis;
using CoffeeMachineWPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Модель представления для анализа циклических процессов кофемашины
    /// </summary>
    public partial class CycleAnalysisVM : OperationViewModelBase
    {
        private readonly CycleAnalyzerService _cycleAnalyzer;

        /// <summary>
        /// Отчет анализа циклического процесса
        /// </summary>
        [ObservableProperty]
        private string _analysisReport = "Выберите процесс для анализа";

        /// <summary>
        /// Выбранный тип процесса для анализа
        /// </summary>
        [ObservableProperty]
        private CycleProcessType _selectedProcess = CycleProcessType.WaterHeating;

        /// <summary>
        /// Текущая температура воды для анализа нагрева
        /// </summary>
        [ObservableProperty]
        private double _currentTemperature = 20;

        /// <summary>
        /// Целевая температура воды для анализа нагрева
        /// </summary>
        [ObservableProperty]
        private double _targetTemperature = 95;

        /// <summary>
        /// Скорость нагрева воды за итерацию
        /// </summary>
        [ObservableProperty]
        private double _heatingRate = 5;

        /// <summary>
        /// Максимальная безопасная температура воды
        /// </summary>
        [ObservableProperty]
        private double _maxSafeTemperature = 100;

        /// <summary>
        /// Текущий уровень отходов для анализа очистки
        /// </summary>
        [ObservableProperty]
        private double _currentWasteLevel = 50;

        /// <summary>
        /// Скорость очистки отходов за итерацию
        /// </summary>
        [ObservableProperty]
        private double _cleaningRate = 10;

        /// <summary>
        /// Количество дозаторов для тестирования
        /// </summary>
        [ObservableProperty]
        private int _numberOfDispensers = 5;

        /// <summary>
        /// Название операции анализа циклов
        /// </summary>
        public override string OperationName => "Анализ циклов";

        /// <summary>
        /// Доступные типы процессов для анализа
        /// </summary>
        public IEnumerable<CycleProcessType> AvailableProcesses =>
            Enum.GetValues<CycleProcessType>();

        /// <summary>
        /// Создание модели представления для анализа циклических процессов
        /// </summary>
        /// <param name="coffeeMachine">Кофемашина для анализа</param>
        /// <param name="cycleAnalyzer">Сервис анализа циклических процессов</param>
        public CycleAnalysisVM(CoffeeMachine coffeeMachine, CycleAnalyzerService cycleAnalyzer)
            : base(coffeeMachine)
        {
            _cycleAnalyzer = cycleAnalyzer;
        }

        /// <summary>
        /// Команда анализа выбранного циклического процесса
        /// </summary>
        [RelayCommand]
        private void AnalyzeCycle()
        {
            try
            {
                CycleAnalysisResult result;
                CycleProcess process;

                switch (SelectedProcess)
                {
                    case CycleProcessType.WaterHeating:
                        result = _cycleAnalyzer.AnalyzeWaterHeating(
                            CurrentTemperature, TargetTemperature, HeatingRate, MaxSafeTemperature);

                        process = new CycleProcess
                        {
                            Name = "Нагрев воды",
                            Code = $"temp := {CurrentTemperature};\nwhile (temp < {TargetTemperature}) {{\n    temp := temp + {HeatingRate};\n    if (temp > {MaxSafeTemperature}) break;\n}}",
                            Invariant = $"{CurrentTemperature} ≤ temp ≤ {MaxSafeTemperature}",
                            Variant = $"{TargetTemperature} - temp",
                            Description = "Процесс нагрева воды до целевой температуры с проверкой безопасности"
                        };
                        break;

                    case CycleProcessType.TankCleaning:
                        result = _cycleAnalyzer.AnalyzeTankCleaning(CurrentWasteLevel, CleaningRate);

                        process = new CycleProcess
                        {
                            Name = "Очистка баков",
                            Code = $"wasteLevel := {CurrentWasteLevel};\nwhile (wasteLevel > 0) {{\n    wasteLevel := wasteLevel - {CleaningRate};\n    if (wasteLevel < 0) wasteLevel := 0;\n}}",
                            Invariant = $"0 ≤ wasteLevel ≤ {CurrentWasteLevel}",
                            Variant = "wasteLevel",
                            Description = "Процесс очистки баков от отходов"
                        };
                        break;

                    case CycleProcessType.DispenserTesting:
                        result = _cycleAnalyzer.AnalyzeDispenserTesting(NumberOfDispensers, TestDispenserSimulation);

                        process = new CycleProcess
                        {
                            Name = "Тестирование дозаторов",
                            Code = $"testsPassed := 0;\ni := 0;\nwhile (i < {NumberOfDispensers}) {{\n    if (testDispenser(i)) testsPassed := testsPassed + 1;\n    i := i + 1;\n}}",
                            Invariant = $"0 ≤ testsPassed ≤ i ≤ {NumberOfDispensers}",
                            Variant = $"{NumberOfDispensers} - i",
                            Description = "Автоматическое тестирование всех дозаторов кофейной машины"
                        };
                        break;

                    default:
                        AnalysisReport = "❌ Выбран неизвестный процесс";
                        return;
                }

                AnalysisReport = _cycleAnalyzer.GenerateCycleAnalysisReport(process, result);
            }
            catch (Exception ex)
            {
                AnalysisReport = $"❌ Ошибка при анализе: {ex.Message}";
            }
        }

        /// <summary>
        /// Симуляция тестирования дозатора
        /// </summary>
        /// <param name="dispenserIndex">Индекс тестируемого дозатора</param>
        /// <returns>Результат тестирования дозатора</returns>
        private bool TestDispenserSimulation(int dispenserIndex)
        {
            // Симуляция тестирования дозатора
            // 80% шанс успешного теста
            Random rand = new Random();
            return rand.NextDouble() > 0.2;
        }

        /// <summary>
        /// Обработчик изменения выбранного процесса
        /// </summary>
        /// <param name="value">Новый тип процесса</param>
        partial void OnSelectedProcessChanged(CycleProcessType value)
        {
            AnalysisReport = "Выберите процесс для анализа";
        }
    }

    /// <summary>
    /// Тип циклического процесса для анализа
    /// </summary>
    public enum CycleProcessType
    {
        WaterHeating, // Процесс нагрева воды
        TankCleaning, // Процесс очистки баков
        DispenserTesting // Процесс тестирования дозаторов
    }
}