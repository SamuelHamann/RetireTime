using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.UpdateDebt;

public partial class UpdateDebtHandler(
    IGenericDebtRepository repository,
    ILogger<UpdateDebtHandler> logger) : IRequestHandler<UpdateDebtCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateDebtCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var debt = new GenericDebt
            {
                Id = request.Id,
                Name = request.Name,
                DebtTypeId = request.DebtTypeId,
                Balance = request.Balance,
                InterestRate = request.InterestRate,
                FrequencyId = request.FrequencyId,
                TermInYears = request.TermInYears,
                DebtAgainstAssetId = request.DebtAgainstAssetId
            };
            var success = await repository.UpdateAsync(debt);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Debt record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the debt. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateDebt handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Debt not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated debt with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateDebtHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating debt with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateDebtHandler> logger, string Exception, long Id);
}
