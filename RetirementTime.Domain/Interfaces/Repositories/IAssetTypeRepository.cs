using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IAssetTypeRepository
{
    Task<List<BeginnerGuideAssetType>> GetAllAsync();
    Task<BeginnerGuideAssetType?> GetByIdAsync(int id);
}

