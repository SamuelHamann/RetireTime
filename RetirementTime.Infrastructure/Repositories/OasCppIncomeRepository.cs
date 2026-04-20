using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OasCppIncomeRepository(ApplicationDbContext context) : IOasCppIncomeRepository
{
    public async Task<OasCppIncome?> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.OasCppIncomes
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId);
    }

    public async Task<OasCppIncome> UpsertAsync(OasCppIncome income)
    {
        var existing = await context.OasCppIncomes
            .FirstOrDefaultAsync(e => e.ScenarioId == income.ScenarioId);

        if (existing == null)
        {
            income.CreatedAt = DateTime.UtcNow;
            income.UpdatedAt = DateTime.UtcNow;
            context.OasCppIncomes.Add(income);
            await context.SaveChangesAsync();
            return income;
        }

        existing.IncomeLastYear = income.IncomeLastYear;
        existing.Income2YearsAgo = income.Income2YearsAgo;
        existing.Income3YearsAgo = income.Income3YearsAgo;
        existing.Income4YearsAgo = income.Income4YearsAgo;
        existing.Income5YearsAgo = income.Income5YearsAgo;
        existing.YearsSpentInCanada = income.YearsSpentInCanada;
        existing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }
}
