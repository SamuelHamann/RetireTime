using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteAssetsInvestmentProperty;

public partial class DeleteAssetsInvestmentPropertyHandler(
    IAssetsInvestmentPropertyRepository repository,
    ILogger<DeleteAssetsInvestmentPropertyHandler> logger) : IRequestHandler<DeleteAssetsInvestmentPropertyCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteAssetsInvestmentPropertyCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Property record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the property. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteAssetsInvestmentProperty handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Investment property not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted investment property with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting investment property with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteAssetsInvestmentPropertyHandler> logger, string Exception, long Id);
}
