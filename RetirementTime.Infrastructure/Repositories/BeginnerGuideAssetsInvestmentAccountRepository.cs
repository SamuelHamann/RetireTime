using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class BeginnerGuideAssetsInvestmentAccountRepository : IBeginnerGuideAssetsInvestmentAccountRepository
{
    private readonly ApplicationDbContext _context;

    public BeginnerGuideAssetsInvestmentAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BeginnerGuideAssetsInvestmentAccount?> GetByIdAsync(int id)
    {
        return await _context.InvestmentAccounts
            .Include(a => a.Stocks)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<BeginnerGuideAssetsInvestmentAccount>> GetByUserIdAsync(long userId)
    {
        return await _context.InvestmentAccounts
            .Include(a => a.Stocks)
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<BeginnerGuideAssetsInvestmentAccount> AddAsync(BeginnerGuideAssetsInvestmentAccount account)
    {
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        
        // Enforce business rule: if not a bulk amount account, BulkAmount should be null
        if (!account.IsBulkAmount)
        {
            account.BulkAmount = null;
        }
        
        await _context.InvestmentAccounts.AddAsync(account);
        await _context.SaveChangesAsync();
        
        return account;
    }

    public async Task<List<BeginnerGuideAssetsInvestmentAccount>> AddRangeAsync(List<BeginnerGuideAssetsInvestmentAccount> accounts)
    {
        var now = DateTime.UtcNow;
        foreach (var account in accounts)
        {
            account.CreatedAt = now;
            account.UpdatedAt = now;
            
            // Enforce business rule: if not a bulk amount account, BulkAmount should be null
            if (!account.IsBulkAmount)
            {
                account.BulkAmount = null;
            }
        }
        
        await _context.InvestmentAccounts.AddRangeAsync(accounts);
        await _context.SaveChangesAsync();
        
        return accounts;
    }

    public async Task<BeginnerGuideAssetsInvestmentAccount> UpdateAsync(BeginnerGuideAssetsInvestmentAccount account)
    {
        account.UpdatedAt = DateTime.UtcNow;
        
        // Enforce business rule: if not a bulk amount account, BulkAmount should be null
        if (!account.IsBulkAmount)
        {
            account.BulkAmount = null;
        }
        
        _context.InvestmentAccounts.Update(account);
        await _context.SaveChangesAsync();
        
        return account;
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _context.InvestmentAccounts.FindAsync(id);
        if (account != null)
        {
            _context.InvestmentAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByUserIdAsync(long userId)
    {
        var accounts = await _context.InvestmentAccounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
        
        _context.InvestmentAccounts.RemoveRange(accounts);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<BeginnerGuideAssetsInvestmentAccount> SavedAccounts, List<int> AccountIds)> UpsertAccountsAsync<TStockDto>(
        long userId,
        List<BeginnerGuideAssetsInvestmentAccount> accounts,
        Dictionary<int, List<TStockDto>> accountStocksMap,
        IBeginnerGuideAssetsStockDataRepository stockDataRepository) where TStockDto : class
    {
        // Use a transaction to ensure atomicity
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Delete existing accounts for this user (cascade will delete stocks)
            var existingAccounts = await _context.InvestmentAccounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
            
            if (existingAccounts.Any())
            {
                _context.InvestmentAccounts.RemoveRange(existingAccounts);
                await _context.SaveChangesAsync();
            }

            // 2. Insert all new accounts in bulk
            var now = DateTime.UtcNow;
            foreach (var account in accounts)
            {
                account.CreatedAt = now;
                account.UpdatedAt = now;
                
                // Enforce business rule: bulk amount accounts cannot have stocks
                // Ensure BulkAmount is null if stocks will be added
                if (!account.IsBulkAmount)
                {
                    account.BulkAmount = null;
                }
            }
            
            await _context.InvestmentAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
            
            var accountIds = accounts.Select(a => a.Id).ToList();

            // 3. Prepare and insert all stocks in bulk
            var allStocks = new List<BeginnerGuideAssetsStockData>();
            for (int i = 0; i < accounts.Count; i++)
            {
                // Double-check: only add stocks if the account is NOT a bulk amount account
                if (!accounts[i].IsBulkAmount && accountStocksMap.TryGetValue(i, out var stockDtos))
                {
                    foreach (dynamic stockDto in stockDtos)
                    {
                        var stock = new BeginnerGuideAssetsStockData
                        {
                            InvestmentAccountId = accounts[i].Id,
                            TickerSymbol = stockDto.TickerSymbol,
                            Amount = stockDto.Amount,
                            InvestmentAccount = null!
                        };
                        allStocks.Add(stock);
                    }
                }
            }

            if (allStocks.Any())
            {
                await _context.InvestmentStocks.AddRangeAsync(allStocks);
                await _context.SaveChangesAsync();
            }

            // Commit the transaction
            await transaction.CommitAsync();

            return (accounts, accountIds);
        }
        catch
        {
            // Rollback on any error
            await transaction.RollbackAsync();
            throw;
        }
    }
}

