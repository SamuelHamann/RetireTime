using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OnboardingPersonalInfoRepository(ApplicationDbContext context) : IOnboardingPersonalInfoRepository
{
    public async Task<OnboardingPersonalInfo?> GetByUserId(long userId)
    {
        return await context.OnboardingPersonalInfos
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<OnboardingPersonalInfo> Upsert(OnboardingPersonalInfo personalInfo)
    {
        var existing = await context.OnboardingPersonalInfos
            .FirstOrDefaultAsync(p => p.UserId == personalInfo.UserId);

        if (existing != null)
        {
            // Update existing record
            existing.DateOfBirth = personalInfo.DateOfBirth;
            existing.CitizenshipStatus = personalInfo.CitizenshipStatus;
            existing.MaritalStatus = personalInfo.MaritalStatus;
            existing.HasCurrentChildren = personalInfo.HasCurrentChildren;
            existing.PlansFutureChildren = personalInfo.PlansFutureChildren;
            existing.IncludePartner = personalInfo.IncludePartner;
            existing.UpdatedAt = DateTime.UtcNow;

            context.OnboardingPersonalInfos.Update(existing);
        }
        else
        {
            // Insert new record
            personalInfo.CreatedAt = DateTime.UtcNow;
            personalInfo.UpdatedAt = DateTime.UtcNow;
            await context.OnboardingPersonalInfos.AddAsync(personalInfo);
        }

        await context.SaveChangesAsync();
        return existing ?? personalInfo;
    }
}
