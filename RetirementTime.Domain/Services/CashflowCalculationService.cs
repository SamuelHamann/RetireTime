using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Asset;using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Helpers;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Domain.Services;

public class CashflowCalculationService : ICashflowCalculationService
{
    /// <summary>
    /// Iterates from the user's current age to their life expectancy, resolving the
    /// applicable income and expense timelines for each year and calculating all annual
    /// totals. Returns one <see cref="CashflowTimelineTotals"/> entry per age year
    /// containing itemised income, expense, and property maintenance figures as well as
    /// derived net cashflow. Inflation compounding, debt balance roll-forward, and
    /// property appreciation are all applied per year.
    /// </summary>
    public List<CashflowTimelineTotals> CalculateTimelineTotals(
        List<CashflowTimelineData> incomeTimelines,
        List<CashflowTimelineData> expenseTimelines,
        List<Frequency> frequencies,
        DashboardAssumptions assumptions,
        List<GenericDebt> debts,
        AssetsHome? home,
        List<AssetsInvestmentProperty> investmentProperties,
        OasConstants? oasConstants,
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
            var investmentExpensesTotal    = CalculateInvestmentExpenses(expenseTimeline?.InvestmentExpenses, inflationRate, yearIndex, frequencies);
            var currentPropertyValue       = CalculatePropertyValues(home, investmentProperties, assumptions.YearlyPropertyAppreciation / 100m, inflationRate, yearIndex);
            var propertyMaintenanceTotal   = CalculatePropertyMaintenanceExpenses(currentPropertyValue, assumptions.YearlyHouseMaintenance);

            // Calculate income with assumptions applied
            var salaryRaiseRate           = assumptions.AnnualSalaryRaise / 100m;
            var employmentIncomeTotal     = CalculateEmploymentIncome(incomeTimeline?.EmploymentIncomes, salaryRaiseRate, inflationRate, yearIndex, frequencies);
            var selfEmploymentIncomeTotal = CalculateSelfEmploymentIncome(incomeTimeline?.SelfEmploymentIncomes, salaryRaiseRate, inflationRate, yearIndex, frequencies);
            var propertyIncomeTotal       = CalculatePropertyIncome(incomeTimeline?.PropertyIncomes, inflationRate, yearIndex, frequencies);
            var otherIncomeTotal          = CalculateOtherIncome(incomeTimeline?.OtherIncomes, inflationRate, yearIndex, frequencies);
            
            
            
        }

        return results;
    }

    /// <summary>
    /// Calculates total annualised net employment income for the given year across all
    /// employment records in the current timeline period. Net figures are used throughout
    /// (NetSalary, NetCommissions, NetBonus, and OtherIncome.Net) so the result represents
    /// take-home revenue after tax and deductions.
    /// <para>
    /// The real rate of return applied to each component is derived from the
    /// <paramref name="annualSalaryRaiseRate"/> adjusted for inflation via the Fisher
    /// equation — meaning the compounding reflects purchasing-power growth above inflation,
    /// not just a nominal raise. All components (salary, commissions, bonus, and other
    /// incomes) are grown at this same rate because they are assumed to be negotiated
    /// together as part of total compensation.
    /// </para>
    /// Returns zero if no employment records exist for the current timeline period.
    /// </summary>
    private static decimal CalculateEmploymentIncome(
        List<EmploymentIncome>? employmentIncomes,
        decimal annualSalaryRaiseRate,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (employmentIncomes is null or { Count: 0 }) return 0m;

        // Real rate of return: salary raise adjusted for inflation (Fisher equation)
        // e.g. 4% raise, 2% inflation → ~1.96% real growth per year
        var realRaiseRate = FinancialMathHelper.RealRateOfReturn(annualSalaryRaiseRate, inflationRate);

        var total = 0m;

        foreach (var emp in employmentIncomes)
        {
            var annualSalary      = ToAnnual(emp.NetSalary,      emp.NetSalaryFrequencyId,      frequencies);
            var annualCommissions = ToAnnual(emp.NetCommissions,  emp.NetCommissionsFrequencyId,  frequencies);
            var annualBonus       = ToAnnual(emp.NetBonus,        emp.NetBonusFrequencyId,        frequencies);
            var annualOther       = emp.OtherIncomes.Sum(o => ToAnnual(o.Net, o.NetFrequencyId, frequencies));

            var annualNet = annualSalary + annualCommissions + annualBonus + annualOther;

            // Compound the total net compensation at the real salary-raise rate
            total += FinancialMathHelper.CompoundInterest(annualNet, realRaiseRate, 1, yearIndex);
        }

        return total;
    }

    /// <summary>
    /// Calculates total annualised net self-employment income for the given year across all
    /// self-employment records in the current timeline period. Net salary and net dividends
    /// are used so the result represents take-home revenue after tax and deductions.
    /// <para>
    /// Both components are grown at the real rate of return derived from the
    /// <paramref name="annualSalaryRaiseRate"/> adjusted for inflation via the Fisher
    /// equation, matching the employment income treatment and reflecting that a business
    /// owner's compensation is expected to grow alongside the salary market.
    /// </para>
    /// Returns zero if no self-employment records exist for the current timeline period.
    /// </summary>
    private static decimal CalculateSelfEmploymentIncome(
        List<SelfEmploymentIncome>? selfEmploymentIncomes,
        decimal annualSalaryRaiseRate,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (selfEmploymentIncomes is null or { Count: 0 }) return 0m;

        var realRaiseRate = FinancialMathHelper.RealRateOfReturn(annualSalaryRaiseRate, inflationRate);

        var total = 0m;

        foreach (var se in selfEmploymentIncomes)
        {
            var annualSalary    = ToAnnual(se.NetSalary,    se.NetSalaryFrequencyId,    frequencies);
            var annualDividends = ToAnnual(se.NetDividends, se.NetDividendsFrequencyId, frequencies);

            var annualNet = annualSalary + annualDividends;

            total += FinancialMathHelper.CompoundInterest(annualNet, realRaiseRate, 1, yearIndex);
        }

        return total;
    }

    /// <summary>
    /// Calculates total annualised rental income from investment properties for the given
    /// year. Each property income entry is converted to an annual amount using its frequency
    /// and then inflation-compounded, reflecting that rents tend to rise in line with the
    /// cost of living over time.
    /// Returns zero if no property income records exist for the current timeline period.
    /// </summary>
    private static decimal CalculatePropertyIncome(
        List<PropertyIncome>? propertyIncomes,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (propertyIncomes is null or { Count: 0 }) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        return propertyIncomes.Sum(p =>
            FinancialMathHelper.CompoundInterest(ToAnnual(p.Amount, p.FrequencyId, frequencies), rate, 1, yearIndex));
    }

    /// <summary>
    /// Calculates total annualised other income (government benefits, pensions, royalties,
    /// etc.) for the given year. Each entry is converted to an annual amount using its
    /// frequency and then inflation-compounded to preserve purchasing power over time.
    /// Returns zero if no other income records exist for the current timeline period.
    /// </summary>
    private static decimal CalculateOtherIncome(
        List<OtherIncomeOrBenefits>? otherIncomes,
        decimal inflationRate,
        int yearIndex,
        List<Frequency> frequencies)
    {
        if (otherIncomes is null or { Count: 0 }) return 0m;

        var rate = FinancialMathHelper.RealRateOfReturn(inflationRate, inflationRate);

        return otherIncomes.Sum(o =>
            FinancialMathHelper.CompoundInterest(ToAnnual(o.Amount, o.FrequencyId ?? (int)FrequencyEnum.Annually, frequencies), rate, 1, yearIndex));
    }

    /// <summary>
    /// Calculates the total debt repayment amount for the current year across all debts.
    /// For each repayment entry the payment is capped at the remaining balance to avoid
    /// overpayment. After payments are applied, each debt with a positive remaining balance
    /// is grown by its annual interest rate, rolling the balance forward into the next year.
    /// </summary>
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

    /// <summary>
    /// Calculates total annual living expenses for the given year. Each expense category
    /// (food, utilities, insurance, gas, home maintenance, property tax, cellphone, health,
    /// and other) is first converted to an annual amount via its frequency and then
    /// inflation-compounded using the real rate of return. Rent is also inflation-compounded,
    /// while mortgage payments are kept fixed as they represent a contracted obligation.
    /// Returns zero if no living expense record exists for the current timeline period.
    /// </summary>
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

    /// <summary>
    /// Calculates total annual discretionary expenses for the given year. When the record
    /// uses a grouped entry, only the single grouped amount is inflation-compounded and
    /// returned. Otherwise, each category (gym, subscriptions, eating out, entertainment,
    /// travel, and other) is inflation-compounded individually. Charitable donations are
    /// kept fixed as a deliberate choice. Returns zero if no discretionary record exists.
    /// </summary>
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

    /// <summary>
    /// Calculates the total annual amount for all user-defined other expenses in the
    /// current timeline period. Each entry is converted to an annual figure using its
    /// frequency and then inflation-compounded for the given year. Returns zero if the
    /// list is null or empty.
    /// </summary>
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

    /// <summary>
    /// Calculates the total annual cost of asset-related expenses (e.g. vehicle running
    /// costs, storage, upkeep) for the current timeline period. Each entry is converted
    /// to an annual figure using its frequency and inflation-compounded for the given year.
    /// Returns zero if the list is null or empty.
    /// </summary>
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

    /// <summary>
    /// Calculates the total annual cost of investment-related expenses (e.g. management
    /// fees, advisory costs) for the current timeline period. Each entry is converted to
    /// an annual figure using its frequency and inflation-compounded for the given year.
    /// Returns zero if the list is null or empty.
    /// </summary>
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

    /// <summary>
    /// Returns the inflation-adjusted combined market value of the home and all investment
    /// properties for the given year. Uses the real rate of return (property appreciation
    /// adjusted for inflation via the Fisher equation) with annual compounding.
    /// </summary>
    private static decimal CalculatePropertyValues(
        AssetsHome? home,
        List<AssetsInvestmentProperty> investmentProperties,
        decimal nominalAppreciationRate,
        decimal inflationRate,
        int yearIndex)
    {
        var realRate = FinancialMathHelper.RealRateOfReturn(nominalAppreciationRate, inflationRate);

        var homeValue = FinancialMathHelper.CompoundInterest(
            home?.HomeValue ?? 0m, realRate, 1, yearIndex);

        var investmentValue = investmentProperties.Sum(p =>
            FinancialMathHelper.CompoundInterest(p.PropertyValue ?? 0m, realRate, 1, yearIndex));

        return homeValue + investmentValue;
    }

    /// <summary>
    /// Returns the annual cost of maintaining all properties based on the
    /// YearlyHouseMaintenance assumption percentage applied to the current
    /// inflation-adjusted combined property value.
    /// </summary>
    private static decimal CalculatePropertyMaintenanceExpenses(
        decimal currentPropertyValue,
        decimal yearlyHouseMaintenancePercent)
    {
        if (currentPropertyValue <= 0m || yearlyHouseMaintenancePercent <= 0m) return 0m;

        return currentPropertyValue * (yearlyHouseMaintenancePercent / 100m);
    }

    /// <summary>
    /// Converts a periodic monetary amount to its annual equivalent by multiplying by
    /// the number of periods per year for the given frequency. Returns zero if the amount
    /// is null or non-positive, or if the frequency cannot be found (defaulting to 1×/year).
    /// </summary>
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

