using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IMainResidenceRepository
{
    Task<BeginnerGuideMainResidence?> GetByUserIdAsync(long userId);
    Task<BeginnerGuideMainResidence> CreateAsync(BeginnerGuideMainResidence mainResidence);
    Task<BeginnerGuideMainResidence> UpdateAsync(BeginnerGuideMainResidence mainResidence);
}

