using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeletePhysicalAsset;

public partial class DeletePhysicalAssetHandler(
    IAssetsPhysicalAssetRepository repository,
    ILogger<DeletePhysicalAssetHandler> logger) : IRequestHandler<DeletePhysicalAssetCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeletePhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Asset record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the asset. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeletePhysicalAsset handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeletePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Physical asset not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeletePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted physical asset with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeletePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting physical asset with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeletePhysicalAssetHandler> logger, string Exception, long Id);
}
