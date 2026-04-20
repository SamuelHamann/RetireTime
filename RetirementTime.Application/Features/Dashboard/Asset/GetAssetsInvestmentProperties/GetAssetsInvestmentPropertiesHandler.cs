using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetAssetsInvestmentProperties;

public partial class GetAssetsInvestmentPropertiesHandler(
    IAssetsInvestmentPropertyRepository repository,
    ILogger<GetAssetsInvestmentPropertiesHandler> logger) : IRequestHandler<GetAssetsInvestmentPropertiesQuery, List<AssetsInvestmentProperty>>
{
    public async Task<List<AssetsInvestmentProperty>> Handle(GetAssetsInvestmentPropertiesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var items = await repository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);
            return items;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssetsInvestmentProperties handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetAssetsInvestmentPropertiesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} investment properties for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetAssetsInvestmentPropertiesHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting investment properties for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetAssetsInvestmentPropertiesHandler> logger, string Exception, long ScenarioId);
}
