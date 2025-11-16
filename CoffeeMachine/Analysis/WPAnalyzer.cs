using CoffeeMachineWPF.Models;
using System.Text;

namespace CoffeeMachineWPF.Analysis
{
    public class WPAnalyzer
    {
        public string AnalyzePreConditions(Dictionary<string, bool> conditions)
        {
            var result = new StringBuilder();
            result.AppendLine("АНАЛИЗ ПРЕДУСЛОВИЙ:");
            result.AppendLine("-------------------");

            foreach (var condition in conditions)
            {
                result.AppendLine($"• {condition.Key}: {(condition.Value ? "ВЫПОЛНЕНО" : "НЕ ВЫПОЛНЕНО")}");
            }

            result.AppendLine($"\nИТОГ: {(conditions.All(c => c.Value) ? "ВСЕ УСЛОВИЯ ВЫПОЛНЕНЫ" : "НЕКОТОРЫЕ УСЛОВИЯ НЕ ВЫПОЛНЕНЫ")}");
            return result.ToString();
        }

        public string AnalyzePostConditions(Dictionary<string, bool> conditions)
        {
            var result = new StringBuilder();
            result.AppendLine("АНАЛИЗ ПОСТУСЛОВИЙ:");
            result.AppendLine("-------------------");

            foreach (var condition in conditions)
            {
                result.AppendLine($"• {condition.Key}: {(condition.Value ? "ВЫПОЛНЕНО" : "НЕ ВЫПОЛНЕНО")}");
            }

            result.AppendLine($"\nИТОГ: {(conditions.All(c => c.Value) ? "ВСЕ УСЛОВИЯ ВЫПОЛНЕНЫ" : "НЕКОТОРЫЕ УСЛОВИЯ НЕ ВЫПОЛНЕНЫ")}");
            return result.ToString();
        }

        public string AnalyzeTimeCalculation(CoffeeType coffeeType, int sugarLevel, bool addMilk, double baseTime)
        {
            var result = new StringBuilder();
            result.AppendLine("АНАЛИЗ РАСЧЕТА ВРЕМЕНИ ПРИГОТОВЛЕНИЯ:");
            result.AppendLine("-------------------------------------");

            result.AppendLine($"• Базовое время: {baseTime:F1} сек");

            var timeFactors = new List<string>();
            double totalTime = baseTime;

            switch (coffeeType)
            {
                case CoffeeType.Espresso:
                    totalTime += 5;
                    timeFactors.Add($"+5.0 сек (Эспрессо - быстрая экстракция)");
                    break;
                case CoffeeType.Americano:
                    totalTime += 10;
                    timeFactors.Add($"+10.0 сек (Американо - добавление воды)");
                    break;
                case CoffeeType.Cappuccino:
                    totalTime += 15;
                    timeFactors.Add($"+15.0 сек (Капучино - взбивание молока)");
                    break;
                case CoffeeType.Latte:
                    totalTime += 18;
                    timeFactors.Add($"+18.0 сек (Латте - сложная подготовка)");
                    break;
            }

            if (sugarLevel > 0)
            {
                double sugarTime = sugarLevel * 0.5;
                totalTime += sugarTime;
                timeFactors.Add($"+{sugarTime:F1} сек (сахар: {sugarLevel} порций)");
            }

            if (addMilk)
            {
                totalTime += 3;
                timeFactors.Add($"+3.0 сек (добавление молока)");
            }

            result.AppendLine("• Факторы влияния:");
            foreach (var factor in timeFactors)
            {
                result.AppendLine($"  {factor}");
            }

            result.AppendLine($"\n• ИТОГОВОЕ ВРЕМЯ: {totalTime:F1} сек");

            result.AppendLine($"\nИНВАРИАНТ: время > 0 && время < 60 сек");
            result.AppendLine($"   {(totalTime > 0 && totalTime < 60 ? "ВЫПОЛНЕН" : "НАРУШЕН")}");

            return result.ToString();
        }

        public string AnalyzeOrderPossibility(Dictionary<string, (int current, int required, bool isMet)> resources)
        {
            var result = new StringBuilder();
            result.AppendLine("АНАЛИЗ ВОЗМОЖНОСТИ ЗАКАЗА:");
            result.AppendLine("---------------------------");

            bool allConditionsMet = true;

            foreach (var resource in resources)
            {
                bool isMet = resource.Value.isMet;
                string status = isMet ? "ДОСТАТОЧНО" : "НЕДОСТАТОЧНО";
                result.AppendLine($"• {resource.Key}: {resource.Value.current}/{resource.Value.required} {status}");

                if (!isMet) allConditionsMet = false;
            }

            result.AppendLine($"\nИТОГ: {(allConditionsMet ? "ЗАКАЗ ВОЗМОЖЕН" : "ЗАКАЗ НЕВОЗМОЖЕН - недостаточно ресурсов")}");

            result.AppendLine($"\nИНВАРИАНТ: все ресурсы >= 0");
            bool invariantMet = resources.All(r => r.Value.current >= 0);
            result.AppendLine($"   {(invariantMet ? "ВЫПОЛНЕН" : "НАРУШЕН")}");

            return result.ToString();
        }

        public string AnalyzeCostCalculation(CoffeeType coffeeType, int sugarLevel, bool addMilk, Dictionary<string, double> prices)
        {
            var result = new StringBuilder();
            result.AppendLine("АНАЛИЗ РАСЧЕТА СТОИМОСТИ:");
            result.AppendLine("--------------------------");

            double baseCost = prices["base"];
            result.AppendLine($"• Базовая стоимость: {baseCost:F2} руб");

            var costComponents = new List<string>();
            double totalCost = baseCost;

            switch (coffeeType)
            {
                case CoffeeType.Espresso:
                    totalCost += 30;
                    costComponents.Add($"+30.00 руб (Эспрессо)");
                    break;
                case CoffeeType.Americano:
                    totalCost += 40;
                    costComponents.Add($"+40.00 руб (Американо)");
                    break;
                case CoffeeType.Cappuccino:
                    totalCost += 50;
                    costComponents.Add($"+50.00 руб (Капучино)");
                    break;
                case CoffeeType.Latte:
                    totalCost += 55;
                    costComponents.Add($"+55.00 руб (Латте)");
                    break;
            }

            if (sugarLevel > 0)
            {
                double sugarCost = sugarLevel * 5;
                totalCost += sugarCost;
                costComponents.Add($"+{sugarCost:F2} руб (сахар: {sugarLevel} порций)");
            }

            if (addMilk)
            {
                totalCost += 15;
                costComponents.Add($"+15.00 руб (молоко)");
            }

            result.AppendLine("• Компоненты стоимости:");
            foreach (var component in costComponents)
            {
                result.AppendLine($"  {component}");
            }

            result.AppendLine($"\n• ИТОГОВАЯ СТОИМОСТЬ: {totalCost:F2} руб");

            result.AppendLine($"\nИНВАРИАНТ: стоимость > 0 && стоимость < 500 руб");
            result.AppendLine($"   {(totalCost > 0 && totalCost < 500 ? "ВЫПОЛНЕН" : "НАРУШЕН")}");

            return result.ToString();
        }

        public string GenerateFullReport(
            Dictionary<string, bool> preConditions,
            Dictionary<string, bool> postConditions,
            CoffeeType coffeeType, int sugarLevel, bool addMilk,
            Dictionary<string, (int current, int required, bool isMet)> resources,
            Dictionary<string, double> prices)
        {
            var report = new StringBuilder();
            report.AppendLine("ПОЛНЫЙ WP-АНАЛИЗ АЛГОРИТМА КОФЕЙНОЙ МАШИНЫ");
            report.AppendLine("===========================================\n");

            report.AppendLine(AnalyzePreConditions(preConditions));
            report.AppendLine();
            report.AppendLine(AnalyzeOrderPossibility(resources));
            report.AppendLine();
            report.AppendLine(AnalyzeTimeCalculation(coffeeType, sugarLevel, addMilk, 10.0));
            report.AppendLine();
            report.AppendLine(AnalyzeCostCalculation(coffeeType, sugarLevel, addMilk, prices));
            report.AppendLine();
            report.AppendLine(AnalyzePostConditions(postConditions));

            return report.ToString();
        }
    }
}