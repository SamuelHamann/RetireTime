using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedContribution;

public partial class CreatePensionDefinedContributionHandler(
    IPensionDefinedContributionRepository repository,
    ILogger<CreatePensionDefinedContributionHandler> logger) : IRequestHandler<CreatePensionDefinedContributionCommand, CreatePensionDefinedContributionResult>
{
    public async Task<CreatePensionDefinedContributionResult> Handle(CreatePensionDefinedContributionCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var plan = new PensionDefinedContribution { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(plan);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreatePensionDefinedContributionResult { Success = true, PlanId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreatePensionDefinedContributionResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding the plan. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreatePensionDefinedContribution handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreatePensionDefinedContributionHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created defined contribution plan with ID: {PlanId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreatePensionDefinedContributionHandler> logger, long PlanId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating defined contribution plan for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreatePensionDefinedContributionHandler> logger, string Exception, long ScenarioId);
}
