using CoffeeMachineWPF.Models;
using System.Text;

namespace CoffeeMachineWPF.Analysis
{
    /// <summary>
    /// Анализатор для верификации алгоритмов кофемашины методом weakest precondition
    /// </summary>
    public class WPAnalyzer
    {
        /// <summary>
        /// Анализ предварительных условий работы кофемашины
        /// </summary>
        /// <param name="conditions">Словарь условий, где ключ - название условия, значение - выполнено ли оно</param>
        /// <returns>Отчет о проверке предварительных условий</returns>
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

        /// <summary>
        /// Анализ постусловий работы кофемашины
        /// </summary>
        /// <param name="conditions">Словарь условий, где ключ - название условия, значение - выполнено ли оно</param>
        /// <returns>Отчет о проверке постусловий</returns>
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

        /// <summary>
        /// Анализ расчета времени приготовления кофе
        /// </summary>
        /// <param name="coffeeType">Тип кофе для анализа времени приготовления</param>
        /// <param name="sugarLevel">Уровень сахара (количество порций)</param>
        /// <param name="baseTime">Базовое время приготовления в секундах</param>
        /// <returns>Отчет о расчете времени с учетом всех факторов и проверкой инвариантов</returns>
        public string AnalyzeTimeCalculation(CoffeeType coffeeType, int sugarLevel,  double baseTime)
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

        /// <summary>
        /// Анализ возможности выполнения заказа на основе доступных ресурсов
        /// </summary>
        /// <param name="resources">Словарь ресурсов, где ключ - название ресурса, значение - (текущее количество, требуемое количество, достаточно ли)</param>
        /// <returns>Отчет о проверке достаточности ресурсов для заказа с проверкой инвариантов</returns>
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

        /// <summary>
        /// Анализ расчета стоимости заказа кофе
        /// </summary>
        /// <param name="coffeeType">Тип кофе для расчета стоимости</param>
        /// <param name="sugarLevel">Уровень сахара (количество порций)</param>
        /// <param name="prices">Словарь цен, содержит ключ "base" с базовой ценой</param>
        /// <returns>Отчет о расчете стоимости с разбивкой по компонентам и проверкой инвариантов</returns>
        public string AnalyzeCostCalculation(CoffeeType coffeeType, int sugarLevel,  Dictionary<string, double> prices)
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

        /// <summary>
        /// Генерация полного отчета анализа работы кофемашины
        /// </summary>
        /// <param name="preConditions">Предварительные условия для анализа</param>
        /// <param name="postConditions">Постусловия для анализа</param>
        /// <param name="coffeeType">Тип кофе для анализа</param>
        /// <param name="sugarLevel">Уровень сахара для анализа</param>
        /// <param name="addMilk">Добавлять ли молоко для анализа</param>
        /// <param name="resources">Информация о ресурсах для анализа</param>
        /// <param name="prices">Цены на компоненты для анализа стоимости</param>
        /// <returns>Полный отчет анализа всех аспектов работы кофемашины</returns>
        public string GenerateFullReport(
            Dictionary<string, bool> preConditions,
            Dictionary<string, bool> postConditions,
            CoffeeType coffeeType, int sugarLevel, 
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
            report.AppendLine(AnalyzeTimeCalculation(coffeeType, sugarLevel, 10.0));
            report.AppendLine();
            report.AppendLine(AnalyzeCostCalculation(coffeeType, sugarLevel,  prices));
            report.AppendLine();
            report.AppendLine(AnalyzePostConditions(postConditions));

            // Формула слабейшего условия (WP) и общий вердикт
            var formula = GetOverallWpFormula(preConditions, postConditions, coffeeType, sugarLevel, resources, prices);
            report.AppendLine($"\nWP-ФОРМУЛА: {formula}");

            bool overall = GetOverallWpVerdict(preConditions, postConditions, coffeeType, sugarLevel, resources, prices);
            report.AppendLine($"\nОБЩИЙ ВЕРДИКТ WP: {(overall ? "ВЫПОЛНЕН" : "НАРУШЕН")}");

            return report.ToString();
        }

        /// <summary>
        /// Возвращает текстовое представление формулы WP (слабейшего условия) для переданных данных.
        /// Формула строится как конъюнкция ключевых частей: предусловия ∧ ресурсы ∧ инвариант времени ∧ инвариант стоимости ∧ постусловия.
        /// </summary>
        public string GetOverallWpFormula(
            Dictionary<string, bool> preConditions,
            Dictionary<string, bool> postConditions,
            CoffeeType coffeeType, int sugarLevel,
            Dictionary<string, (int current, int required, bool isMet)> resources,
            Dictionary<string, double> prices)
        {
            string prePart = "TRUE";
            if (preConditions != null && preConditions.Count > 0)
                prePart = string.Join(" ∧ ", preConditions.Keys.Select(k => $"({k})"));

            string resourcesPart = "TRUE";
            if (resources != null && resources.Count > 0)
                resourcesPart = string.Join(" ∧ ", resources.Keys.Select(k => $"({k}_available)") );

            // Время приготовления — то же, что и в анализе времени
            string timePart = "(0 < totalTime < 60)";

            // Стоимость — то же, что и в анализе стоимости
            string costPart = "(0 < totalCost < 500)";

            string postPart = "TRUE";
            if (postConditions != null && postConditions.Count > 0)
                postPart = string.Join(" ∧ ", postConditions.Keys.Select(k => $"({k})"));

            return $"{prePart} ∧ {resourcesPart} ∧ {timePart} ∧ {costPart} ∧ {postPart}";
        }

        /// <summary>
        /// Оценивает единый итоговый WP-вердикт по входным данным.
        /// Возвращает true если все ключевые инварианты и предусловия/постусловия выполнены.
        /// </summary>
        public bool GetOverallWpVerdict(
            Dictionary<string, bool> preConditions,
            Dictionary<string, bool> postConditions,
            CoffeeType coffeeType, int sugarLevel,
            Dictionary<string, (int current, int required, bool isMet)> resources,
            Dictionary<string, double> prices)
        {
            // 1) Предусловия — все должны быть true
            bool preOk = preConditions == null || preConditions.All(p => p.Value);

            // 2) Ресурсы — все позиции должны быть помечены как isMet
            bool resourcesOk = resources == null || resources.All(r => r.Value.isMet);

            // 3) Время приготовления — использовать ту же логику, что и в AnalyzeTimeCalculation
            double totalTime = 10.0;
            switch (coffeeType)
            {
                case CoffeeType.Espresso:
                    totalTime += 5;
                    break;
                case CoffeeType.Americano:
                    totalTime += 10;
                    break;
                case CoffeeType.Cappuccino:
                    totalTime += 15;
                    break;
                case CoffeeType.Latte:
                    totalTime += 18;
                    break;
            }
            if (sugarLevel > 0)
            {
                totalTime += sugarLevel * 0.5;
            }
            bool timeOk = totalTime > 0 && totalTime < 60;

            // 4) Стоимость — та же логика, что и в AnalyzeCostCalculation
            double totalCost = prices != null && prices.ContainsKey("base") ? prices["base"] : 0.0;
            switch (coffeeType)
            {
                case CoffeeType.Espresso:
                    totalCost += 30;
                    break;
                case CoffeeType.Americano:
                    totalCost += 40;
                    break;
                case CoffeeType.Cappuccino:
                    totalCost += 50;
                    break;
                case CoffeeType.Latte:
                    totalCost += 55;
                    break;
            }
            if (sugarLevel > 0)
            {
                totalCost += sugarLevel * 5;
            }
            bool costOk = totalCost > 0 && totalCost < 500;

            // 5) Постусловия — все должны быть true
            bool postOk = postConditions == null || postConditions.All(p => p.Value);

            // Консолидированный вердикт
            return preOk && resourcesOk && timeOk && costOk && postOk;
        }
    }
}