using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOtherAssetRepository
{
    Task<List<OtherAsset>> GetByUserIdAsync(long userId);
    Task<OtherAsset?> GetByIdAsync(int id);
    Task<OtherAsset> AddAsync(OtherAsset asset);
    Task<OtherAsset> UpdateAsync(OtherAsset asset);
    Task DeleteAsync(int id);
    Task DeleteByUserIdAsync(long userId);
    Task<(List<OtherAsset> SavedAssets, List<int> AssetIds)> UpsertAssetsAsync(long userId, List<OtherAsset> assets);
}

