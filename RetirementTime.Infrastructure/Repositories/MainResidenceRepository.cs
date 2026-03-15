using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class MainResidenceRepository(ApplicationDbContext context) : IMainResidenceRepository
{
    public async Task<BeginnerGuideMainResidence?> GetByUserIdAsync(long userId)
    {
        return await context.MainResidences
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<BeginnerGuideMainResidence> CreateAsync(BeginnerGuideMainResidence mainResidence)
    {
        mainResidence.CreatedAt = DateTime.UtcNow;
        mainResidence.UpdatedAt = DateTime.UtcNow;
        
        context.MainResidences.Add(mainResidence);
        await context.SaveChangesAsync();
        
        return mainResidence;
    }

    public async Task<BeginnerGuideMainResidence> UpdateAsync(BeginnerGuideMainResidence mainResidence)
    {
        mainResidence.UpdatedAt = DateTime.UtcNow;
        
        context.MainResidences.Update(mainResidence);
        await context.SaveChangesAsync();
        
        return mainResidence;
    }
}

