using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeMachineWPF.ViewModels
{
    /// <summary>
    /// Модель представления для отображения информации о приложении
    /// </summary>
    public partial class AppInfoVM : OperationViewModelBase
    {
        /// <summary>
        /// Название операции 
        /// </summary>
        public override string OperationName => "Справка о приложении";

        /// <summary>
        /// Создание модели представления информации о приложении
        /// </summary>
        /// <remarks>
        /// Передает null в базовый конструктор, так как эта модель представления
        /// не требует доступа к функциональности кофемашины
        /// </remarks>
        public AppInfoVM() : base(null) { }
    }
}