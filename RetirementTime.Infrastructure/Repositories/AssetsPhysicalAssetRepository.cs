using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class AssetsPhysicalAssetRepository(ApplicationDbContext context) : IAssetsPhysicalAssetRepository
{
    public async Task<List<AssetsPhysicalAsset>> GetByScenarioIdAsync(long scenarioId)
    {
        return await context.AssetsPhysicalAssets
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<PhysicalAssetType>> GetAssetTypesAsync()
    {
        return await context.Set<PhysicalAssetType>().OrderBy(e => e.Id).ToListAsync();
    }

    public async Task<AssetsPhysicalAsset> CreateAsync(AssetsPhysicalAsset asset)
    {
        asset.CreatedAt = DateTime.UtcNow;
        asset.UpdatedAt = DateTime.UtcNow;
        context.AssetsPhysicalAssets.Add(asset);
        await context.SaveChangesAsync();
        return asset;
    }

    public async Task<bool> UpdateAsync(AssetsPhysicalAsset asset)
    {
        var existing = await context.AssetsPhysicalAssets.FindAsync(asset.Id);
        if (existing == null) return false;

        existing.AssetTypeId = asset.AssetTypeId;
        existing.Name = asset.Name;
        existing.EstimatedValue = asset.EstimatedValue;
        existing.AdjustedCostBasis = asset.AdjustedCostBasis;
        existing.IsConsideredPersonalProperty = asset.IsConsideredPersonalProperty;
        existing.IsConsideredAsARetirementAsset = asset.IsConsideredAsARetirementAsset;
        existing.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(long assetId)
    {
        var existing = await context.AssetsPhysicalAssets.FindAsync(assetId);
        if (existing == null) return false;

        context.AssetsPhysicalAssets.Remove(existing);
        return await context.SaveChangesAsync() > 0;
    }
}
