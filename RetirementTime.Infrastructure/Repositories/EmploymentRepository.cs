using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class EmploymentRepository(ApplicationDbContext context) : IEmploymentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<BeginnerGuideEmployment>> GetByUserIdAsync(long userId)
    {
        return await _context.Employments
            .Include(e => e.AdditionalCompensations)
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.EmployerName)
            .ToListAsync();
    }

    public async Task<List<BeginnerGuideEmployment>> UpsertEmploymentsAsync(
        long userId,
        List<BeginnerGuideEmployment> employments)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Delete existing employments and their compensations for this user
            var existingEmployments = await _context.Employments
                .Include(e => e.AdditionalCompensations)
                .Where(e => e.UserId == userId)
                .ToListAsync();
            
            if (existingEmployments.Any())
            {
                _context.Employments.RemoveRange(existingEmployments);
                await _context.SaveChangesAsync();
            }

            // 2. Insert all new employments and compensations in bulk
            var now = DateTime.UtcNow;
            foreach (var employment in employments)
            {
                employment.CreatedAt = now;
                employment.UpdatedAt = now;

                foreach (var compensation in employment.AdditionalCompensations)
                {
                    compensation.CreatedAt = now;
                    compensation.UpdatedAt = now;
                }
            }
            
            await _context.Employments.AddRangeAsync(employments);
            await _context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return employments;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var employments = await _context.Employments
            .Where(e => e.UserId == userId)
            .ToListAsync();
        
        _context.Employments.RemoveRange(employments);
        await _context.SaveChangesAsync();
    }
}
