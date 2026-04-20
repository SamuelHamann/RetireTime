using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedBenefits;

public partial class DeletePensionDefinedBenefitsHandler(
    IPensionDefinedBenefitsRepository repository,
    ILogger<DeletePensionDefinedBenefitsHandler> logger) : IRequestHandler<DeletePensionDefinedBenefitsCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeletePensionDefinedBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Pension record not found." };
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
                ErrorMessage = "An error occurred while deleting the pension. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeletePensionDefinedBenefits handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeletePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Defined benefits pension not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeletePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted defined benefits pension with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeletePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting defined benefits pension with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeletePensionDefinedBenefitsHandler> logger, string Exception, long Id);
}
