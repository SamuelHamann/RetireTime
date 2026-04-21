using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.GetAssumptions;

public partial class GetAssumptionsHandler(
    IDashboardAssumptionsRepository assumptionsRepository,
    ILogger<GetAssumptionsHandler> logger) : IRequestHandler<GetAssumptionsQuery, GetAssumptionsResult>
{
    public async Task<GetAssumptionsResult> Handle(GetAssumptionsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var assumptions = await assumptionsRepository.GetByScenarioIdAsync(request.ScenarioId);

            if (assumptions == null)
            {
                return new GetAssumptionsResult { Success = false, ErrorMessage = "Assumptions not found." };
            }

            LogSuccessfullyCompleted(logger, request.ScenarioId);

            return new GetAssumptionsResult
            {
                Success = true,
                Assumptions = new AssumptionsDto
                {
                    Id = assumptions.Id,
                    YearlyInflationRate = assumptions.YearlyInflationRate,
                    YearlyPropertyAppreciation = assumptions.YearlyPropertyAppreciation,
                    StockAllocation = assumptions.StockAllocation,
                    StockYearlyReturn = assumptions.StockYearlyReturn,
                    StockYearlyDividend = assumptions.StockYearlyDividend,
                    StockCanadianAllocation = assumptions.StockCanadianAllocation,
                    StockForeignAllocation = assumptions.StockForeignAllocation,
                    StockFees = assumptions.StockFees,
                    BondAllocation = assumptions.BondAllocation,
                    BondYearlyReturn = assumptions.BondYearlyReturn,
                    BondFees = assumptions.BondFees,
                    CashAllocation = assumptions.CashAllocation,
                    CashYearlyReturn = assumptions.CashYearlyReturn,
                }
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetAssumptionsResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssumptions for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetAssumptionsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved assumptions for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetAssumptionsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetAssumptionsHandler> logger, string Exception, long ScenarioId);
}

