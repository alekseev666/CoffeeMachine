
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeMachineWPF.ViewModels
{
    public partial class AppInfoVM : OperationViewModelBase
    {
        public override string OperationName => "О приложении";

        public AppInfoVM() : base(null) { }
    }
}