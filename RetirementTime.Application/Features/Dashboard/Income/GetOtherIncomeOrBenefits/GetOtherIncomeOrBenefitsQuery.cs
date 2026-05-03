using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetOtherIncomeOrBenefits;

public record GetOtherIncomeOrBenefitsQuery(long ScenarioId, long TimelineId) : IRequest<List<OtherIncomeOrBenefits>>;
