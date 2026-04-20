using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetAssetsHome;

public record GetAssetsHomeQuery(long ScenarioId) : IRequest<AssetsHome?>;
