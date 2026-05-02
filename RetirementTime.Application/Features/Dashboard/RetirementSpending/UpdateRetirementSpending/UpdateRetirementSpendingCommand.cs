using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.UpdateRetirementSpending;

public record UpdateRetirementSpendingCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public long ScenarioId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int AgeFrom { get; init; }
    public int AgeTo { get; init; }
    public bool IsFullyCreated { get; init; }
}

