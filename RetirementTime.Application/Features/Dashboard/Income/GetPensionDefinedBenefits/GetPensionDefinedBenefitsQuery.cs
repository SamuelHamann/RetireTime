using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedBenefits;

public record GetPensionDefinedBenefitsQuery(long ScenarioId) : IRequest<List<PensionDefinedBenefits>>;
