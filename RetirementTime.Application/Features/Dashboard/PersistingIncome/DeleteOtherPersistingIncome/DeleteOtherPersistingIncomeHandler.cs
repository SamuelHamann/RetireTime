using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.DeleteOtherPersistingIncome;

public partial class DeleteOtherPersistingIncomeHandler(
    IOtherPersistingIncomeRepository repository,
    ILogger<DeleteOtherPersistingIncomeHandler> logger) : IRequestHandler<DeleteOtherPersistingIncomeCommand, DeleteOtherPersistingIncomeResult>
{
    public async Task<DeleteOtherPersistingIncomeResult> Handle(DeleteOtherPersistingIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.IncomeId);
        try
        {
            await repository.DeleteAsync(request.IncomeId);
            LogSuccessfullyCompleted(logger, request.IncomeId);
            return new DeleteOtherPersistingIncomeResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.IncomeId);
            return new DeleteOtherPersistingIncomeResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteOtherPersistingIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteOtherPersistingIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted other persisting income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteOtherPersistingIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error deleting other persisting income for ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteOtherPersistingIncomeHandler> logger, string Exception, long Id);
}

