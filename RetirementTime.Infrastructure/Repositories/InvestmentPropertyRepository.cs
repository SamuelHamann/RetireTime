using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class InvestmentPropertyRepository(ApplicationDbContext context) : IInvestmentPropertyRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<InvestmentProperty>> GetByUserIdAsync(long userId)
    {
        return await _context.InvestmentProperties
            .Where(p => p.UserId == userId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<InvestmentProperty?> GetByIdAsync(long id)
    {
        return await _context.InvestmentProperties
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<InvestmentProperty> AddAsync(InvestmentProperty property)
    {
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        
        await _context.InvestmentProperties.AddAsync(property);
        await _context.SaveChangesAsync();
        
        return property;
    }

    public async Task<InvestmentProperty> UpdateAsync(InvestmentProperty property)
    {
        property.UpdatedAt = DateTime.UtcNow;
        
        _context.InvestmentProperties.Update(property);
        await _context.SaveChangesAsync();
        
        return property;
    }

    public async Task DeleteAsync(long id)
    {
        var property = await _context.InvestmentProperties.FindAsync(id);
        if (property != null)
        {
            _context.InvestmentProperties.Remove(property);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var properties = await _context.InvestmentProperties
            .Where(p => p.UserId == userId)
            .ToListAsync();
        
        _context.InvestmentProperties.RemoveRange(properties);
        await _context.SaveChangesAsync();
    }

    public async Task<List<InvestmentProperty>> UpsertPropertiesAsync(
        long userId,
        List<InvestmentProperty> properties)
    {
        // Use a transaction to ensure atomicity
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Delete existing properties for this user
            var existingProperties = await _context.InvestmentProperties
                .Where(p => p.UserId == userId)
                .ToListAsync();
            
            if (existingProperties.Any())
            {
                _context.InvestmentProperties.RemoveRange(existingProperties);
                await _context.SaveChangesAsync();
            }

            // 2. Insert all new properties in bulk
            var now = DateTime.UtcNow;
            foreach (var property in properties)
            {
                property.CreatedAt = now;
                property.UpdatedAt = now;
            }
            
            await _context.InvestmentProperties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return properties;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

