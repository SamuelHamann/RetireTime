using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.SaveAssetsHome;

public record SaveAssetsHomeCommand : IRequest<BaseResult>
{
    public required long ScenarioId { get; init; }
    public DateTime PurchaseDate { get; init; }
    public decimal? HomeValue { get; init; }
    public decimal? PurchasePrice { get; init; }
}
