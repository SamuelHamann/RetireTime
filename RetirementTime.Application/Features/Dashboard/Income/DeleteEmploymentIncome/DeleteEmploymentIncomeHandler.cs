using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteEmploymentIncome;

public partial class DeleteEmploymentIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<DeleteEmploymentIncomeHandler> logger) : IRequestHandler<DeleteEmploymentIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await employmentIncomeRepository.DeleteAsync(request.Id);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Employment income record not found." };
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
                ErrorMessage = "An error occurred while deleting employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteEmploymentIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Employment income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted employment income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting employment income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteEmploymentIncomeHandler> logger, string Exception, long Id);
}
