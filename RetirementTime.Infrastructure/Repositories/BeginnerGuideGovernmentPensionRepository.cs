using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuideGovernmentPensionRepository(ApplicationDbContext context) : IBeginnerGuideGovernmentPensionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<BeginnerGuideGovernmentPension?> GetByUserIdAsync(long userId)
    {
        return await _context.GovernmentPensions
            .FirstOrDefaultAsync(gp => gp.UserId == userId);
    }

    public async Task<BeginnerGuideGovernmentPension> UpsertAsync(BeginnerGuideGovernmentPension governmentPension)
    {
        var existing = await _context.GovernmentPensions
            .FirstOrDefaultAsync(gp => gp.UserId == governmentPension.UserId);

        var now = DateTime.UtcNow;

        if (existing is null)
        {
            governmentPension.CreatedAt = now;
            governmentPension.UpdatedAt = now;
            await _context.GovernmentPensions.AddAsync(governmentPension);
            await _context.SaveChangesAsync();
            return governmentPension;
        }

        existing.YearsWorked = governmentPension.YearsWorked;
        existing.HasSpecializedPublicSectorPension = governmentPension.HasSpecializedPublicSectorPension;
        existing.SpecializedPensionName = governmentPension.SpecializedPensionName;
        existing.UpdatedAt = now;

        await _context.SaveChangesAsync();
        return existing;
    }
}

