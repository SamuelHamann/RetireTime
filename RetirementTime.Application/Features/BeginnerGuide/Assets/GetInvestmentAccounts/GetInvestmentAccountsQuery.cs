using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentAccounts;

public class GetInvestmentAccountsQuery : IRequest<GetInvestmentAccountsResult>
{
    public long UserId { get; set; }
}

public class GetInvestmentAccountsResult
{
    public List<InvestmentAccountDto> Accounts { get; set; } = new();
}

public class InvestmentAccountDto
{
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }
    public bool IsBulkAmount { get; set; }
    public decimal? BulkAmount { get; set; }
    public List<StockDto> Stocks { get; set; } = new();
}

public class StockDto
{
    public int Id { get; set; }
    public string TickerSymbol { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

