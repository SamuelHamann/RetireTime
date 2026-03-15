using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentAccounts;

public partial class UpsertInvestmentAccountsHandler(
    IBeginnerGuideAssetsInvestmentAccountRepository investmentAccountRepository,
    IBeginnerGuideAssetsStockDataRepository stockDataRepository,
    ILogger<UpsertInvestmentAccountsHandler> logger) : IRequestHandler<UpsertInvestmentAccountsCommand, UpsertInvestmentAccountsResult>
{
    public async Task<UpsertInvestmentAccountsResult> Handle(UpsertInvestmentAccountsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.Accounts.Count);

        try
        {
            // Validate all accounts
            foreach (var accountDto in request.Accounts)
            {
                if (string.IsNullOrWhiteSpace(accountDto.AccountName))
                {
                    LogValidationFailed(logger, "Account name is required");
                    return new UpsertInvestmentAccountsResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a name for all accounts."
                    };
                }

                if (accountDto.AccountTypeId <= 0)
                {
                    LogValidationFailed(logger, "Invalid account type");
                    return new UpsertInvestmentAccountsResult
                    {
                        Success = false,
                        ErrorMessage = "Please select a valid account type for all accounts."
                    };
                }

                if (accountDto.IsBulkAmount)
                {
                    // Bulk amount validation
                    if (!accountDto.BulkAmount.HasValue || accountDto.BulkAmount.Value <= 0)
                    {
                        LogValidationFailed(logger, "Invalid bulk amount");
                        return new UpsertInvestmentAccountsResult
                        {
                            Success = false,
                            ErrorMessage = "Please enter a valid amount for all accounts."
                        };
                    }
                    
                    // Automatically remove stocks if bulk amount is selected
                    if (accountDto.Stocks.Count > 0)
                    {
                        LogDataCleanup(logger, "Removing stocks for bulk amount account", accountDto.AccountName);
                        accountDto.Stocks.Clear();
                    }
                }
                else
                {
                    // Stocks validation
                    if (accountDto.Stocks.Count == 0)
                    {
                        LogValidationFailed(logger, "No stocks provided");
                        return new UpsertInvestmentAccountsResult
                        {
                            Success = false,
                            ErrorMessage = "Please add at least one holding for accounts with individual holdings."
                        };
                    }
                    
                    // Automatically clear bulk amount if stocks are selected
                    if (accountDto.BulkAmount.HasValue && accountDto.BulkAmount.Value != 0)
                    {
                        LogDataCleanup(logger, "Clearing bulk amount for stock account", accountDto.AccountName);
                        accountDto.BulkAmount = null;
                    }

                    foreach (var stock in accountDto.Stocks)
                    {
                        if (string.IsNullOrWhiteSpace(stock.TickerSymbol))
                        {
                            LogValidationFailed(logger, "Ticker symbol is required");
                            return new UpsertInvestmentAccountsResult
                            {
                                Success = false,
                                ErrorMessage = "Please enter a ticker symbol for all holdings."
                            };
                        }

                        if (stock.Amount <= 0)
                        {
                            LogValidationFailed(logger, "Invalid stock amount");
                            return new UpsertInvestmentAccountsResult
                            {
                                Success = false,
                                ErrorMessage = "Please enter a valid amount for all holdings."
                            };
                        }
                    }
                }
            }
            
            // Prepare all accounts and stocks for bulk insert
            var accounts = new List<BeginnerGuideAssetsInvestmentAccount>();
            var accountStocksMap = new Dictionary<int, List<StockDto>>();

            for (int i = 0; i < request.Accounts.Count; i++)
            {
                var accountDto = request.Accounts[i];
                var account = new BeginnerGuideAssetsInvestmentAccount
                {
                    UserId = request.UserId,
                    AccountName = accountDto.AccountName,
                    AccountTypeId = accountDto.AccountTypeId,
                    IsBulkAmount = accountDto.IsBulkAmount,
                    // Ensure mutual exclusivity: bulk amount OR stocks, never both
                    BulkAmount = accountDto.IsBulkAmount ? accountDto.BulkAmount : null,
                    User = null!,
                    AccountType = null!
                };

                accounts.Add(account);

                // Store stocks to add after accounts are saved (only if NOT bulk amount)
                if (!accountDto.IsBulkAmount && accountDto.Stocks.Any())
                {
                    accountStocksMap[i] = accountDto.Stocks;
                }
            }

            // Execute delete and inserts in a single transaction
            var (_, accountIds) = await investmentAccountRepository.UpsertAccountsAsync(
                request.UserId,
                accounts,
                accountStocksMap,
                stockDataRepository);

            LogUpsertSuccessful(logger, request.UserId, accountIds.Count);

            return new UpsertInvestmentAccountsResult
            {
                Success = true,
                AccountIds = accountIds
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertInvestmentAccountsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your investment accounts. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting upsert for UserId: {UserId}, AccountCount: {AccountCount}")]
    static partial void LogStartingUpsert(ILogger logger, long userId, int accountCount);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger logger, string reason);

    [LoggerMessage(LogLevel.Information, "Data cleanup: {Action} for account: {AccountName}")]
    static partial void LogDataCleanup(ILogger logger, string action, string accountName);

    [LoggerMessage(LogLevel.Information, "Successfully upserted investment accounts for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}

