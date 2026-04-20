using MediatR;

namespace RetirementTime.Application.Features.Dashboard.UpdateScenario;

public record UpdateScenarioCommand : IRequest<UpdateScenarioResult>
{
    public required long ScenarioId { get; init; }
    public required long UserId { get; init; }
    public required string ScenarioName { get; init; }
}

public record UpdateScenarioResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
