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

    public async Task<SpendingLivingExpenses?> GetLivingExpensesAsync(long scenarioId, long timelineId) =>
        await context.SpendingLivingExpenses.FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId);

    public async Task<SpendingLivingExpenses> UpsertLivingExpensesAsync(SpendingLivingExpenses expenses)
    {
        var existing = await context.SpendingLivingExpenses
            .FirstOrDefaultAsync(e => e.ScenarioId == expenses.ScenarioId && e.RetirementTimelineId == expenses.RetirementTimelineId);

        if (existing == null)
        {
            expenses.CreatedAt = DateTime.UtcNow;
            expenses.UpdatedAt = DateTime.UtcNow;
            context.SpendingLivingExpenses.Add(expenses);
            await context.SaveChangesAsync();
            return expenses;
        }

        existing.RentOrMortgage = expenses.RentOrMortgage;
        existing.RentOrMortgageFrequencyId = expenses.RentOrMortgageFrequencyId;
        existing.Food = expenses.Food;
        existing.FoodFrequencyId = expenses.FoodFrequencyId;
        existing.Utilities = expenses.Utilities;
        existing.UtilitiesFrequencyId = expenses.UtilitiesFrequencyId;
        existing.Insurance = expenses.Insurance;
        existing.InsuranceFrequencyId = expenses.InsuranceFrequencyId;
        existing.Gas = expenses.Gas;
        existing.GasFrequencyId = expenses.GasFrequencyId;
        existing.HomeMaintenance = expenses.HomeMaintenance;
        existing.HomeMaintenanceFrequencyId = expenses.HomeMaintenanceFrequencyId;
        existing.PropertyTax = expenses.PropertyTax;
        existing.PropertyTaxFrequencyId = expenses.PropertyTaxFrequencyId;
        existing.Cellphone = expenses.Cellphone;
        existing.CellphoneFrequencyId = expenses.CellphoneFrequencyId;
        existing.HealthSpendings = expenses.HealthSpendings;
        existing.HealthSpendingsFrequencyId = expenses.HealthSpendingsFrequencyId;
        existing.OtherLivingExpenses = expenses.OtherLivingExpenses;
        existing.OtherLivingExpensesFrequencyId = expenses.OtherLivingExpensesFrequencyId;
        existing.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existing;
    }

    // ── Discretionary Expenses ───────────────────────────────────────────────

    public async Task<SpendingDiscretionaryExpenses?> GetDiscretionaryExpensesAsync(long scenarioId, long timelineId) =>
        await context.SpendingDiscretionaryExpenses.FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId);

    public async Task<SpendingDiscretionaryExpenses> UpsertDiscretionaryExpensesAsync(SpendingDiscretionaryExpenses expenses)
    {
        var existing = await context.SpendingDiscretionaryExpenses
            .FirstOrDefaultAsync(e => e.ScenarioId == expenses.ScenarioId && e.RetirementTimelineId == expenses.RetirementTimelineId);

        if (existing == null)
        {
            expenses.CreatedAt = DateTime.UtcNow;
            expenses.UpdatedAt = DateTime.UtcNow;
            context.SpendingDiscretionaryExpenses.Add(expenses);
            await context.SaveChangesAsync();
            return expenses;
        }

        existing.GymMembership = expenses.GymMembership;
        existing.GymMembershipFrequencyId = expenses.GymMembershipFrequencyId;
        existing.Subscriptions = expenses.Subscriptions;
        existing.SubscriptionsFrequencyId = expenses.SubscriptionsFrequencyId;
        existing.EatingOut = expenses.EatingOut;
        existing.EatingOutFrequencyId = expenses.EatingOutFrequencyId;
        existing.Entertainment = expenses.Entertainment;
        existing.EntertainmentFrequencyId = expenses.EntertainmentFrequencyId;
        existing.Travel = expenses.Travel;
        existing.TravelFrequencyId = expenses.TravelFrequencyId;
        existing.CharitableDonations = expenses.CharitableDonations;
        existing.CharitableDonationsFrequencyId = expenses.CharitableDonationsFrequencyId;
        existing.OtherDiscretionaryExpenses = expenses.OtherDiscretionaryExpenses;
        existing.OtherDiscretionaryExpensesFrequencyId = expenses.OtherDiscretionaryExpensesFrequencyId;
        existing.UseGroupedEntry = expenses.UseGroupedEntry;
        existing.GroupedAmount = expenses.GroupedAmount;
        existing.GroupedFrequencyId = expenses.GroupedFrequencyId;
        existing.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existing;
    }

    // ── Debt Repayments ──────────────────────────────────────────────────────

    public async Task<List<SpendingDebtRepayment>> GetDebtRepaymentsAsync(long scenarioId, long timelineId) =>
        await context.SpendingDebtRepayments
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
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
        existing.Name = item.Name;
        existing.Amount = item.Amount;
        existing.FrequencyId = item.FrequencyId;
        existing.GenericDebtId = item.GenericDebtId;
        existing.UpdatedAt = DateTime.UtcNow;
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

    public async Task<List<SpendingAssetsExpense>> GetAssetsExpensesAsync(long scenarioId, long timelineId) =>
        await context.SpendingAssetsExpenses
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
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
        existing.Name = item.Name;
        existing.Expense = item.Expense;
        existing.FrequencyId = item.FrequencyId;
        existing.AssetsHomeId = item.AssetsHomeId;
        existing.AssetsInvestmentPropertyId = item.AssetsInvestmentPropertyId;
        existing.AssetsInvestmentAccountId = item.AssetsInvestmentAccountId;
        existing.AssetsPhysicalAssetId = item.AssetsPhysicalAssetId;
        existing.UpdatedAt = DateTime.UtcNow;
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

    public async Task<List<SpendingOtherExpense>> GetOtherExpensesAsync(long scenarioId, long timelineId) =>
        await context.SpendingOtherExpenses
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
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
        existing.Name = item.Name;
        existing.Amount = item.Amount;
        existing.FrequencyId = item.FrequencyId;
        existing.UpdatedAt = DateTime.UtcNow;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOtherExpenseAsync(long id)
    {
        var item = await context.SpendingOtherExpenses.FindAsync(id);
        if (item == null) return false;
        context.SpendingOtherExpenses.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }

    // ── Investment Expenses ──────────────────────────────────────────────────

    public async Task<List<SpendingInvestmentExpense>> GetInvestmentExpensesAsync(long scenarioId, long timelineId) =>
        await context.SpendingInvestmentExpenses
            .AsNoTracking()
            .Include(e => e.InvestmentAccount)
            .ThenInclude(a => a!.AccountType)
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

    public async Task<SpendingInvestmentExpense> CreateInvestmentExpenseAsync(SpendingInvestmentExpense item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        context.SpendingInvestmentExpenses.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateInvestmentExpenseAsync(SpendingInvestmentExpense item)
    {
        var existing = await context.SpendingInvestmentExpenses.FindAsync(item.Id);
        if (existing == null) return false;
        existing.Amount = item.Amount;
        existing.FrequencyId = item.FrequencyId;
        existing.InvestmentAccountId = item.InvestmentAccountId;
        existing.RetirementTimelineId = item.RetirementTimelineId;
        existing.UpdatedAt = DateTime.UtcNow;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteInvestmentExpenseAsync(long id)
    {
        var item = await context.SpendingInvestmentExpenses.FindAsync(id);
        if (item == null) return false;
        context.SpendingInvestmentExpenses.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }

    // ── Timeline linking ─────────────────────────────────────────────────────

    public async Task CreateEmptyExpensesForTimelineAsync(long scenarioId, long timelineId)
    {
        var now = DateTime.UtcNow;

        context.SpendingLivingExpenses.Add(new SpendingLivingExpenses
        {
            ScenarioId = scenarioId, RetirementTimelineId = timelineId,
            CreatedAt = now, UpdatedAt = now
        });

        context.SpendingDiscretionaryExpenses.Add(new SpendingDiscretionaryExpenses
        {
            ScenarioId = scenarioId, RetirementTimelineId = timelineId,
            CreatedAt = now, UpdatedAt = now
        });

        await context.SaveChangesAsync();
    }

    public async Task CloneExpensesFromTimelineAsync(long scenarioId, long sourceTimelineId, long targetTimelineId)
    {
        var now = DateTime.UtcNow;

        // Clone living expenses
        var sourceLiving = await context.SpendingLivingExpenses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId);

        context.SpendingLivingExpenses.Add(sourceLiving is not null
            ? new SpendingLivingExpenses
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                RentOrMortgage = sourceLiving.RentOrMortgage, RentOrMortgageFrequencyId = sourceLiving.RentOrMortgageFrequencyId,
                Food = sourceLiving.Food, FoodFrequencyId = sourceLiving.FoodFrequencyId,
                Utilities = sourceLiving.Utilities, UtilitiesFrequencyId = sourceLiving.UtilitiesFrequencyId,
                Insurance = sourceLiving.Insurance, InsuranceFrequencyId = sourceLiving.InsuranceFrequencyId,
                Gas = sourceLiving.Gas, GasFrequencyId = sourceLiving.GasFrequencyId,
                HomeMaintenance = sourceLiving.HomeMaintenance, HomeMaintenanceFrequencyId = sourceLiving.HomeMaintenanceFrequencyId,
                PropertyTax = sourceLiving.PropertyTax, PropertyTaxFrequencyId = sourceLiving.PropertyTaxFrequencyId,
                Cellphone = sourceLiving.Cellphone, CellphoneFrequencyId = sourceLiving.CellphoneFrequencyId,
                HealthSpendings = sourceLiving.HealthSpendings, HealthSpendingsFrequencyId = sourceLiving.HealthSpendingsFrequencyId,
                OtherLivingExpenses = sourceLiving.OtherLivingExpenses, OtherLivingExpensesFrequencyId = sourceLiving.OtherLivingExpensesFrequencyId,
                CreatedAt = now, UpdatedAt = now
            }
            : new SpendingLivingExpenses { ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId, CreatedAt = now, UpdatedAt = now });

        // Clone discretionary expenses
        var sourceDisc = await context.SpendingDiscretionaryExpenses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId);

        context.SpendingDiscretionaryExpenses.Add(sourceDisc is not null
            ? new SpendingDiscretionaryExpenses
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                GymMembership = sourceDisc.GymMembership, GymMembershipFrequencyId = sourceDisc.GymMembershipFrequencyId,
                Subscriptions = sourceDisc.Subscriptions, SubscriptionsFrequencyId = sourceDisc.SubscriptionsFrequencyId,
                EatingOut = sourceDisc.EatingOut, EatingOutFrequencyId = sourceDisc.EatingOutFrequencyId,
                Entertainment = sourceDisc.Entertainment, EntertainmentFrequencyId = sourceDisc.EntertainmentFrequencyId,
                Travel = sourceDisc.Travel, TravelFrequencyId = sourceDisc.TravelFrequencyId,
                CharitableDonations = sourceDisc.CharitableDonations, CharitableDonationsFrequencyId = sourceDisc.CharitableDonationsFrequencyId,
                OtherDiscretionaryExpenses = sourceDisc.OtherDiscretionaryExpenses, OtherDiscretionaryExpensesFrequencyId = sourceDisc.OtherDiscretionaryExpensesFrequencyId,
                UseGroupedEntry = sourceDisc.UseGroupedEntry, GroupedAmount = sourceDisc.GroupedAmount, GroupedFrequencyId = sourceDisc.GroupedFrequencyId,
                CreatedAt = now, UpdatedAt = now
            }
            : new SpendingDiscretionaryExpenses { ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId, CreatedAt = now, UpdatedAt = now });

        // Clone debt repayments (list)
        var sourceDebts = await context.SpendingDebtRepayments
            .AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var d in sourceDebts)
            context.SpendingDebtRepayments.Add(new SpendingDebtRepayment
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = d.Name, Amount = d.Amount, FrequencyId = d.FrequencyId,
                GenericDebtId = d.GenericDebtId, CreatedAt = now, UpdatedAt = now
            });

        // Clone assets expenses (list)
        var sourceAssets = await context.SpendingAssetsExpenses
            .AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var a in sourceAssets)
            context.SpendingAssetsExpenses.Add(new SpendingAssetsExpense
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = a.Name, Expense = a.Expense, FrequencyId = a.FrequencyId,
                AssetsHomeId = a.AssetsHomeId, AssetsInvestmentPropertyId = a.AssetsInvestmentPropertyId,
                AssetsInvestmentAccountId = a.AssetsInvestmentAccountId, AssetsPhysicalAssetId = a.AssetsPhysicalAssetId,
                CreatedAt = now, UpdatedAt = now
            });

        // Clone other expenses (list)
        var sourceOther = await context.SpendingOtherExpenses
            .AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var o in sourceOther)
            context.SpendingOtherExpenses.Add(new SpendingOtherExpense
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = o.Name, Amount = o.Amount, FrequencyId = o.FrequencyId,
                CreatedAt = now, UpdatedAt = now
            });

        // Clone investment expenses (list)
        var sourceInvestment = await context.SpendingInvestmentExpenses
            .AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var i in sourceInvestment)
            context.SpendingInvestmentExpenses.Add(new SpendingInvestmentExpense
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Amount = i.Amount, FrequencyId = i.FrequencyId,
                InvestmentAccountId = i.InvestmentAccountId,
                CreatedAt = now, UpdatedAt = now
            });

        await context.SaveChangesAsync();
    }
}
