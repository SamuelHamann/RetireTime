using RetirementTime.Domain.Entities.BeginnerGuide.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IEmploymentRepository
{
    Task<List<BeginnerGuideEmployment>> GetByUserIdAsync(long userId);
    Task<List<BeginnerGuideEmployment>> UpsertEmploymentsAsync(long userId, List<BeginnerGuideEmployment> employments);
    Task DeleteByUserIdAsync(long userId);
}
