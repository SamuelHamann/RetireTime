using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncomeOrBenefits;

public record CreateOtherIncomeOrBenefitsCommand(long ScenarioId, long TimelineId) : IRequest<CreateOtherIncomeOrBenefitsResult>;

public record CreateOtherIncomeOrBenefitsResult : BaseResult
{
    public long IncomeId { get; init; }
}
