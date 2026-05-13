using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePropertyIncome;

public record CreatePropertyIncomeCommand(
    long ScenarioId,
    long TimelineId,
    long? InvestmentPropertyId,
    string Name) : IRequest<CreatePropertyIncomeResult>;

public record CreatePropertyIncomeResult : BaseResult
{
    public long IncomeId { get; init; }
}

