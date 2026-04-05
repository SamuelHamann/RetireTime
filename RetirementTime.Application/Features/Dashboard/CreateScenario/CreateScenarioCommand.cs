using MediatR;

namespace RetirementTime.Application.Features.Dashboard.CreateScenario;

public record CreateScenarioCommand : IRequest<CreateScenarioResult>
{
    public required long UserId { get; init; }
    public string ScenarioName { get; init; } = "New Scenario";
    public long? CloneFromScenarioId { get; init; }
}

public record CreateScenarioResult
{
    public bool Success { get; init; }
    public long ScenarioId { get; init; }
    public string? ErrorMessage { get; init; }
}
