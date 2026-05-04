using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Domain.Services;

public class CashflowCalculationService : ICashflowCalculationService
{
    public List<CashflowTimelineTotals> CalculateTimelineTotals(
        List<CashflowTimelineData> incomeTimelines,
        List<CashflowTimelineData> expenseTimelines,
        List<Frequency> frequencies,
        DashboardAssumptions assumptions,
        int currentAge,
        int retirementAge,
        int lifeExpectancy)
    {
        var results = new List<CashflowTimelineTotals>();

        for (var age = currentAge; age <= lifeExpectancy; age++)
        {
            var isRetired = age >= retirementAge;

            var incomeTimeline = incomeTimelines
                .FirstOrDefault(t => age >= t.Timeline.AgeFrom && age <= t.Timeline.AgeTo);

            var expenseTimeline = expenseTimelines
                .FirstOrDefault(t => age >= t.Timeline.AgeFrom && age <= t.Timeline.AgeTo);

            // Calculations for this year will be implemented in subsequent steps.
        }

        return results;
    }

    private static decimal ToAnnual(decimal? amount, int frequencyId, List<Frequency> frequencies)
    {
        if (amount is null or <= 0) return 0m;
        var freq = frequencies.FirstOrDefault(f => f.Id == frequencyId);
        return amount.Value * (freq?.FrequencyPerYear ?? 1);
    }
}
