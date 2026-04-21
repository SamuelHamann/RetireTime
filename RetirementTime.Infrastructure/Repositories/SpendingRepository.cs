using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class SpendingRepository(ApplicationDbContext context) : ISpendingRepository
{
    public async Task<List<Frequency>> GetFrequenciesAsync() =>
        await context.Frequencies.OrderBy(f => f.Id).ToListAsync();

    // ── Living Expenses ──────────────────────────────────────────────────────

    public async Task<SpendingLivingExpenses?> GetLivingExpensesAsync(long scenarioId) =>
        await context.SpendingLivingExpenses.FirstOrDefaultAsync(e => e.ScenarioId == scenarioId);

    public async Task<SpendingLivingExpenses> UpsertLivingExpensesAsync(SpendingLivingExpenses expenses)
    {
        var existing = await context.SpendingLivingExpenses
            .FirstOrDefaultAsync(e => e.ScenarioId == expenses.ScenarioId);

        if (existing == null)
        {
            expenses.CreatedAt = DateTime.UtcNow;
            expenses.UpdatedAt = DateTime.UtcNow;
            context.SpendingLivingExpenses.Add(expenses);
            await context.SaveChangesAsync();
            return expenses;
        }

        existing.RentOrMortgage              = expenses.RentOrMortgage;
        existing.RentOrMortgageFrequencyId   = expenses.RentOrMortgageFrequencyId;
        existing.Food                        = expenses.Food;
        existing.FoodFrequencyId             = expenses.FoodFrequencyId;
        existing.Utilities                   = expenses.Utilities;
        existing.UtilitiesFrequencyId        = expenses.UtilitiesFrequencyId;
        existing.Insurance                   = expenses.Insurance;
        existing.InsuranceFrequencyId        = expenses.InsuranceFrequencyId;
        existing.Gas                         = expenses.Gas;
        existing.GasFrequencyId              = expenses.GasFrequencyId;
        existing.HomeMaintenance             = expenses.HomeMaintenance;
        existing.HomeMaintenanceFrequencyId  = expenses.HomeMaintenanceFrequencyId;
        existing.Cellphone                   = expenses.Cellphone;
        existing.CellphoneFrequencyId        = expenses.CellphoneFrequencyId;
        existing.HealthSpendings             = expenses.HealthSpendings;
        existing.HealthSpendingsFrequencyId  = expenses.HealthSpendingsFrequencyId;
        existing.OtherLivingExpenses         = expenses.OtherLivingExpenses;
        existing.OtherLivingExpensesFrequencyId = expenses.OtherLivingExpensesFrequencyId;
        existing.UpdatedAt                   = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existing;
    }

    // ── Discretionary Expenses ───────────────────────────────────────────────

    public async Task<SpendingDiscretionaryExpenses?> GetDiscretionaryExpensesAsync(long scenarioId) =>
        await context.SpendingDiscretionaryExpenses.FirstOrDefaultAsync(e => e.ScenarioId == scenarioId);

    public async Task<SpendingDiscretionaryExpenses> UpsertDiscretionaryExpensesAsync(SpendingDiscretionaryExpenses expenses)
    {
        var existing = await context.SpendingDiscretionaryExpenses
            .FirstOrDefaultAsync(e => e.ScenarioId == expenses.ScenarioId);

        if (existing == null)
        {
            expenses.CreatedAt = DateTime.UtcNow;
            expenses.UpdatedAt = DateTime.UtcNow;
            context.SpendingDiscretionaryExpenses.Add(expenses);
            await context.SaveChangesAsync();
            return expenses;
        }

        existing.GymMembership                         = expenses.GymMembership;
        existing.GymMembershipFrequencyId              = expenses.GymMembershipFrequencyId;
        existing.Subscriptions                         = expenses.Subscriptions;
        existing.SubscriptionsFrequencyId              = expenses.SubscriptionsFrequencyId;
        existing.EatingOut                             = expenses.EatingOut;
        existing.EatingOutFrequencyId                  = expenses.EatingOutFrequencyId;
        existing.Entertainment                         = expenses.Entertainment;
        existing.EntertainmentFrequencyId              = expenses.EntertainmentFrequencyId;
        existing.Travel                                = expenses.Travel;
        existing.TravelFrequencyId                     = expenses.TravelFrequencyId;
        existing.CharitableDonations                   = expenses.CharitableDonations;
        existing.CharitableDonationsFrequencyId        = expenses.CharitableDonationsFrequencyId;
        existing.OtherDiscretionaryExpenses            = expenses.OtherDiscretionaryExpenses;
        existing.OtherDiscretionaryExpensesFrequencyId = expenses.OtherDiscretionaryExpensesFrequencyId;
        existing.UseGroupedEntry                       = expenses.UseGroupedEntry;
        existing.GroupedAmount                         = expenses.GroupedAmount;
        existing.GroupedFrequencyId                    = expenses.GroupedFrequencyId;
        existing.UpdatedAt                             = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existing;
    }

    // ── Debt Repayments ──────────────────────────────────────────────────────

    public async Task<List<SpendingDebtRepayment>> GetDebtRepaymentsAsync(long scenarioId) =>
        await context.SpendingDebtRepayments
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

    public async Task<SpendingDebtRepayment> CreateDebtRepaymentAsync(SpendingDebtRepayment item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        context.SpendingDebtRepayments.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateDebtRepaymentAsync(SpendingDebtRepayment item)
    {
        var existing = await context.SpendingDebtRepayments.FindAsync(item.Id);
        if (existing == null) return false;
        existing.Name         = item.Name;
        existing.Amount       = item.Amount;
        existing.FrequencyId  = item.FrequencyId;
        existing.GenericDebtId = item.GenericDebtId;
        existing.UpdatedAt    = DateTime.UtcNow;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteDebtRepaymentAsync(long id)
    {
        var item = await context.SpendingDebtRepayments.FindAsync(id);
        if (item == null) return false;
        context.SpendingDebtRepayments.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }

    // ── Assets Expenses ──────────────────────────────────────────────────────

    public async Task<List<SpendingAssetsExpense>> GetAssetsExpensesAsync(long scenarioId) =>
        await context.SpendingAssetsExpenses
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

    public async Task<SpendingAssetsExpense> CreateAssetsExpenseAsync(SpendingAssetsExpense item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        context.SpendingAssetsExpenses.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAssetsExpenseAsync(SpendingAssetsExpense item)
    {
        var existing = await context.SpendingAssetsExpenses.FindAsync(item.Id);
        if (existing == null) return false;
        existing.Name                       = item.Name;
        existing.Expense                    = item.Expense;
        existing.FrequencyId                = item.FrequencyId;
        existing.AssetsHomeId               = item.AssetsHomeId;
        existing.AssetsInvestmentPropertyId = item.AssetsInvestmentPropertyId;
        existing.AssetsInvestmentAccountId  = item.AssetsInvestmentAccountId;
        existing.AssetsPhysicalAssetId      = item.AssetsPhysicalAssetId;
        existing.UpdatedAt                  = DateTime.UtcNow;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAssetsExpenseAsync(long id)
    {
        var item = await context.SpendingAssetsExpenses.FindAsync(id);
        if (item == null) return false;
        context.SpendingAssetsExpenses.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }

    // ── Other Expenses ───────────────────────────────────────────────────────

    public async Task<List<SpendingOtherExpense>> GetOtherExpensesAsync(long scenarioId) =>
        await context.SpendingOtherExpenses
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

    public async Task<SpendingOtherExpense> CreateOtherExpenseAsync(SpendingOtherExpense item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        context.SpendingOtherExpenses.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateOtherExpenseAsync(SpendingOtherExpense item)
    {
        var existing = await context.SpendingOtherExpenses.FindAsync(item.Id);
        if (existing == null) return false;
        existing.Name        = item.Name;
        existing.Amount      = item.Amount;
        existing.FrequencyId = item.FrequencyId;
        existing.UpdatedAt   = DateTime.UtcNow;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOtherExpenseAsync(long id)
    {
        var item = await context.SpendingOtherExpenses.FindAsync(id);
        if (item == null) return false;
        context.SpendingOtherExpenses.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }
}
