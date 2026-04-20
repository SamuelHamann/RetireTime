using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.CreateScenario;

public partial class CreateScenarioHandler(
    IDashboardScenarioRepository scenarioRepository,
    ILogger<CreateScenarioHandler> logger) : IRequestHandler<CreateScenarioCommand, CreateScenarioResult>
{
    public async Task<CreateScenarioResult> Handle(CreateScenarioCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioName, request.UserId);

        try
        {
            // Create the new scenario
            var scenario = new DashboardScenario
            {
                UserId = request.UserId,
                ScenarioName = request.ScenarioName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var scenarioId = await scenarioRepository.CreateAsync(scenario);

            // If cloning from an existing scenario, clone the data
            if (request.CloneFromScenarioId.HasValue)
            {
                await CloneScenarioData(request.CloneFromScenarioId.Value, scenarioId);
            }

            LogSuccessfullyCompleted(logger, scenarioId, request.ScenarioName, request.UserId);

            return new CreateScenarioResult
            {
                Success = true,
                ScenarioId = scenarioId
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioName, request.UserId);
            return new CreateScenarioResult
            {
                Success = false,
                ErrorMessage = "An error occurred while creating the scenario."
            };
        }
    }

    private async Task CloneScenarioData(long sourceScenarioId, long targetScenarioId)
    {
        // TODO: Implement cloning logic when we have scenario-specific data
        // For now, this is just a placeholder method
        // In the future, this will clone:
        // - Assets associated with the scenario
        // - Debts associated with the scenario
        // - Income sources associated with the scenario
        // - Spending categories associated with the scenario
        // - Any other scenario-specific data

        await Task.CompletedTask;
        LogCloningScenario(logger, sourceScenarioId, targetScenarioId);
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateScenario '{ScenarioName}' for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<CreateScenarioHandler> logger, string ScenarioName, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully created scenario with ID: {ScenarioId}, Name: '{ScenarioName}' for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateScenarioHandler> logger, long ScenarioId, string ScenarioName, long UserId);

    [LoggerMessage(LogLevel.Information, "Cloning scenario data from ScenarioId: {SourceScenarioId} to ScenarioId: {TargetScenarioId}")]
    static partial void LogCloningScenario(ILogger<CreateScenarioHandler> logger, long SourceScenarioId, long TargetScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred for ScenarioName: '{ScenarioName}', UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateScenarioHandler> logger, string Exception, string ScenarioName, long UserId);
}
