using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteSelfEmploymentIncome;

public partial class DeleteSelfEmploymentIncomeHandler(
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    ILogger<DeleteSelfEmploymentIncomeHandler> logger) : IRequestHandler<DeleteSelfEmploymentIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteSelfEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await selfEmploymentIncomeRepository.DeleteAsync(request.Id);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Self-employment income record not found." };
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
                ErrorMessage = "An error occurred while deleting self-employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteSelfEmploymentIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Self-employment income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted self-employment income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting self-employment income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteSelfEmploymentIncomeHandler> logger, string Exception, long Id);
}
