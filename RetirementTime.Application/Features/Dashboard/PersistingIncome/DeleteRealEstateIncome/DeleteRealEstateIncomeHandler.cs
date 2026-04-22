using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.DeleteRealEstateIncome;

public partial class DeleteRealEstateIncomeHandler(
    IRealEstateIncomeRepository repository,
    ILogger<DeleteRealEstateIncomeHandler> logger) : IRequestHandler<DeleteRealEstateIncomeCommand, DeleteRealEstateIncomeResult>
{
    public async Task<DeleteRealEstateIncomeResult> Handle(DeleteRealEstateIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.IncomeId);
        try
        {
            await repository.DeleteAsync(request.IncomeId);
            LogSuccessfullyCompleted(logger, request.IncomeId);
            return new DeleteRealEstateIncomeResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.IncomeId);
            return new DeleteRealEstateIncomeResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteRealEstateIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteRealEstateIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted real estate income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteRealEstateIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error deleting real estate income for ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteRealEstateIncomeHandler> logger, string Exception, long Id);
}

