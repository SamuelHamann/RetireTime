using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OnboardingEmploymentRepository(ApplicationDbContext context) : IOnboardingEmploymentRepository
{
    public async Task<OnboardingEmployment> Upsert(OnboardingEmployment employment)
    {
        var existing = await context.OnboardingEmployments
            .FirstOrDefaultAsync(e => e.UserId == employment.UserId);

        if (existing != null)
        {
            // Update existing record
            existing.IsEmployed = employment.IsEmployed;
            existing.IsSelfEmployed = employment.IsSelfEmployed;
            existing.PlannedRetirementAge = employment.PlannedRetirementAge;
            existing.CppContributionYears = employment.CppContributionYears;
            existing.HasRoyalties = employment.HasRoyalties;
            existing.HasDividends = employment.HasDividends;
            existing.HasRentalIncome = employment.HasRentalIncome;
            existing.HasOtherIncome = employment.HasOtherIncome;
            existing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return existing;
        }

        // Create new record
        employment.CreatedAt = DateTime.UtcNow;
        employment.UpdatedAt = DateTime.UtcNow;
        context.OnboardingEmployments.Add(employment);
        await context.SaveChangesAsync();
        return employment;
    }

    public async Task<OnboardingEmployment?> GetByUserId(long userId)
    {
        return await context.OnboardingEmployments
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }
}
