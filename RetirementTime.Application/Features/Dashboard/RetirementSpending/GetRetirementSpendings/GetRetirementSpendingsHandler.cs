using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.GetRetirementSpendings;

public partial class GetRetirementSpendingsHandler(
    IRetirementSpendingRepository repository,
    ILogger<GetRetirementSpendingsHandler> logger) : IRequestHandler<GetRetirementSpendingsQuery, List<RetirementSpendingDto>>
{
    public async Task<List<RetirementSpendingDto>> Handle(GetRetirementSpendingsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var items = await repository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);
            return items.Select(e => new RetirementSpendingDto
            {
                Id             = e.Id,
                Name           = e.Name,
                AgeFrom        = e.AgeFrom,
                AgeTo          = e.AgeTo,
                IsFullyCreated = e.IsFullyCreated
            }).ToList();
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetRetirementSpendings for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetRetirementSpendingsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Retrieved {Count} retirement spendings for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetRetirementSpendingsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error retrieving retirement spendings for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetRetirementSpendingsHandler> logger, string Exception, long ScenarioId);
}

