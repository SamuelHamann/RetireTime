using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.DeleteDebt;

public partial class DeleteDebtHandler(
    IGenericDebtRepository repository,
    ILogger<DeleteDebtHandler> logger) : IRequestHandler<DeleteDebtCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteDebtCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Debt record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the debt. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteDebt handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Debt not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted debt with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting debt with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteDebtHandler> logger, string Exception, long Id);
}
