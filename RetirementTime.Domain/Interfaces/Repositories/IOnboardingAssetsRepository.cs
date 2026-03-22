using RetirementTime.Domain.Entities.Onboarding;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOnboardingAssetsRepository
{
    Task<OnboardingAssets> Upsert(OnboardingAssets assets);
    Task<OnboardingAssets?> GetByUserId(long userId);
}
