using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetsPhysicalAssetRepository
{
    Task<List<AssetsPhysicalAsset>> GetByScenarioIdAsync(long scenarioId);
    Task<List<PhysicalAssetType>> GetAssetTypesAsync();
    Task<AssetsPhysicalAsset> CreateAsync(AssetsPhysicalAsset asset);
    Task<bool> UpdateAsync(AssetsPhysicalAsset asset);
    Task<bool> DeleteAsync(long assetId);
}
