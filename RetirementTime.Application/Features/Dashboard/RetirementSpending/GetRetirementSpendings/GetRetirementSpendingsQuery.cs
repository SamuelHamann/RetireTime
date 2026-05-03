using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.GetRetirementSpendings;

public record GetRetirementSpendingsQuery(long ScenarioId) : IRequest<List<RetirementSpendingDto>>;

public record RetirementSpendingDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int AgeFrom { get; init; }
    public int AgeTo { get; init; }
    public RetirementTimelineTypeEnum TimelineType { get; init; }
    public bool IsFullyCreated { get; init; }
}
