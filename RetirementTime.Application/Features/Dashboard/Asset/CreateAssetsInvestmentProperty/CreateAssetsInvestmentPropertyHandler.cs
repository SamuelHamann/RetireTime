using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateAssetsInvestmentProperty;

public partial class CreateAssetsInvestmentPropertyHandler(
    IAssetsInvestmentPropertyRepository repository,
    ILogger<CreateAssetsInvestmentPropertyHandler> logger) : IRequestHandler<CreateAssetsInvestmentPropertyCommand, CreateAssetsInvestmentPropertyResult>
{
    public async Task<CreateAssetsInvestmentPropertyResult> Handle(CreateAssetsInvestmentPropertyCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var property = new AssetsInvestmentProperty { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(property);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateAssetsInvestmentPropertyResult { Success = true, PropertyId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateAssetsInvestmentPropertyResult { Success = false, ErrorMessage = "An error occurred while adding the property. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateAssetsInvestmentProperty handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateAssetsInvestmentPropertyHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created investment property with ID: {PropertyId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateAssetsInvestmentPropertyHandler> logger, long PropertyId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating investment property for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateAssetsInvestmentPropertyHandler> logger, string Exception, long ScenarioId);
}
