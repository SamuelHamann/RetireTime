using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AssetsHomeRepository(ApplicationDbContext context) : IAssetsHomeRepository
{
    public async Task<AssetsHome?> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.AssetsHomes
            .FirstOrDefaultAsync(e => e.ScenarioId == scenarioId);
    }

    public async Task<AssetsHome> UpsertAsync(AssetsHome home)
    {
        var existing = await context.AssetsHomes
            .FirstOrDefaultAsync(e => e.ScenarioId == home.ScenarioId);

        if (existing == null)
        {
            home.PurchaseDate = DateTime.SpecifyKind(home.PurchaseDate, DateTimeKind.Utc);
            home.CreatedAt = DateTime.UtcNow;
            home.UpdatedAt = DateTime.UtcNow;
            context.AssetsHomes.Add(home);
            await context.SaveChangesAsync();
            return home;
        }

        existing.PurchaseDate = DateTime.SpecifyKind(home.PurchaseDate, DateTimeKind.Utc);
        existing.HomeValue = home.HomeValue;
        existing.PurchasePrice = home.PurchasePrice;
        existing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }
}
