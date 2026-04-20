using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateAssetsInvestmentProperty;

public record CreateAssetsInvestmentPropertyCommand(long ScenarioId) : IRequest<CreateAssetsInvestmentPropertyResult>;

public record CreateAssetsInvestmentPropertyResult : BaseResult
{
    public long PropertyId { get; init; }
}
