using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuideDebtRepository(ApplicationDbContext context) : IBeginnerGuideDebtRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuideDebt>> GetByUserIdAsync(long userId)
    {
        return await _context.BeginnerGuideDebts
            .Where(d => d.UserId == userId)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BeginnerGuideDebt>> UpsertDebtsAsync(long userId, List<BeginnerGuideDebt> debts)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existing = await _context.BeginnerGuideDebts
                .Where(d => d.UserId == userId)
                .ToListAsync();

            if (existing.Count > 0)
            {
                _context.BeginnerGuideDebts.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;
            foreach (var debt in debts)
            {
                debt.CreatedAt = now;
                debt.UpdatedAt = now;
            }

            await _context.BeginnerGuideDebts.AddRangeAsync(debts);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return debts;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var debts = await _context.BeginnerGuideDebts
            .Where(d => d.UserId == userId)
            .ToListAsync();

        if (debts.Count > 0)
        {
            _context.BeginnerGuideDebts.RemoveRange(debts);
            await _context.SaveChangesAsync();
        }
    }
}

