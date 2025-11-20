namespace CoffeeMachineWPF.Models.CycleAnalysis
{
    /// <summary>
    /// Результат анализа циклического алгоритма кофемашины
    /// </summary>
    public class CycleAnalysisResult
    {
        /// <summary>
        /// Инвариант цикла сохраняется на всех итерациях
        /// </summary>
        public bool IsInvariantMaintained { get; set; }
        /// <summary>
        /// Вариант цикла корректен и уменьшается
        /// </summary>
        public bool IsVariantValid { get; set; }
        /// <summary>
        /// Количество итераций в цикле
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// Список шагов выполнения цикла с описанием изменений состояния
        /// </summary>
        public List<string> Steps { get; set; } = new();
        // <summary>
        /// Заключение о корректности циклического алгоритма
        /// </summary>
        public string Conclusion { get; set; } = string.Empty;
    }
}
