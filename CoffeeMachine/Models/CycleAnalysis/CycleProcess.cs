namespace CoffeeMachineWPF.Models.CycleAnalysis
{
    /// <summary>
    /// Циклический процесс кофемашины для анализа корректности
    /// </summary>
    public class CycleProcess
    {
        /// <summary>
        /// Название циклического процесса
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Реализация цикла
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Условие-инвариант цикла
        /// </summary>
        public string Invariant { get; set; } = string.Empty;
        /// <summary>
        /// Функция варианта, обеспечивающая завершение цикла
        /// </summary>
        public string Variant { get; set; } = string.Empty;
        /// <summary>
        /// Описание процесса и его назначения
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Переменные и их начальные значения для анализа
        /// </summary>
        public Dictionary<string, double> Variables { get; set; } = new();
    }
}
