using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuideOtherRecurringGainRepository(ApplicationDbContext context) : IBeginnerGuideOtherRecurringGainRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuideOtherRecurringGain>> GetByUserIdAsync(long userId)
    {
        return await _context.OtherRecurringGains
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BeginnerGuideOtherRecurringGain>> UpsertGainsAsync(long userId, List<BeginnerGuideOtherRecurringGain> gains)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existing = await _context.OtherRecurringGains
                .Where(g => g.UserId == userId)
                .ToListAsync();

            if (existing.Count > 0)
            {
                _context.OtherRecurringGains.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;
            foreach (var gain in gains)
            {
                gain.CreatedAt = now;
                gain.UpdatedAt = now;
            }

            await _context.OtherRecurringGains.AddRangeAsync(gains);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return gains;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var gains = await _context.OtherRecurringGains
            .Where(g => g.UserId == userId)
            .ToListAsync();

        if (gains.Count > 0)
        {
            _context.OtherRecurringGains.RemoveRange(gains);
            await _context.SaveChangesAsync();
        }
    }
}

