using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.SaveLivingExpenses;

public partial class SaveLivingExpensesHandler(
    ISpendingRepository repository,
    ILogger<SaveLivingExpensesHandler> logger) : IRequestHandler<SaveLivingExpensesCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveLivingExpensesCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var entity = new SpendingLivingExpenses
            {
                ScenarioId                     = request.ScenarioId,
                RetirementTimelineId           = request.TimelineId,
                RentOrMortgage                 = request.RentOrMortgage,
                RentOrMortgageFrequencyId      = request.RentOrMortgageFrequencyId,
                Food                           = request.Food,
                FoodFrequencyId                = request.FoodFrequencyId,
                Utilities                      = request.Utilities,
                UtilitiesFrequencyId           = request.UtilitiesFrequencyId,
                Insurance                      = request.Insurance,
                InsuranceFrequencyId           = request.InsuranceFrequencyId,
                Gas                            = request.Gas,
                GasFrequencyId                 = request.GasFrequencyId,
                HomeMaintenance                = request.HomeMaintenance,
                HomeMaintenanceFrequencyId     = request.HomeMaintenanceFrequencyId,
                PropertyTax                    = request.PropertyTax,
                PropertyTaxFrequencyId         = request.PropertyTaxFrequencyId,
                Cellphone                      = request.Cellphone,
                CellphoneFrequencyId           = request.CellphoneFrequencyId,
                HealthSpendings                = request.HealthSpendings,
                HealthSpendingsFrequencyId     = request.HealthSpendingsFrequencyId,
                OtherLivingExpenses            = request.OtherLivingExpenses,
                OtherLivingExpensesFrequencyId = request.OtherLivingExpensesFrequencyId,
            };

            await repository.UpsertLivingExpensesAsync(entity);

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveLivingExpenses handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveLivingExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved living expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveLivingExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error saving living expenses for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveLivingExpensesHandler> logger, string Exception, long ScenarioId);
}
