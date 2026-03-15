using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOtherAssetRepository
{
    Task<List<BeginnerGuideOtherAsset>> GetByUserIdAsync(long userId);
    Task<BeginnerGuideOtherAsset?> GetByIdAsync(int id);
    Task<BeginnerGuideOtherAsset> AddAsync(BeginnerGuideOtherAsset asset);
    Task<BeginnerGuideOtherAsset> UpdateAsync(BeginnerGuideOtherAsset asset);
    Task DeleteAsync(int id);
    Task DeleteByUserIdAsync(long userId);
    Task<(List<BeginnerGuideOtherAsset> SavedAssets, List<int> AssetIds)> UpsertAssetsAsync(long userId, List<BeginnerGuideOtherAsset> assets);
}

