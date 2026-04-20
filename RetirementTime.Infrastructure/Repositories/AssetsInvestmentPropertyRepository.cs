using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AssetsInvestmentPropertyRepository(ApplicationDbContext context) : IAssetsInvestmentPropertyRepository
{
    public async Task<List<AssetsInvestmentProperty>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.AssetsInvestmentProperties
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<AssetsInvestmentProperty> CreateAsync(AssetsInvestmentProperty property)
    {
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        context.AssetsInvestmentProperties.Add(property);
        await context.SaveChangesAsync();
        return property;
    }

    public async Task<bool> UpdateAsync(AssetsInvestmentProperty property)
    {
        var existing = await context.AssetsInvestmentProperties.FindAsync(property.Id);
        if (existing == null) return false;

        existing.Name = property.Name;
        existing.PurchaseDate = property.PurchaseDate;
        existing.PropertyValue = property.PropertyValue;
        existing.PurchasePrice = property.PurchasePrice;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long propertyId)
    {
        var existing = await context.AssetsInvestmentProperties.FindAsync(propertyId);
        if (existing == null) return false;

        context.AssetsInvestmentProperties.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
