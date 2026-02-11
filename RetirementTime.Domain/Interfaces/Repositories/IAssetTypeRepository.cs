using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetTypeRepository
{
    Task<List<AssetType>> GetAllAsync();
    Task<AssetType?> GetByIdAsync(int id);
}

