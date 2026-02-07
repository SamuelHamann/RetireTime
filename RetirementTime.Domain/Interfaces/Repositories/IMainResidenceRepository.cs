using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IMainResidenceRepository
{
    Task<MainResidence?> GetByUserIdAsync(long userId);
    Task<MainResidence> CreateAsync(MainResidence mainResidence);
    Task<MainResidence> UpdateAsync(MainResidence mainResidence);
}

