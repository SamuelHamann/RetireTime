using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Helpers;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Domain.Services;

public class CashflowCalculationService : ICashflowCalculationService
{
    public List<CashflowTimelineTotals> CalculateTimelineTotals(
        List<CashflowTimelineData> incomeTimelines,
        List<CashflowTimelineData> expenseTimelines,
        List<Frequency> frequencies,
        DashboardAssumptions assumptions,
        List<GenericDebt> debts,
        int currentAge,
        int retirementAge,
        int lifeExpectancy)
    {
        var results = new List<CashflowTimelineTotals>();

        var inflationRate = assumptions.YearlyInflationRate / 100m;

        // Build debt repayment list once — amounts are fixed, not per-year
        var debtRepayments = expenseTimelines
            .SelectMany(t => t.DebtRepayments)
            .Where(d => d.GenericDebtId.HasValue)
            .GroupBy(d => d.GenericDebtId!.Value)
            .Select(g => new DebtRepaymentEntry
            {
                GenericDebtId    = g.Key,
                AnnualAmountPaid = g.Sum(d => ToAnnual(d.Amount, d.FrequencyId, frequencies))
            })
            .ToList();

        // Track remaining balance per debt — grows by interest each year
        var remainingBalances = debts.ToDictionary(d => d.Id, d => d.Balance ?? 0m);

        for (var age = currentAge; age <= lifeExpectancy; age++)
        {
            var isRetired = age >= retirementAge;
            var yearIndex = age - currentAge;

            var incomeTimeline = incomeTimelines
                .FirstOrDefault(t => age >= t.Timeline.AgeFrom && age <= t.Timeline.AgeTo);

            var expenseTimeline = expenseTimelines
                .FirstOrDefault(t => age >= t.Timeline.AgeFrom && age <= t.Timeline.AgeTo);

            // Calculate expenses with assumptions applied
            var livingExpensesTotal        = CalculateLivingExpenses(expenseTimeline?.LivingExpenses, inflationRate, yearIndex, frequencies);
            var discretionaryExpensesTotal = CalculateDiscretionaryExpenses(expenseTimeline?.DiscretionaryExpenses, inflationRate, yearIndex, frequencies);
            var debtRepaymentsTotal        = CalculateDebtRepayments(debtRepayments, debts, remainingBalances);
            var otherExpensesTotal         = CalculateOtherExpenses(expenseTimeline?.OtherExpenses, inflationRate, yearIndex, frequencies);
            var assetsExpensesTotal        = CalculateAssetsExpenses(expenseTimeline?.AssetsExpenses, inflationRate, yearIndex, frequencies);
            
            var firstExpenseTimeline = expenseTimelines.FirstOrDefault();
            var includeInvestmentExpenses = isRetired && expenseTimeline is not null && expenseTimeline.Timeline.Id != firstExpenseTimeline?.Timeline.Id;
            var investmentExpensesTotal    = CalculateInvestmentExpenses(
                includeInvestmentExpenses ? expenseTimeline?.InvestmentExpenses : null,
                inflationRate, yearIndex, frequencies);
            
        }

        return results;
    }

    private static decimal CalculateDebtRepayments(
        List<DebtRepaymentEntry> repayments,
        List<GenericDebt> debts,
        Dictionary<long, decimal> remainingBalances)
    {
        var totalThisYear = 0m;

        foreach (var repayment in repayments)
        {
            var debt = debts.FirstOrDefault(d => d.Id == repayment.GenericDebtId);
            if (debt is null) continue;

            var remaining = remainingBalances.GetValueOrDefault(debt.Id);
            if (remaining <= 0) continue;

            // Only count what's still owed — no excess beyond the remaining balance
            var paidThisYear = Math.Min(repayment.AnnualAmountPaid, remaining);
            remainingBalances[debt.Id] -= paidThisYear;
            totalThisYear             += paidThisYear;
        }

        // Grow each debt that still has a balance by its annual interest rate
        foreach (var debt in debts)
        {
            var remaining    = remainingBalances.GetValueOrDefault(debt.Id);
            if (remaining <= 0) continue;

            var interestRate = (debt.InterestRate ?? 0m) / 100m;
            remainingBalances[debt.Id] *= 1 + interestRate;
        }

        return totalThisYear;
    }

    private static decimal CalculateLivingExpenses(
        SpendingLivingExpenses? living,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (living is null) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        // Rent increases with inflation; mortgage payment is fixed
        var rent     = FinancialMathHelper.CompoundInterest(ToAnnual(living.Rent,     living.RentFrequencyId     ?? (int)FrequencyEnum.Monthly,  frequencies), rate, 1, yearIndex);
        var mortgage = ToAnnual(living.Mortgage, living.MortgageFrequencyId ?? (int)FrequencyEnum.Monthly, frequencies);

        return rent + mortgage +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.Food,                living.FoodFrequencyId,                frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.Utilities,           living.UtilitiesFrequencyId,           frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.Insurance,           living.InsuranceFrequencyId,           frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.Gas,                 living.GasFrequencyId,                 frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.HomeMaintenance,     living.HomeMaintenanceFrequencyId,     frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.PropertyTax,         living.PropertyTaxFrequencyId,         frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.Cellphone,           living.CellphoneFrequencyId,           frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.HealthSpendings,     living.HealthSpendingsFrequencyId,     frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(living.OtherLivingExpenses, living.OtherLivingExpensesFrequencyId, frequencies), rate, 1, yearIndex);
    }

    private static decimal CalculateDiscretionaryExpenses(
        SpendingDiscretionaryExpenses? disc,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (disc is null) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        if (disc.UseGroupedEntry)
            return FinancialMathHelper.CompoundInterest(ToAnnual(disc.GroupedAmount, disc.GroupedFrequencyId, frequencies), rate, 1, yearIndex);

        // Charitable donations stay fixed; everything else grows with inflation
        return
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.GymMembership,              disc.GymMembershipFrequencyId,              frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.Subscriptions,              disc.SubscriptionsFrequencyId,              frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.EatingOut,                  disc.EatingOutFrequencyId,                  frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.Entertainment,              disc.EntertainmentFrequencyId,              frequencies), rate, 1, yearIndex) +
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.Travel,                     disc.TravelFrequencyId,                     frequencies), rate, 1, yearIndex) +
            ToAnnual(disc.CharitableDonations, disc.CharitableDonationsFrequencyId, frequencies) +
            FinancialMathHelper.CompoundInterest(ToAnnual(disc.OtherDiscretionaryExpenses, disc.OtherDiscretionaryExpensesFrequencyId, frequencies), rate, 1, yearIndex);
    }

    private static decimal CalculateOtherExpenses(
        List<SpendingOtherExpense>? otherExpenses,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (otherExpenses is null or { Count: 0 }) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        return otherExpenses.Sum(e =>
            FinancialMathHelper.CompoundInterest(ToAnnual(e.Amount, e.FrequencyId, frequencies), rate, 1, yearIndex));
    }

    private static decimal CalculateAssetsExpenses(
        List<SpendingAssetsExpense>? assetsExpenses,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (assetsExpenses is null or { Count: 0 }) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        return assetsExpenses.Sum(e =>
            FinancialMathHelper.CompoundInterest(ToAnnual(e.Expense, e.FrequencyId, frequencies), rate, 1, yearIndex));
    }

    private static decimal CalculateInvestmentExpenses(
        List<SpendingInvestmentExpense>? investmentExpenses,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (investmentExpenses is null or { Count: 0 }) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        return investmentExpenses.Sum(e =>
            FinancialMathHelper.CompoundInterest(ToAnnual(e.Amount, e.FrequencyId, frequencies), rate, 1, yearIndex));
    }

    private static decimal ToAnnual(decimal? amount, int frequencyId, List<Frequency> frequencies)
    {
        if (amount is null or <= 0) return 0m;
        var freq = frequencies.FirstOrDefault(f => f.Id == frequencyId);
        return amount.Value * (freq?.FrequencyPerYear ?? 1);
    }
    
}

public record DebtRepaymentEntry
{
    public long GenericDebtId { get; init; }
    public decimal AnnualAmountPaid { get; init; }
}

