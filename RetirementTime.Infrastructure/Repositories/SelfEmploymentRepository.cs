using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class SelfEmploymentRepository(ApplicationDbContext context) : ISelfEmploymentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuideSelfEmployment>> GetByUserIdAsync(long userId)
    {
        return await _context.SelfEmployments
            .Include(e => e.AdditionalCompensations)
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.BusinessName)
            .ToListAsync();
    }

    public async Task<List<BeginnerGuideSelfEmployment>> UpsertSelfEmploymentsAsync(
        long userId,
        List<BeginnerGuideSelfEmployment> selfEmployments)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Delete existing self-employments and their compensations for this user
            var existingSelfEmployments = await _context.SelfEmployments
                .Include(e => e.AdditionalCompensations)
                .Where(e => e.UserId == userId)
                .ToListAsync();
            
            if (existingSelfEmployments.Any())
            {
                _context.SelfEmployments.RemoveRange(existingSelfEmployments);
                await _context.SaveChangesAsync();
            }

            // 2. Insert all new self-employments and compensations in bulk
            var now = DateTime.UtcNow;
            foreach (var selfEmployment in selfEmployments)
            {
                selfEmployment.CreatedAt = now;
                selfEmployment.UpdatedAt = now;

                foreach (var compensation in selfEmployment.AdditionalCompensations)
                {
                    compensation.CreatedAt = now;
                    compensation.UpdatedAt = now;
                }
            }
            
            await _context.SelfEmployments.AddRangeAsync(selfEmployments);
            await _context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return selfEmployments;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var selfEmployments = await _context.SelfEmployments
            .Where(e => e.UserId == userId)
            .ToListAsync();
        
        _context.SelfEmployments.RemoveRange(selfEmployments);
        await _context.SaveChangesAsync();
    }
}
