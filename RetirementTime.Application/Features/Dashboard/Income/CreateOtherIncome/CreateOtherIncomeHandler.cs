using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncome;

public partial class CreateOtherIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<CreateOtherIncomeHandler> logger) : IRequestHandler<CreateOtherIncomeCommand, CreateOtherIncomeResult>
{
    public async Task<CreateOtherIncomeResult> Handle(CreateOtherIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.EmploymentIncomeId);

        try
        {
            var otherIncome = new OtherEmploymentIncome
            {
                EmploymentIncomeId = request.EmploymentIncomeId,
                EmploymentIncome = null!
            };

            var created = await employmentIncomeRepository.CreateOtherIncomeAsync(otherIncome);

            LogSuccessfullyCompleted(logger, created.Id, request.EmploymentIncomeId);

            return new CreateOtherIncomeResult { Success = true, OtherIncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.EmploymentIncomeId);
            return new CreateOtherIncomeResult
            {
                Success = false,
                ErrorMessage = "An error occurred while adding other income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateOtherIncome handler for EmploymentIncomeId: {EmploymentIncomeId}")]
    static partial void LogStartingHandler(ILogger<CreateOtherIncomeHandler> logger, long EmploymentIncomeId);

    [LoggerMessage(LogLevel.Information, "Successfully created other income with ID: {OtherIncomeId} for EmploymentIncomeId: {EmploymentIncomeId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateOtherIncomeHandler> logger, long OtherIncomeId, long EmploymentIncomeId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating other income for EmploymentIncomeId: {EmploymentIncomeId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateOtherIncomeHandler> logger, string Exception, long EmploymentIncomeId);
}
