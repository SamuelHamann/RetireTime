using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentAccounts;

public partial class GetInvestmentAccountsHandler(
    IBeginnerGuideAssetsInvestmentAccountRepository investmentAccountRepository,
    ILogger<GetInvestmentAccountsHandler> logger) : IRequestHandler<GetInvestmentAccountsQuery, GetInvestmentAccountsResult>
{
    public async Task<GetInvestmentAccountsResult> Handle(GetInvestmentAccountsQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var accounts = await investmentAccountRepository.GetByUserIdAsync(request.UserId);

            var accountDtos = accounts.Select(a => new InvestmentAccountDto
            {
                Id = a.Id,
                AccountName = a.AccountName,
                AccountTypeId = a.AccountTypeId,
                IsBulkAmount = a.IsBulkAmount,
                BulkAmount = a.BulkAmount,
                Stocks = a.Stocks.Select(s => new StockDto
                {
                    Id = s.Id,
                    TickerSymbol = s.TickerSymbol,
                    Amount = s.Amount
                }).ToList()
            }).ToList();

            LogQuerySuccessful(logger, request.UserId, accountDtos.Count);

            return new GetInvestmentAccountsResult
            {
                Accounts = accountDtos
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetInvestmentAccountsResult
            {
                Accounts = new List<InvestmentAccountDto>()
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved investment accounts for UserId: {UserId}, Count: {Count}")]
    static partial void LogQuerySuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}

