using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Application.Features.Dashboard.NetWorth.SnapshotNetWorth;

public partial class SnapshotNetWorthHandler(
    IDashboardScenarioRepository scenarioRepository,
    INetWorthSnapshotService netWorthSnapshotService,
    ILogger<SnapshotNetWorthHandler> logger) : IRequestHandler<SnapshotNetWorthCommand, SnapshotNetWorthResult>
{
    private const int BatchSize = 10;

    public async Task<SnapshotNetWorthResult> Handle(SnapshotNetWorthCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger);

        try
        {
            var scenarioIds = await scenarioRepository.GetAllScenarioIdsAsync();

            LogRetrievedScenarioIds(logger, scenarioIds.Count);

            var batches = scenarioIds
                .Select((id, index) => (id, index))
                .GroupBy(x => x.index / BatchSize)
                .Select(g => g.Select(x => x.id).ToList())
                .ToList();

            var batchTasks = batches
                .Select(batch => netWorthSnapshotService.ProcessScenarioBatchAsync(batch, cancellationToken))
                .ToList();

            await Task.WhenAll(batchTasks);

            LogSuccessfullyCompleted(logger, scenarioIds.Count);

            return new SnapshotNetWorthResult
            {
                Success = true,
                ScenariosProcessed = scenarioIds.Count
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message);
            return new SnapshotNetWorthResult
            {
                Success = false,
                ErrorMessage = "An error occurred while snapshotting net worth. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SnapshotNetWorth handler")]
    static partial void LogStartingHandler(ILogger<SnapshotNetWorthHandler> logger);

    [LoggerMessage(LogLevel.Information, "Retrieved {Count} scenario IDs to process")]
    static partial void LogRetrievedScenarioIds(ILogger<SnapshotNetWorthHandler> logger, int Count);

    [LoggerMessage(LogLevel.Information, "Successfully completed net worth snapshot for {Count} scenarios")]
    static partial void LogSuccessfullyCompleted(ILogger<SnapshotNetWorthHandler> logger, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred during net worth snapshot | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SnapshotNetWorthHandler> logger, string Exception);
}
