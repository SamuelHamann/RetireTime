using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuidePensionRepository(ApplicationDbContext context) : IBeginnerGuidePensionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuidePension>> GetByUserIdAsync(long userId)
    {
        return await _context.BeginnerGuidePensions
            .Where(p => p.UserId == userId)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BeginnerGuidePension>> UpsertPensionsAsync(long userId, List<BeginnerGuidePension> pensions)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existing = await _context.BeginnerGuidePensions
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (existing.Count > 0)
            {
                _context.BeginnerGuidePensions.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;
            foreach (var pension in pensions)
            {
                pension.CreatedAt = now;
                pension.UpdatedAt = now;
            }

            await _context.BeginnerGuidePensions.AddRangeAsync(pensions);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return pensions;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var pensions = await _context.BeginnerGuidePensions
            .Where(p => p.UserId == userId)
            .ToListAsync();

        if (pensions.Count > 0)
        {
            _context.BeginnerGuidePensions.RemoveRange(pensions);
            await _context.SaveChangesAsync();
        }
    }
}

