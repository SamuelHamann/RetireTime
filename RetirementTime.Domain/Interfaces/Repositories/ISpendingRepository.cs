using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISpendingRepository
{
    Task<List<Frequency>> GetFrequenciesAsync();

    // Living Expenses (single record per scenario)
    Task<SpendingLivingExpenses?> GetLivingExpensesAsync(long scenarioId);
    Task<SpendingLivingExpenses> UpsertLivingExpensesAsync(SpendingLivingExpenses expenses);

    // Discretionary Expenses (single record per scenario)
    Task<SpendingDiscretionaryExpenses?> GetDiscretionaryExpensesAsync(long scenarioId);
    Task<SpendingDiscretionaryExpenses> UpsertDiscretionaryExpensesAsync(SpendingDiscretionaryExpenses expenses);

    // Debt Repayments (list)
    Task<List<SpendingDebtRepayment>> GetDebtRepaymentsAsync(long scenarioId);
    Task<SpendingDebtRepayment> CreateDebtRepaymentAsync(SpendingDebtRepayment item);
    Task<bool> UpdateDebtRepaymentAsync(SpendingDebtRepayment item);
    Task<bool> DeleteDebtRepaymentAsync(long id);

    // Assets Expenses (list)
    Task<List<SpendingAssetsExpense>> GetAssetsExpensesAsync(long scenarioId);
    Task<SpendingAssetsExpense> CreateAssetsExpenseAsync(SpendingAssetsExpense item);
    Task<bool> UpdateAssetsExpenseAsync(SpendingAssetsExpense item);
    Task<bool> DeleteAssetsExpenseAsync(long id);

    // Other Expenses (list)
    Task<List<SpendingOtherExpense>> GetOtherExpensesAsync(long scenarioId);
    Task<SpendingOtherExpense> CreateOtherExpenseAsync(SpendingOtherExpense item);
    Task<bool> UpdateOtherExpenseAsync(SpendingOtherExpense item);
    Task<bool> DeleteOtherExpenseAsync(long id);
}
