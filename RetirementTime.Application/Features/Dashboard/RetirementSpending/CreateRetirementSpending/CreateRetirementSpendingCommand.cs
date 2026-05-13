using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.CreateRetirementSpending;

public record CreateRetirementSpendingCommand(long ScenarioId, string Name = "New Retirement Timeline", RetirementTimelineTypeEnum TimelineType = RetirementTimelineTypeEnum.Expenses) : IRequest<CreateRetirementSpendingResult>;

public record CreateRetirementSpendingResult : BaseResult
{
    public long RetirementSpendingId { get; init; }
}
