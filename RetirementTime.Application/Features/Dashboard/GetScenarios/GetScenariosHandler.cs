using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Features.Dashboard.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.GetScenarios;

public partial class GetScenariosHandler(
    IDashboardScenarioRepository scenarioRepository,
    ILogger<GetScenariosHandler> logger) : IRequestHandler<GetScenariosQuery, GetScenariosResult>
{
    public async Task<GetScenariosResult> Handle(GetScenariosQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var scenarios = await scenarioRepository.GetAllByUserIdAsync(request.UserId);

            var scenarioDtos = scenarios.Select(s => new ScenarioDto
            {
                ScenarioId = s.ScenarioId,
                ScenarioName = s.ScenarioName,
                UserId = s.UserId,
                ScenarioFullyCreated = s.ScenarioFullyCreated,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            LogSuccessfullyCompleted(logger, scenarioDtos.Count, request.UserId);

            return new GetScenariosResult
            {
                Success = true,
                Scenarios = scenarioDtos
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetScenariosResult
            {
                Success = false,
                ErrorMessage = "An error occurred while retrieving scenarios."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetScenarios for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<GetScenariosHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} scenarios for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetScenariosHandler> logger, int Count, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetScenariosHandler> logger, string Exception, long UserId);
}
