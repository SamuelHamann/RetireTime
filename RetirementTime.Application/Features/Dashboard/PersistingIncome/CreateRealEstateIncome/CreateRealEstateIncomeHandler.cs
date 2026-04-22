using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateRealEstateIncome;

public partial class CreateRealEstateIncomeHandler(
    IRealEstateIncomeRepository repository,
    ILogger<CreateRealEstateIncomeHandler> logger) : IRequestHandler<CreateRealEstateIncomeCommand, CreateRealEstateIncomeResult>
{
    public async Task<CreateRealEstateIncomeResult> Handle(CreateRealEstateIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var income = new RealEstateIncome
            {
                ScenarioId = request.ScenarioId,
                InvestmentPropertyId = request.InvestmentPropertyId,
                PropertyName = request.PropertyName
            };
            var created = await repository.CreateAsync(income);
            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateRealEstateIncomeResult { Success = true, IncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateRealEstateIncomeResult { Success = false, ErrorMessage = "An error occurred while adding the income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateRealEstateIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateRealEstateIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created real estate income with ID: {IncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateRealEstateIncomeHandler> logger, long IncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating real estate income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateRealEstateIncomeHandler> logger, string Exception, long ScenarioId);
}

