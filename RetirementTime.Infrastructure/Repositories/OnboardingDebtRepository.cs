using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class OnboardingDebtRepository(ApplicationDbContext context) : IOnboardingDebtRepository
{
    public async Task<OnboardingDebt> Upsert(OnboardingDebt debt)
    {
        var existing = await context.OnboardingDebts
            .FirstOrDefaultAsync(d => d.UserId == debt.UserId);

        if (existing != null)
        {
            // Update existing record
            existing.HasPrimaryMortgage = debt.HasPrimaryMortgage;
            existing.HasInvestmentPropertyMortgage = debt.HasInvestmentPropertyMortgage;
            existing.HasCarPayments = debt.HasCarPayments;
            existing.HasStudentLoans = debt.HasStudentLoans;
            existing.HasCreditCardDebt = debt.HasCreditCardDebt;
            existing.HasPersonalLoans = debt.HasPersonalLoans;
            existing.HasBusinessLoans = debt.HasBusinessLoans;
            existing.HasTaxDebt = debt.HasTaxDebt;
            existing.HasMedicalDebt = debt.HasMedicalDebt;
            existing.HasInformalDebt = debt.HasInformalDebt;
            existing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return existing;
        }

        // Create new record
        debt.CreatedAt = DateTime.UtcNow;
        debt.UpdatedAt = DateTime.UtcNow;
        context.OnboardingDebts.Add(debt);
        await context.SaveChangesAsync();
        return debt;
    }

    public async Task<OnboardingDebt?> GetByUserId(long userId)
    {
        return await context.OnboardingDebts
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }
}
