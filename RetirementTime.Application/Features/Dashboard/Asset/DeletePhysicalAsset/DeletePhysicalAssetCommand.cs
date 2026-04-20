using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeletePhysicalAsset;

public record DeletePhysicalAssetCommand(long Id) : IRequest<BaseResult>;
