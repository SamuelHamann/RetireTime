using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedBenefits;

public record CreatePensionDefinedBenefitsCommand(long ScenarioId) : IRequest<CreatePensionDefinedBenefitsResult>;

public record CreatePensionDefinedBenefitsResult : BaseResult
{
    public long PensionId { get; init; }
}
