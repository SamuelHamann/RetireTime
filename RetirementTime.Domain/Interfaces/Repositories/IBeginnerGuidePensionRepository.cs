using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuidePensionRepository
{
    Task<List<BeginnerGuidePension>> GetByUserIdAsync(long userId);
    Task<List<BeginnerGuidePension>> UpsertPensionsAsync(long userId, List<BeginnerGuidePension> pensions);
    Task DeleteByUserIdAsync(long userId);
}

