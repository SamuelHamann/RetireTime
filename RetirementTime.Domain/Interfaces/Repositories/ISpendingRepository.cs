using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISpendingRepository
{
    Task<List<Frequency>> GetFrequenciesAsync();

    // Living Expenses (single record per scenario+timeline)
    Task<SpendingLivingExpenses?> GetLivingExpensesAsync(long scenarioId, long timelineId);
    Task<SpendingLivingExpenses> UpsertLivingExpensesAsync(SpendingLivingExpenses expenses);

    // Discretionary Expenses (single record per scenario+timeline)
    Task<SpendingDiscretionaryExpenses?> GetDiscretionaryExpensesAsync(long scenarioId, long timelineId);
    Task<SpendingDiscretionaryExpenses> UpsertDiscretionaryExpensesAsync(SpendingDiscretionaryExpenses expenses);

    // Debt Repayments (list)
    Task<List<SpendingDebtRepayment>> GetDebtRepaymentsAsync(long scenarioId, long timelineId);
    Task<SpendingDebtRepayment> CreateDebtRepaymentAsync(SpendingDebtRepayment item);
    Task<bool> UpdateDebtRepaymentAsync(SpendingDebtRepayment item);
    Task<bool> DeleteDebtRepaymentAsync(long id);

    // Assets Expenses (list)
    Task<List<SpendingAssetsExpense>> GetAssetsExpensesAsync(long scenarioId, long timelineId);
    Task<SpendingAssetsExpense> CreateAssetsExpenseAsync(SpendingAssetsExpense item);
    Task<bool> UpdateAssetsExpenseAsync(SpendingAssetsExpense item);
    Task<bool> DeleteAssetsExpenseAsync(long id);

    // Other Expenses (list)
    Task<List<SpendingOtherExpense>> GetOtherExpensesAsync(long scenarioId, long timelineId);
    Task<SpendingOtherExpense> CreateOtherExpenseAsync(SpendingOtherExpense item);
    Task<bool> UpdateOtherExpenseAsync(SpendingOtherExpense item);
    Task<bool> DeleteOtherExpenseAsync(long id);

    // Investment Expenses (list)
    Task<List<SpendingInvestmentExpense>> GetInvestmentExpensesAsync(long scenarioId, long timelineId);
    Task<SpendingInvestmentExpense> CreateInvestmentExpenseAsync(SpendingInvestmentExpense item);
    Task<bool> UpdateInvestmentExpenseAsync(SpendingInvestmentExpense item);
    Task<bool> DeleteInvestmentExpenseAsync(long id);

    // Timeline linking
    Task CreateEmptyExpensesForTimelineAsync(long scenarioId, long timelineId);
    Task CloneExpensesFromTimelineAsync(long scenarioId, long sourceTimelineId, long targetTimelineId);
}
