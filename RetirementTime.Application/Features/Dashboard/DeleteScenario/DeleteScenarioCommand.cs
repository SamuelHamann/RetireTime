using MediatR;

namespace RetirementTime.Application.Features.Dashboard.DeleteScenario;

public record DeleteScenarioCommand : IRequest<DeleteScenarioResult>
{
    public required long ScenarioId { get; init; }
    public required long UserId { get; init; }
}

public record DeleteScenarioResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
