using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.SaveAssumptions;

public partial class SaveAssumptionsHandler(
    IDashboardAssumptionsRepository assumptionsRepository,
    ILogger<SaveAssumptionsHandler> logger) : IRequestHandler<SaveAssumptionsCommand, SaveAssumptionsResult>
{
    public async Task<SaveAssumptionsResult> Handle(SaveAssumptionsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var assumptions = await assumptionsRepository.GetByScenarioIdAsync(request.ScenarioId);

            if (assumptions == null)
            {
                return new SaveAssumptionsResult { Success = false, ErrorMessage = "Assumptions not found for this scenario." };
            }

            assumptions.YearlyInflationRate = request.YearlyInflationRate;
            assumptions.YearlyPropertyAppreciation = request.YearlyPropertyAppreciation;
            assumptions.YearlyHouseMaintenance = request.YearlyHouseMaintenance;
            assumptions.AnnualSalaryRaise = request.AnnualSalaryRaise;
            assumptions.LifeExpectancy = request.LifeExpectancy;
            assumptions.StockAllocation = request.StockAllocation;
            assumptions.StockYearlyReturn = request.StockYearlyReturn;
            assumptions.StockYearlyDividend = request.StockYearlyDividend;
            assumptions.StockCanadianAllocation = request.StockCanadianAllocation;
            assumptions.StockForeignAllocation = request.StockForeignAllocation;
            assumptions.StockFees = request.StockFees;
            assumptions.BondAllocation = request.BondAllocation;
            assumptions.BondYearlyReturn = request.BondYearlyReturn;
            assumptions.BondFees = request.BondFees;
            assumptions.CashAllocation = request.CashAllocation;
            assumptions.CashYearlyReturn = request.CashYearlyReturn;

            var updated = await assumptionsRepository.UpdateAsync(assumptions);

            if (!updated)
            {
                return new SaveAssumptionsResult { Success = false, ErrorMessage = "Failed to save assumptions." };
            }

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new SaveAssumptionsResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new SaveAssumptionsResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveAssumptions for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveAssumptionsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved assumptions for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveAssumptionsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error saving assumptions for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveAssumptionsHandler> logger, string Exception, long ScenarioId);
}

