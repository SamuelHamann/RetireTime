using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.DeleteRetirementSpending;

public partial class DeleteRetirementSpendingHandler(
    IRetirementTimelineRepository repository,
    ILogger<DeleteRetirementSpendingHandler> logger) : IRequestHandler<DeleteRetirementSpendingCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteRetirementSpendingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);
        try
        {
            await repository.DeleteAsync(request.Id);
            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteRetirementSpending for Id: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteRetirementSpendingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Deleted RetirementSpending Id: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteRetirementSpendingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error deleting RetirementSpending Id: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteRetirementSpendingHandler> logger, string Exception, long Id);
}

