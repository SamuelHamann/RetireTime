using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedContribution;

public partial class DeletePensionDefinedContributionHandler(
    IPensionDefinedContributionRepository repository,
    ILogger<DeletePensionDefinedContributionHandler> logger) : IRequestHandler<DeletePensionDefinedContributionCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeletePensionDefinedContributionCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Plan record not found." };
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
                ErrorMessage = "An error occurred while deleting the plan. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeletePensionDefinedContribution handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeletePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Defined contribution plan not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeletePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted defined contribution plan with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeletePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting defined contribution plan with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeletePensionDefinedContributionHandler> logger, string Exception, long Id);
}
