using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OnboardingAssetsRepository(ApplicationDbContext context) : IOnboardingAssetsRepository
{
    public async Task<OnboardingAssets> Upsert(OnboardingAssets assets)
    {
        var existing = await context.OnboardingAssets
            .FirstOrDefaultAsync(a => a.UserId == assets.UserId);

        if (existing != null)
        {
            // Update existing record
            existing.HasSavingsAccount = assets.HasSavingsAccount;
            existing.HasTFSA = assets.HasTFSA;
            existing.HasRRSP = assets.HasRRSP;
            existing.HasRRIF = assets.HasRRIF;
            existing.HasFHSA = assets.HasFHSA;
            existing.HasRESP = assets.HasRESP;
            existing.HasRDSP = assets.HasRDSP;
            existing.HasNonRegistered = assets.HasNonRegistered;
            existing.HasPension = assets.HasPension;
            existing.HasPrincipalResidence = assets.HasPrincipalResidence;
            existing.HasCar = assets.HasCar;
            existing.HasInvestmentProperty = assets.HasInvestmentProperty;
            existing.HasBusiness = assets.HasBusiness;
            existing.HasIncorporation = assets.HasIncorporation;
            existing.HasPreciousMetals = assets.HasPreciousMetals;
            existing.HasOtherHardAssets = assets.HasOtherHardAssets;
            existing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return existing;
        }

        // Create new record
        assets.CreatedAt = DateTime.UtcNow;
        assets.UpdatedAt = DateTime.UtcNow;
        context.OnboardingAssets.Add(assets);
        await context.SaveChangesAsync();
        return assets;
    }

    public async Task<OnboardingAssets?> GetByUserId(long userId)
    {
        return await context.OnboardingAssets
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }
}
