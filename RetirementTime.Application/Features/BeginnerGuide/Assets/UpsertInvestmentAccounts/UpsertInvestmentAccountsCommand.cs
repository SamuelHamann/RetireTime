using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentAccounts;

public class UpsertInvestmentAccountsCommand : IRequest<UpsertInvestmentAccountsResult>
{
    public long UserId { get; set; }
    public List<InvestmentAccountDto> Accounts { get; set; } = new();
}

public class InvestmentAccountDto
{
    public int? Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }
    public bool IsBulkAmount { get; set; }
    public decimal? BulkAmount { get; set; }
    public List<StockDto> Stocks { get; set; } = new();
}

public class StockDto
{
    public int? Id { get; set; }
    public string TickerSymbol { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class UpsertInvestmentAccountsResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<int> AccountIds { get; set; } = new();
}

