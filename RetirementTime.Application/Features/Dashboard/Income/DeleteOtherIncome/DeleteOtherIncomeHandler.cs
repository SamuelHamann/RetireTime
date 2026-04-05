using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncome;

public partial class DeleteOtherIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<DeleteOtherIncomeHandler> logger) : IRequestHandler<DeleteOtherIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteOtherIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await employmentIncomeRepository.DeleteOtherIncomeAsync(request.Id);

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
                ErrorMessage = "An error occurred while deleting other income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteOtherIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Other income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted other income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteOtherIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting other income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteOtherIncomeHandler> logger, string Exception, long Id);
}
