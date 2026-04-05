namespace RetirementTime.Application.Features.Dashboard.Common;

public record ScenarioDto
{
    public long ScenarioId { get; init; }
    public required string ScenarioName { get; init; }
    public long UserId { get; init; }
    public bool ScenarioFullyCreated { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
