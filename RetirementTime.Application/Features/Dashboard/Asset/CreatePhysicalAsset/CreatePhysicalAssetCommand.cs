using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreatePhysicalAsset;

public record CreatePhysicalAssetCommand(long ScenarioId) : IRequest<CreatePhysicalAssetResult>;

public record CreatePhysicalAssetResult : BaseResult
{
    public long AssetId { get; init; }
}
