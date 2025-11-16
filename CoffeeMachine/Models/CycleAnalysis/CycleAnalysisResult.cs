namespace CoffeeMachineWPF.Models.CycleAnalysis
{
    public class CycleAnalysisResult
    {
        public bool IsInvariantMaintained { get; set; }
        public bool IsVariantValid { get; set; }
        public int Iterations { get; set; }
        public List<string> Steps { get; set; } = new();
        public string Conclusion { get; set; } = string.Empty;
    }
}
