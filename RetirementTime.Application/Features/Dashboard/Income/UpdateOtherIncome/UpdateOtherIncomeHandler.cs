using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateOtherIncome;

public partial class UpdateOtherIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<UpdateOtherIncomeHandler> logger) : IRequestHandler<UpdateOtherIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateOtherIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var otherIncome = new OtherEmploymentIncome
            {
                Id = request.Id,
                Name = request.Name,
                Gross = request.Gross,
                Net = request.Net,
                EmploymentIncome = null!
            };

            var success = await employmentIncomeRepository.UpdateOtherIncomeAsync(otherIncome);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Other income record not found." };
            }

            LogSuccessfullyCompleted(logger, request.Id);

            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving other income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateOtherIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Other income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated other income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating other income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateOtherIncomeHandler> logger, string Exception, long Id);
}
