using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuideGovernmentPensionRepository
{
    Task<BeginnerGuideGovernmentPension?> GetByUserIdAsync(long userId);
    Task<BeginnerGuideGovernmentPension> UpsertAsync(BeginnerGuideGovernmentPension governmentPension);
}

