using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.UpdateScenario;

public partial class UpdateScenarioHandler(
    IDashboardScenarioRepository scenarioRepository,
    ILogger<UpdateScenarioHandler> logger) : IRequestHandler<UpdateScenarioCommand, UpdateScenarioResult>
{
    public async Task<UpdateScenarioResult> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId, request.ScenarioName);

        try
        {
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);

            if (scenario == null)
            {
                LogScenarioNotFound(logger, request.ScenarioId);
                return new UpdateScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Scenario not found."
                };
            }

            // Verify ownership
            if (scenario.UserId != request.UserId)
            {
                LogUnauthorizedAccess(logger, request.ScenarioId, request.UserId);
                return new UpdateScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized access to scenario."
                };
            }

            scenario.ScenarioName = request.ScenarioName;
            scenario.ScenarioFullyCreated = true;
            scenario.UpdatedAt = DateTime.UtcNow;

            var updated = await scenarioRepository.UpdateAsync(scenario);

            if (!updated)
            {
                LogUpdateFailed(logger, request.ScenarioId);
                return new UpdateScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Failed to update scenario."
                };
            }

            LogSuccessfullyCompleted(logger, request.ScenarioId, request.ScenarioName);

            return new UpdateScenarioResult
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new UpdateScenarioResult
            {
                Success = false,
                ErrorMessage = "An error occurred while updating the scenario."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateScenario for ScenarioId: {ScenarioId}, NewName: '{ScenarioName}'")]
    static partial void LogStartingHandler(ILogger<UpdateScenarioHandler> logger, long ScenarioId, string ScenarioName);

    [LoggerMessage(LogLevel.Warning, "Scenario not found: ScenarioId {ScenarioId}")]
    static partial void LogScenarioNotFound(ILogger<UpdateScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Warning, "Unauthorized access attempt to ScenarioId: {ScenarioId} by UserId: {UserId}")]
    static partial void LogUnauthorizedAccess(ILogger<UpdateScenarioHandler> logger, long ScenarioId, long UserId);

    [LoggerMessage(LogLevel.Error, "Failed to update scenario in database: ScenarioId {ScenarioId}")]
    static partial void LogUpdateFailed(ILogger<UpdateScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully updated ScenarioId: {ScenarioId} to '{ScenarioName}'")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateScenarioHandler> logger, long ScenarioId, string ScenarioName);

    [LoggerMessage(LogLevel.Error, "Error occurred for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateScenarioHandler> logger, string Exception, long ScenarioId);
}
