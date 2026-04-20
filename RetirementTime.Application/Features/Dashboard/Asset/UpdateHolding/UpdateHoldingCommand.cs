using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateHolding;

public record UpdateHoldingCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string AssetName { get; init; } = string.Empty;
    public bool IsPubliclyTraded { get; init; }
    public decimal? CurrentValue { get; init; }
    public string TickerSymbol { get; init; } = string.Empty;
    public decimal? AdjustedCostBasis { get; init; }
}
