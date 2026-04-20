using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateAssetsInvestmentProperty;

public partial class UpdateAssetsInvestmentPropertyHandler(
    IAssetsInvestmentPropertyRepository repository,
    ILogger<UpdateAssetsInvestmentPropertyHandler> logger) : IRequestHandler<UpdateAssetsInvestmentPropertyCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateAssetsInvestmentPropertyCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var property = new AssetsInvestmentProperty { Id = request.Id, Name = request.Name, PurchaseDate = request.PurchaseDate, PropertyValue = request.PropertyValue, PurchasePrice = request.PurchasePrice };
            var success = await repository.UpdateAsync(property);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Property record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the property. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateAssetsInvestmentProperty handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Investment property not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated investment property with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateAssetsInvestmentPropertyHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating investment property with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateAssetsInvestmentPropertyHandler> logger, string Exception, long Id);
}
