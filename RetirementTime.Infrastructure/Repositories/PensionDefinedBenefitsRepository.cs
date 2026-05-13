using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class PensionDefinedBenefitsRepository(ApplicationDbContext context) : IPensionDefinedBenefitsRepository
{
    public async Task<List<PensionDefinedBenefits>> GetByScenarioIdAsync(long scenarioId, long timelineId)
    {
        return await context.PensionDefinedBenefits
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == timelineId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<PensionDefinedBenefits> CreateAsync(PensionDefinedBenefits pension)
    {
        pension.CreatedAt = DateTime.UtcNow;
        pension.UpdatedAt = DateTime.UtcNow;
        context.PensionDefinedBenefits.Add(pension);
        await context.SaveChangesAsync();
        return pension;
    }

    public async Task<bool> UpdateAsync(PensionDefinedBenefits pension)
    {
        var existing = await context.PensionDefinedBenefits.FindAsync(pension.Id);
        if (existing == null) return false;

        existing.Name = pension.Name;
        existing.StartAge = pension.StartAge;
        existing.BenefitsPre65 = pension.BenefitsPre65;
        existing.BenefitsPost65 = pension.BenefitsPost65;
        existing.PercentIndexedToInflation = pension.PercentIndexedToInflation;
        existing.PercentSurvivorBenefits = pension.PercentSurvivorBenefits;
        existing.RrspAdjustment = pension.RrspAdjustment;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long pensionId)
    {
        var existing = await context.PensionDefinedBenefits.FindAsync(pensionId);
        if (existing == null) return false;

        context.PensionDefinedBenefits.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
