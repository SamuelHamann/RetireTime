using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.DeleteScenario;

public partial class DeleteScenarioHandler(
    IDashboardScenarioRepository scenarioRepository,
    ILogger<DeleteScenarioHandler> logger) : IRequestHandler<DeleteScenarioCommand, DeleteScenarioResult>
{
    public async Task<DeleteScenarioResult> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);

            if (scenario == null)
            {
                LogScenarioNotFound(logger, request.ScenarioId);
                return new DeleteScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Scenario not found."
                };
            }

            // Verify ownership
            if (scenario.UserId != request.UserId)
            {
                LogUnauthorizedAccess(logger, request.ScenarioId, request.UserId);
                return new DeleteScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized access to scenario."
                };
            }

            var deleted = await scenarioRepository.DeleteAsync(request.ScenarioId);

            if (!deleted)
            {
                LogDeleteFailed(logger, request.ScenarioId);
                return new DeleteScenarioResult
                {
                    Success = false,
                    ErrorMessage = "Failed to delete scenario."
                };
            }

            LogSuccessfullyCompleted(logger, request.ScenarioId);

            return new DeleteScenarioResult
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new DeleteScenarioResult
            {
                Success = false,
                ErrorMessage = "An error occurred while deleting the scenario."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteScenario for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<DeleteScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Warning, "Scenario not found: ScenarioId {ScenarioId}")]
    static partial void LogScenarioNotFound(ILogger<DeleteScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Warning, "Unauthorized delete attempt for ScenarioId: {ScenarioId} by UserId: {UserId}")]
    static partial void LogUnauthorizedAccess(ILogger<DeleteScenarioHandler> logger, long ScenarioId, long UserId);

    [LoggerMessage(LogLevel.Error, "Failed to delete scenario from database: ScenarioId {ScenarioId}")]
    static partial void LogDeleteFailed(ILogger<DeleteScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully deleted ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteScenarioHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteScenarioHandler> logger, string Exception, long ScenarioId);
}
