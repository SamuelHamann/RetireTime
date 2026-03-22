using RetirementTime.Domain.Entities.Onboarding;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOnboardingPersonalInfoRepository
{
    Task<OnboardingPersonalInfo?> GetByUserId(long userId);
    Task<OnboardingPersonalInfo> Upsert(OnboardingPersonalInfo personalInfo);
}
