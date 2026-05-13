using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateEmploymentIncome;

public partial class CreateEmploymentIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<CreateEmploymentIncomeHandler> logger) : IRequestHandler<CreateEmploymentIncomeCommand, CreateEmploymentIncomeResult>
{
    public async Task<CreateEmploymentIncomeResult> Handle(CreateEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var employment = new EmploymentIncome
            {
                ScenarioId = request.ScenarioId,
                UserId = request.UserId,
                RetirementTimelineId = request.TimelineId
            };

            var created = await employmentIncomeRepository.CreateAsync(employment);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);

            return new CreateEmploymentIncomeResult { Success = true, EmploymentIncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateEmploymentIncomeResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateEmploymentIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateEmploymentIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created employment income with ID: {EmploymentIncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateEmploymentIncomeHandler> logger, long EmploymentIncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating employment income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateEmploymentIncomeHandler> logger, string Exception, long ScenarioId);
}
