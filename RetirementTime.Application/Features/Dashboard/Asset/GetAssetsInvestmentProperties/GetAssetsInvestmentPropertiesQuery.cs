using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetAssetsInvestmentProperties;

public record GetAssetsInvestmentPropertiesQuery(long ScenarioId) : IRequest<List<AssetsInvestmentProperty>>;
