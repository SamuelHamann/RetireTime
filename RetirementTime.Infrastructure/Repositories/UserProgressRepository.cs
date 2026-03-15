using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class UserProgressRepository(ApplicationDbContext context) : IUserProgressRepository
{
    public async Task<UserProgress?> GetByUserId(long userId)
    {
        return await context.UserProgresses
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<UserProgress> Upsert(UserProgress userProgress)
    {
        var existing = await context.UserProgresses
            .FirstOrDefaultAsync(p => p.UserId == userProgress.UserId);

        if (existing == null)
        {
            context.UserProgresses.Add(userProgress);
        }
        else
        {
            existing.HasFinishedBeginnerGuide = userProgress.HasFinishedBeginnerGuide;
            context.UserProgresses.Update(existing);
        }

        await context.SaveChangesAsync();
        return userProgress;
    }
}

