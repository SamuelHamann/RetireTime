using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.CreateRetirementSpending;

public record CreateRetirementSpendingCommand(long ScenarioId, string Name = "New Retirement Spending") : IRequest<CreateRetirementSpendingResult>;

public record CreateRetirementSpendingResult : BaseResult
{
    public long RetirementSpendingId { get; init; }
}

