using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.SaveExpenseTimeline;

public record SaveExpenseTimelineCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public long ScenarioId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int AgeFrom { get; init; }
    public int AgeTo { get; init; }
    public RetirementTimelineTypeEnum TimelineType { get; init; } = RetirementTimelineTypeEnum.Expenses;
    public bool IsFirstSave { get; init; } = true;
    public long? CloneFromTimelineId { get; init; }
}

