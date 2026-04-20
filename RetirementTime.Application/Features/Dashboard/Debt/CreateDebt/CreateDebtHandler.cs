using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.CreateDebt;

public partial class CreateDebtHandler(
    IGenericDebtRepository repository,
    ILogger<CreateDebtHandler> logger) : IRequestHandler<CreateDebtCommand, CreateDebtResult>
{
    public async Task<CreateDebtResult> Handle(CreateDebtCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var debt = new GenericDebt
            {
                ScenarioId = request.ScenarioId,
                DebtTypeId = request.DebtTypeId,
                Name = string.Empty,
                FrequencyId = (int)FrequencyEnum.Annually,
                DebtAgainstAssetId = request.DebtAgainstAssetId
            };
            var created = await repository.CreateAsync(debt);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateDebtResult { Success = true, DebtId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateDebtResult { Success = false, ErrorMessage = "An error occurred while adding the debt. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateDebt handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateDebtHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created debt with ID: {DebtId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateDebtHandler> logger, long DebtId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating debt for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateDebtHandler> logger, string Exception, long ScenarioId);
}
