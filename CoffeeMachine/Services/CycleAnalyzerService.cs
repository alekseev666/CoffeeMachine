using CoffeeMachineWPF.Models.CycleAnalysis;
using System.Text;

namespace CoffeeMachineWPF.Services
{
    public class CycleAnalyzerService
    {
        public CycleAnalysisResult AnalyzeWaterHeating(double currentTemp, double targetTemp, double heatingRate, double maxSafeTemp)
        {
            var result = new CycleAnalysisResult();
            var steps = new List<string>();

            double temp = currentTemp;
            int iterations = 0;
            bool invariantMaintained = true;
            bool variantValid = true;

            steps.Add("НАЧАЛО ПРОЦЕССА НАГРЕВА ВОДЫ");
            steps.Add($"Начальная температура: {currentTemp}°C");
            steps.Add($"Целевая температура: {targetTemp}°C");
            steps.Add($"Скорость нагрева: {heatingRate}°C/итерация");
            steps.Add($"Макс. безопасная температура: {maxSafeTemp}°C");
            steps.Add("");

            while (temp < targetTemp)
            {
                iterations++;
                double previousTemp = temp;
                double previousVariant = targetTemp - temp;

                bool invariantBefore = currentTemp <= temp && temp <= maxSafeTemp;
                if (!invariantBefore)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА на итерации {iterations}: {temp}°C не в диапазоне [{currentTemp}, {maxSafeTemp}]");
                }

                temp += heatingRate;

                if (temp > maxSafeTemp)
                {
                    steps.Add($"ПРЕРЫВАНИЕ ЦИКЛА: температура {temp}°C превысила безопасную {maxSafeTemp}°C");
                    break;
                }

                bool invariantAfter = currentTemp <= temp && temp <= maxSafeTemp;
                if (!invariantAfter)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА после итерации {iterations}: {temp}°C не в диапазоне [{currentTemp}, {maxSafeTemp}]");
                }

                double currentVariant = targetTemp - temp;
                if (currentVariant >= previousVariant)
                {
                    variantValid = false;
                    steps.Add($"НАРУШЕНИЕ ВАРИАНТА: {currentVariant} не уменьшился относительно {previousVariant}");
                }

                steps.Add($"Итерация {iterations}: {previousTemp}°C → {temp}°C | Вариант: {currentVariant:F2}");
            }

            steps.Add("");
            steps.Add("РЕЗУЛЬТАТ АНАЛИЗА:");
            steps.Add($"• Итераций выполнено: {iterations}");
            steps.Add($"• Конечная температура: {temp}°C");
            steps.Add($"• Инвариант сохранен: {(invariantMaintained ? "ДА" : "НЕТ")}");
            steps.Add($"• Вариант корректен: {(variantValid ? "ДА" : "НЕТ")}");

            result.IsInvariantMaintained = invariantMaintained;
            result.IsVariantValid = variantValid;
            result.Iterations = iterations;
            result.Steps = steps;
            result.Conclusion = invariantMaintained && variantValid ?
                "Цикл корректен: инвариант сохранен, вариант уменьшается" :
                "Цикл содержит ошибки";

            return result;
        }

        public CycleAnalysisResult AnalyzeTankCleaning(double currentWaste, double cleaningRate)
        {
            var result = new CycleAnalysisResult();
            var steps = new List<string>();

            double wasteLevel = currentWaste;
            int iterations = 0;
            bool invariantMaintained = true;
            bool variantValid = true;
            double initialWaste = currentWaste;

            steps.Add("НАЧАЛО ПРОЦЕССА ОЧИСТКИ БАКОВ");
            steps.Add($"Начальный уровень отходов: {currentWaste}%");
            steps.Add($"Скорость очистки: {cleaningRate}%/итерация");
            steps.Add("");

            while (wasteLevel > 0)
            {
                iterations++;
                double previousWaste = wasteLevel;

                bool invariantBefore = 0 <= wasteLevel && wasteLevel <= initialWaste;
                if (!invariantBefore)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА на итерации {iterations}: {wasteLevel}% не в диапазоне [0, {initialWaste}]");
                }

                wasteLevel -= cleaningRate;

                if (wasteLevel < 0)
                {
                    wasteLevel = 0;
                    steps.Add("КОРРЕКЦИЯ: уровень отходов установлен в 0%");
                }

                bool invariantAfter = 0 <= wasteLevel && wasteLevel <= initialWaste;
                if (!invariantAfter)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА после итерации {iterations}: {wasteLevel}% не в диапазоне [0, {initialWaste}]");
                }

                if (wasteLevel >= previousWaste)
                {
                    variantValid = false;
                    steps.Add($"НАРУШЕНИЕ ВАРИАНТА: {wasteLevel} не уменьшился относительно {previousWaste}");
                }

                steps.Add($"Итерация {iterations}: {previousWaste}% → {wasteLevel}% | Вариант: {wasteLevel:F2}");
            }

            steps.Add("");
            steps.Add("РЕЗУЛЬТАТ АНАЛИЗА:");
            steps.Add($"• Итераций выполнено: {iterations}");
            steps.Add($"• Конечный уровень отходов: {wasteLevel}%");
            steps.Add($"• Инвариант сохранен: {(invariantMaintained ? "ДА" : "НЕТ")}");
            steps.Add($"• Вариант корректен: {(variantValid ? "ДА" : "НЕТ")}");

            result.IsInvariantMaintained = invariantMaintained;
            result.IsVariantValid = variantValid;
            result.Iterations = iterations;
            result.Steps = steps;
            result.Conclusion = invariantMaintained && variantValid ?
                "Цикл корректен: инвариант сохранен, вариант уменьшается" :
                "Цикл содержит ошибки";

            return result;
        }

        public CycleAnalysisResult AnalyzeDispenserTesting(int numDispensers, Func<int, bool> testDispenser)
        {
            var result = new CycleAnalysisResult();
            var steps = new List<string>();

            int testsPassed = 0;
            int i = 0;
            bool invariantMaintained = true;
            bool variantValid = true;

            steps.Add("НАЧАЛО АВТОМАТИЧЕСКОГО ТЕСТИРОВАНИЯ ДОЗАТОРОВ");
            steps.Add($"Количество дозаторов: {numDispensers}");
            steps.Add("");

            while (i < numDispensers)
            {
                double previousVariant = numDispensers - i;

                bool invariantBefore = 0 <= testsPassed && testsPassed <= i && i <= numDispensers;
                if (!invariantBefore)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА на итерации {i}: testsPassed={testsPassed}, i={i}");
                }

                bool testResult = testDispenser(i);
                if (testResult)
                {
                    testsPassed++;
                }

                steps.Add($"Тест дозатора {i + 1}: {(testResult ? "ПРОЙДЕН" : "НЕ ПРОЙДЕН")}");

                i++;

                bool invariantAfter = 0 <= testsPassed && testsPassed <= i && i <= numDispensers;
                if (!invariantAfter)
                {
                    invariantMaintained = false;
                    steps.Add($"НАРУШЕНИЕ ИНВАРИАНТА после итерации {i}: testsPassed={testsPassed}, i={i}");
                }

                double currentVariant = numDispensers - i;
                if (currentVariant >= previousVariant)
                {
                    variantValid = false;
                    steps.Add($"НАРУШЕНИЕ ВАРИАНТА: {currentVariant} не уменьшился относительно {previousVariant}");
                }

                steps.Add($"Итерация {i}: testsPassed={testsPassed} | Вариант: {currentVariant}");
            }

            steps.Add("");
            steps.Add("РЕЗУЛЬТАТ АНАЛИЗА:");
            steps.Add($"• Протестировано дозаторов: {i}");
            steps.Add($"• Успешных тестов: {testsPassed}/{numDispensers}");
            steps.Add($"• Инвариант сохранен: {(invariantMaintained ? "ДА" : "НЕТ")}");
            steps.Add($"• Вариант корректен: {(variantValid ? "ДА" : "НЕТ")}");

            result.IsInvariantMaintained = invariantMaintained;
            result.IsVariantValid = variantValid;
            result.Iterations = i;
            result.Steps = steps;
            result.Conclusion = invariantMaintained && variantValid ?
                "Цикл корректен: инвариант сохранен, вариант уменьшается" :
                "Цикл содержит ошибки";

            return result;
        }

        public string GenerateCycleAnalysisReport(CycleProcess process, CycleAnalysisResult result)
        {
            var report = new StringBuilder();

            report.AppendLine("АНАЛИЗ ЦИКЛИЧЕСКОГО ПРОЦЕССА");
            report.AppendLine("=============================");
            report.AppendLine($"Процесс: {process.Name}");
            report.AppendLine($"Описание: {process.Description}");
            report.AppendLine();

            report.AppendLine("КОД ПРОЦЕССА:");
            report.AppendLine(process.Code);
            report.AppendLine();

            report.AppendLine("ПАРАМЕТРЫ АНАЛИЗА:");
            report.AppendLine($"• Инвариант: {process.Invariant}");
            report.AppendLine($"• Вариант: {process.Variant}");
            report.AppendLine();

            report.AppendLine("РЕЗУЛЬТАТЫ ВЫПОЛНЕНИЯ:");
            foreach (var step in result.Steps)
            {
                report.AppendLine(step);
            }
            report.AppendLine();

            report.AppendLine("ИТОГОВЫЙ ВЕРДИКТ:");
            report.AppendLine(result.Conclusion);

            return report.ToString();
        }
    }
}