using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.SaveOasCppIncome;

public partial class SaveOasCppIncomeHandler(
    IOasCppIncomeRepository repository,
    ILogger<SaveOasCppIncomeHandler> logger) : IRequestHandler<SaveOasCppIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveOasCppIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var income = new OasCppIncome
            {
                ScenarioId = request.ScenarioId,
                IncomeLastYear = request.IncomeLastYear,
                Income2YearsAgo = request.Income2YearsAgo,
                Income3YearsAgo = request.Income3YearsAgo,
                Income4YearsAgo = request.Income4YearsAgo,
                Income5YearsAgo = request.Income5YearsAgo,
                YearsSpentInCanada = request.YearsSpentInCanada
            };

            await repository.UpsertAsync(income);

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving OAS/CPP data. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveOasCppIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveOasCppIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved OAS/CPP income for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveOasCppIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while saving OAS/CPP income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveOasCppIncomeHandler> logger, string Exception, long ScenarioId);
}
