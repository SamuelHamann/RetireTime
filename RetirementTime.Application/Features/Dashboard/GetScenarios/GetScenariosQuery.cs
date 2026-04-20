using MediatR;
using RetirementTime.Application.Features.Dashboard.Common;

namespace RetirementTime.Application.Features.Dashboard.GetScenarios;

public record GetScenariosQuery : IRequest<GetScenariosResult>
{
    public required long UserId { get; init; }
}

public record GetScenariosResult
{
    public bool Success { get; init; }
    public List<ScenarioDto> Scenarios { get; init; } = [];
    public string? ErrorMessage { get; init; }
}
