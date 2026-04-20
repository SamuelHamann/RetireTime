using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetAssetsHome;

public partial class GetAssetsHomeHandler(
    IAssetsHomeRepository repository,
    ILogger<GetAssetsHomeHandler> logger) : IRequestHandler<GetAssetsHomeQuery, AssetsHome?>
{
    public async Task<AssetsHome?> Handle(GetAssetsHomeQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var item = await repository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return item;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return null;
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssetsHome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetAssetsHomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved assets home for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetAssetsHomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting assets home for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetAssetsHomeHandler> logger, string Exception, long ScenarioId);
}
