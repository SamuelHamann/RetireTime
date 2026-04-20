using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateInvestmentAccount;

public record UpdateInvestmentAccountCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public long AccountTypeId { get; init; }
    public decimal? AdjustedCostBasis { get; init; }
    public decimal? CurrentTotalValue { get; init; }
    public bool UseIndividualHoldings { get; init; }
}
