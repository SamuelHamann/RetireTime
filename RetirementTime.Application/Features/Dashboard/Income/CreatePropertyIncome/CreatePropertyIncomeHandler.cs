using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePropertyIncome;

public partial class CreatePropertyIncomeHandler(
    IPropertyIncomeRepository repository,
    ILogger<CreatePropertyIncomeHandler> logger) : IRequestHandler<CreatePropertyIncomeCommand, CreatePropertyIncomeResult>
{
    public async Task<CreatePropertyIncomeResult> Handle(CreatePropertyIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var income = new PropertyIncome
            {
                ScenarioId           = request.ScenarioId,
                RetirementTimelineId = request.TimelineId,
                InvestmentPropertyId = request.InvestmentPropertyId,
                Name                 = request.Name,
                FrequencyId          = (int)FrequencyEnum.Monthly,
            };

            var created = await repository.CreateAsync(income);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreatePropertyIncomeResult { Success = true, IncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreatePropertyIncomeResult { Success = false, ErrorMessage = "An error occurred while adding the property income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreatePropertyIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreatePropertyIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created property income with ID: {IncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreatePropertyIncomeHandler> logger, long IncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating property income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreatePropertyIncomeHandler> logger, string Exception, long ScenarioId);
}

