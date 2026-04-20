using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateSharePurchasePlan;

public partial class CreateSharePurchasePlanHandler(
    ISharePurchasePlanRepository repository,
    ILogger<CreateSharePurchasePlanHandler> logger) : IRequestHandler<CreateSharePurchasePlanCommand, CreateSharePurchasePlanResult>
{
    public async Task<CreateSharePurchasePlanResult> Handle(CreateSharePurchasePlanCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var plan = new SharePurchasePlan { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(plan);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateSharePurchasePlanResult { Success = true, PlanId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateSharePurchasePlanResult { Success = false, ErrorMessage = "An error occurred while adding the plan. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateSharePurchasePlan handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateSharePurchasePlanHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created share purchase plan with ID: {PlanId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateSharePurchasePlanHandler> logger, long PlanId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating share purchase plan for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateSharePurchasePlanHandler> logger, string Exception, long ScenarioId);
}
