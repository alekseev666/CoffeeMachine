namespace CoffeeMachineWPF.Models.CycleAnalysis
{
    public class CycleProcess
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Invariant { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, double> Variables { get; set; } = new();
    }
}
