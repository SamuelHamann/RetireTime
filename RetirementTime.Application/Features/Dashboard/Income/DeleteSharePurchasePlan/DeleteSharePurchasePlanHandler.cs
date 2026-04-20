using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteSharePurchasePlan;

public partial class DeleteSharePurchasePlanHandler(
    ISharePurchasePlanRepository repository,
    ILogger<DeleteSharePurchasePlanHandler> logger) : IRequestHandler<DeleteSharePurchasePlanCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteSharePurchasePlanCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Plan record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the plan. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteSharePurchasePlan handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Share purchase plan not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted share purchase plan with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting share purchase plan with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteSharePurchasePlanHandler> logger, string Exception, long Id);
}
