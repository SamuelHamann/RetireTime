using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Interfaces.Services;

/// <summary>
/// Input data grouped by timeline, passed to the cashflow calculation service.
/// </summary>
public class CashflowTimelineData
{
    public RetirementTimeline Timeline { get; set; } = null!;

    // Income
    public List<EmploymentIncome> EmploymentIncomes { get; set; } = [];
    public List<SelfEmploymentIncome> SelfEmploymentIncomes { get; set; } = [];
    public List<PensionDefinedBenefits> PensionDefinedBenefits { get; set; } = [];
    public List<OtherIncomeOrBenefits> OtherIncomes { get; set; } = [];

    // Expenses
    public SpendingLivingExpenses? LivingExpenses { get; set; }
    public SpendingDiscretionaryExpenses? DiscretionaryExpenses { get; set; }
    public List<SpendingDebtRepayment> DebtRepayments { get; set; } = [];
    public List<SpendingAssetsExpense> AssetsExpenses { get; set; } = [];
    public List<SpendingOtherExpense> OtherExpenses { get; set; } = [];
    public List<SpendingInvestmentExpense> InvestmentExpenses { get; set; } = [];
}

/// <summary>
/// Totals for a single timeline period (annual figures).
/// </summary>
public class CashflowTimelineTotals
{
    public RetirementTimeline Timeline { get; set; } = null!;

    // Income totals (annual)
    public decimal EmploymentIncome { get; set; }
    public decimal SelfEmploymentIncome { get; set; }
    public decimal DefinedBenefitsPension { get; set; }
    public decimal OtherIncome { get; set; }
    public decimal TotalIncome => EmploymentIncome + SelfEmploymentIncome + DefinedBenefitsPension + OtherIncome;

    // Expense totals (annual)
    public decimal LivingExpenses { get; set; }
    public decimal DiscretionaryExpenses { get; set; }
    public decimal DebtRepayments { get; set; }
    public decimal AssetsExpenses { get; set; }
    public decimal OtherExpenses { get; set; }
    public decimal TotalExpenses => LivingExpenses + DiscretionaryExpenses + DebtRepayments + AssetsExpenses + OtherExpenses;

    public decimal NetCashflow => TotalIncome - TotalExpenses;
}

public interface ICashflowCalculationService
{
    /// <summary>
    /// Calculates annual income and expense totals for each timeline period.
    /// </summary>
    List<CashflowTimelineTotals> CalculateTimelineTotals(
        List<CashflowTimelineData> incomeTimelines,
        List<CashflowTimelineData> expenseTimelines,
        List<Frequency> frequencies,
        DashboardAssumptions assumptions,
        List<GenericDebt> debts,
        int currentAge,
        int retirementAge,
        int lifeExpectancy);
}

