using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateGroupRrsp;

public partial class CreateGroupRrspHandler(
    IGroupRrspRepository repository,
    ILogger<CreateGroupRrspHandler> logger) : IRequestHandler<CreateGroupRrspCommand, CreateGroupRrspResult>
{
    public async Task<CreateGroupRrspResult> Handle(CreateGroupRrspCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var plan = new GroupRrsp { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(plan);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateGroupRrspResult { Success = true, PlanId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateGroupRrspResult { Success = false, ErrorMessage = "An error occurred while adding the plan. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateGroupRrsp handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateGroupRrspHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created Group RRSP plan with ID: {PlanId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateGroupRrspHandler> logger, long PlanId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating Group RRSP plan for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateGroupRrspHandler> logger, string Exception, long ScenarioId);
}
