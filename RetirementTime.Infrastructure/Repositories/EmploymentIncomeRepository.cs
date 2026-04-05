using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class EmploymentIncomeRepository(ApplicationDbContext context) : IEmploymentIncomeRepository
{
    public async Task<List<EmploymentIncome>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.EmploymentIncomes
            .Include(e => e.OtherIncomes)
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<EmploymentIncome> CreateAsync(EmploymentIncome employmentIncome)
    {
        employmentIncome.CreatedAt = DateTime.UtcNow;
        employmentIncome.UpdatedAt = DateTime.UtcNow;
        context.EmploymentIncomes.Add(employmentIncome);
        await context.SaveChangesAsync();
        return employmentIncome;
    }

    public async Task<bool> UpdateAsync(EmploymentIncome employmentIncome)
    {
        var existing = await context.EmploymentIncomes.FindAsync(employmentIncome.Id);
        if (existing == null) return false;

        existing.EmployerName = employmentIncome.EmployerName;
        existing.GrossSalary = employmentIncome.GrossSalary;
        existing.NetSalary = employmentIncome.NetSalary;
        existing.GrossCommissions = employmentIncome.GrossCommissions;
        existing.NetCommissions = employmentIncome.NetCommissions;
        existing.GrossBonus = employmentIncome.GrossBonus;
        existing.NetBonus = employmentIncome.NetBonus;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long employmentIncomeId)
    {
        var existing = await context.EmploymentIncomes.FindAsync(employmentIncomeId);
        if (existing == null) return false;

        context.EmploymentIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<OtherEmploymentIncome> CreateOtherIncomeAsync(OtherEmploymentIncome otherIncome)
    {
        otherIncome.CreatedAt = DateTime.UtcNow;
        otherIncome.UpdatedAt = DateTime.UtcNow;

        var employment = await context.EmploymentIncomes.FindAsync(otherIncome.EmploymentIncomeId);
        otherIncome.EmploymentIncome = employment!;

        context.OtherEmploymentIncomes.Add(otherIncome);
        await context.SaveChangesAsync();
        return otherIncome;
    }

    public async Task<bool> UpdateOtherIncomeAsync(OtherEmploymentIncome otherIncome)
    {
        var existing = await context.OtherEmploymentIncomes.FindAsync(otherIncome.Id);
        if (existing == null) return false;

        existing.Name = otherIncome.Name;
        existing.Gross = otherIncome.Gross;
        existing.Net = otherIncome.Net;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOtherIncomeAsync(long otherIncomeId)
    {
        var existing = await context.OtherEmploymentIncomes.FindAsync(otherIncomeId);
        if (existing == null) return false;

        context.OtherEmploymentIncomes.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
