using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class RetirementSpendingRepository(ApplicationDbContext context) : IRetirementSpendingRepository
{
    public async Task<List<RetirementSpending>> GetByScenarioIdAsync(long scenarioId) =>
        await context.RetirementSpendings
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.AgeFrom)
            .ToListAsync();

    public async Task<RetirementSpending?> GetByIdAsync(long id) =>
        await context.RetirementSpendings.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<RetirementSpending> CreateAsync(RetirementSpending entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        context.RetirementSpendings.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(RetirementSpending entity)
    {
        var existing = await context.RetirementSpendings.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existing is null) return;

        existing.Name           = entity.Name;
        existing.AgeFrom        = entity.AgeFrom;
        existing.AgeTo          = entity.AgeTo;
        existing.IsFullyCreated = entity.IsFullyCreated;
        existing.UpdatedAt      = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existing = await context.RetirementSpendings.FirstOrDefaultAsync(e => e.Id == id);
        if (existing is null) return;
        context.RetirementSpendings.Remove(existing);
        await context.SaveChangesAsync();
    }
}

