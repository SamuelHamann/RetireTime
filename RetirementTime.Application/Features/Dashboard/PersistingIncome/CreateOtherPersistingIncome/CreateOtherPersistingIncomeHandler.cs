using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateOtherPersistingIncome;

public partial class CreateOtherPersistingIncomeHandler(
    IOtherPersistingIncomeRepository repository,
    ILogger<CreateOtherPersistingIncomeHandler> logger) : IRequestHandler<CreateOtherPersistingIncomeCommand, CreateOtherPersistingIncomeResult>
{
    public async Task<CreateOtherPersistingIncomeResult> Handle(CreateOtherPersistingIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var income = new OtherPersistingIncome { ScenarioId = request.ScenarioId };
            var created = await repository.CreateAsync(income);
            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateOtherPersistingIncomeResult { Success = true, IncomeId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateOtherPersistingIncomeResult { Success = false, ErrorMessage = "An error occurred while adding the income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateOtherPersistingIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateOtherPersistingIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created other persisting income with ID: {IncomeId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateOtherPersistingIncomeHandler> logger, long IncomeId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error creating other persisting income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateOtherPersistingIncomeHandler> logger, string Exception, long ScenarioId);
}

