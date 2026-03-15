using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuideOtherRecurringGainRepository
{
    Task<List<BeginnerGuideOtherRecurringGain>> GetByUserIdAsync(long userId);
    Task<List<BeginnerGuideOtherRecurringGain>> UpsertGainsAsync(long userId, List<BeginnerGuideOtherRecurringGain> gains);
    Task DeleteByUserIdAsync(long userId);
}

