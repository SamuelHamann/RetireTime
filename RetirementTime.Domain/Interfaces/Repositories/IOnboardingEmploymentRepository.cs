using RetirementTime.Domain.Entities.Onboarding;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOnboardingEmploymentRepository
{
    Task<OnboardingEmployment> Upsert(OnboardingEmployment employment);
    Task<OnboardingEmployment?> GetByUserId(long userId);
}
