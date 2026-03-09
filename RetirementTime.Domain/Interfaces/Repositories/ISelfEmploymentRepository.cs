using RetirementTime.Domain.Entities.BeginnerGuide.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISelfEmploymentRepository
{
    Task<List<BeginnerGuideSelfEmployment>> GetByUserIdAsync(long userId);
    Task<List<BeginnerGuideSelfEmployment>> UpsertSelfEmploymentsAsync(long userId, List<BeginnerGuideSelfEmployment> selfEmployments);
    Task DeleteByUserIdAsync(long userId);
}
