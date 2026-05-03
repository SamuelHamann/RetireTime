using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncomeOrBenefits;

public partial class CreateOtherIncomeOrBenefitsHandler(
    IOtherIncomeOrBenefitsRepository repository,
    ILogger<CreateOtherIncomeOrBenefitsHandler> logger) : IRequestHandler<CreateOtherIncomeOrBenefitsCommand, CreateOtherIncomeOrBenefitsResult>
{
    public async Task<CreateOtherIncomeOrBenefitsResult> Handle(CreateOtherIncomeOrBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var income = new OtherIncomeOrBenefits { ScenarioId = request.ScenarioId, RetirementTimelineId = request.TimelineId };
            var created = await repository.CreateAsync(income);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateOtherIncomeOrBenefitsResult { Success = true, IncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateOtherIncomeOrBenefitsResult { Success = false, ErrorMessage = "An error occurred while adding the income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateOtherIncomeOrBenefits handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateOtherIncomeOrBenefitsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created other income/benefit with ID: {IncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateOtherIncomeOrBenefitsHandler> logger, long IncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating other income/benefit for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateOtherIncomeOrBenefitsHandler> logger, string Exception, long ScenarioId);
}
