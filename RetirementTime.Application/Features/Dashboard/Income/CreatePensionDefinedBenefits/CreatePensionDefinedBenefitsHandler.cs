using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedBenefits;

public partial class CreatePensionDefinedBenefitsHandler(
    IPensionDefinedBenefitsRepository repository,
    ILogger<CreatePensionDefinedBenefitsHandler> logger) : IRequestHandler<CreatePensionDefinedBenefitsCommand, CreatePensionDefinedBenefitsResult>
{
    public async Task<CreatePensionDefinedBenefitsResult> Handle(CreatePensionDefinedBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var pension = new PensionDefinedBenefits { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(pension);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);

            return new CreatePensionDefinedBenefitsResult { Success = true, PensionId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreatePensionDefinedBenefitsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding the pension. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreatePensionDefinedBenefits handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreatePensionDefinedBenefitsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created defined benefits pension with ID: {PensionId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreatePensionDefinedBenefitsHandler> logger, long PensionId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating defined benefits pension for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreatePensionDefinedBenefitsHandler> logger, string Exception, long ScenarioId);
}
