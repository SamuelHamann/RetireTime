using RetirementTime.Domain.Entities.Onboarding;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOnboardingDebtRepository
{
    Task<OnboardingDebt> Upsert(OnboardingDebt debt);
    Task<OnboardingDebt?> GetByUserId(long userId);
}
