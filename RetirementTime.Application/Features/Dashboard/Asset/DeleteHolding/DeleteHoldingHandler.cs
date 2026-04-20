using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteHolding;

public partial class DeleteHoldingHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<DeleteHoldingHandler> logger) : IRequestHandler<DeleteHoldingCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteHoldingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteHoldingAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Holding record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the holding. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteHolding handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Holding not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted holding with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting holding with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteHoldingHandler> logger, string Exception, long Id);
}
