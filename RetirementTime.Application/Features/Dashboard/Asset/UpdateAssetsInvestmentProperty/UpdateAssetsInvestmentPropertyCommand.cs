using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateAssetsInvestmentProperty;

public record UpdateAssetsInvestmentPropertyCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime PurchaseDate { get; init; }
    public decimal? PropertyValue { get; init; }
    public decimal? PurchasePrice { get; init; }
}
