using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateSelfEmploymentIncome;

public partial class CreateSelfEmploymentIncomeHandler(
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    ILogger<CreateSelfEmploymentIncomeHandler> logger) : IRequestHandler<CreateSelfEmploymentIncomeCommand, CreateSelfEmploymentIncomeResult>
{
    public async Task<CreateSelfEmploymentIncomeResult> Handle(CreateSelfEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var selfEmployment = new SelfEmploymentIncome
            {
                ScenarioId = request.ScenarioId
            };

            var created = await selfEmploymentIncomeRepository.CreateAsync(selfEmployment);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);

            return new CreateSelfEmploymentIncomeResult { Success = true, SelfEmploymentIncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateSelfEmploymentIncomeResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding self-employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateSelfEmploymentIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateSelfEmploymentIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created self-employment income with ID: {SelfEmploymentIncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateSelfEmploymentIncomeHandler> logger, long SelfEmploymentIncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating self-employment income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateSelfEmploymentIncomeHandler> logger, string Exception, long ScenarioId);
}
