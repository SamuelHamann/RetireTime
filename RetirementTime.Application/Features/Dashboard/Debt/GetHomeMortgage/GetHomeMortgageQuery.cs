using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;

namespace RetirementTime.Application.Features.Dashboard.Debt.GetHomeMortgage;

public record GetHomeMortgageQuery(long ScenarioId) : IRequest<GetHomeMortgageResult>;

public record GetHomeMortgageResult
{
    public GenericDebt? Debt { get; init; }
    public List<Frequency> Frequencies { get; init; } = [];
}
