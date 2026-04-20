using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.SaveHomeMortgage;

public partial class SaveHomeMortgageHandler(
    IGenericDebtRepository repository,
    ILogger<SaveHomeMortgageHandler> logger) : IRequestHandler<SaveHomeMortgageCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveHomeMortgageCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var debt = new GenericDebt
            {
                Name = "Primary Residence Mortgage",
                Balance = request.Balance,
                InterestRate = request.InterestRate,
                FrequencyId = request.FrequencyId,
                TermInYears = request.TermInYears,
                DebtAgainstAssetId = request.DebtAgainstAssetId
            };
            await repository.UpsertHomeMortgageAsync(request.ScenarioId, debt);

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the home mortgage. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveHomeMortgage handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveHomeMortgageHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved home mortgage for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveHomeMortgageHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while saving home mortgage for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveHomeMortgageHandler> logger, string Exception, long ScenarioId);
}
