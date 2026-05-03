using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateDefinedProfitSharing;

public partial class CreateDefinedProfitSharingHandler(
    IDefinedProfitSharingRepository repository,
    ILogger<CreateDefinedProfitSharingHandler> logger) : IRequestHandler<CreateDefinedProfitSharingCommand, CreateDefinedProfitSharingResult>
{
    public async Task<CreateDefinedProfitSharingResult> Handle(CreateDefinedProfitSharingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var plan = new DefinedProfitSharing { ScenarioId = request.ScenarioId, RetirementTimelineId = request.TimelineId };
            var created = await repository.CreateAsync(plan);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateDefinedProfitSharingResult { Success = true, PlanId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateDefinedProfitSharingResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding the plan. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateDefinedProfitSharing handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateDefinedProfitSharingHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created profit sharing plan with ID: {PlanId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateDefinedProfitSharingHandler> logger, long PlanId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating profit sharing plan for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateDefinedProfitSharingHandler> logger, string Exception, long ScenarioId);
}
