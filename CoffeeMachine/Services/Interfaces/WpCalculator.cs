namespace CoffeeMachineWPF.Services.Interfaces
{
    /// <summary>
    /// Калькулятор weakest precondition (WP) для верификации алгоритмов
    /// </summary>
    public interface IWpCalculator
    {
        /// <summary>
        /// Вычисляет weakest precondition для заданного кода и постусловия
        /// </summary>
        /// <param name="code">Код алгоритма для анализа</param>
        /// <param name="postCondition">Постусловие, которое должно выполняться после кода</param>
        /// <returns>Weakest precondition в виде строки</returns>
        string CalculateWp(string code, string postCondition);
    }
}
