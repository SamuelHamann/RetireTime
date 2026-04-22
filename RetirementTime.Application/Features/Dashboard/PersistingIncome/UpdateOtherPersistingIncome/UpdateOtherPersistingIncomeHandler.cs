using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateOtherPersistingIncome;

public partial class UpdateOtherPersistingIncomeHandler(
    IOtherPersistingIncomeRepository repository,
    ILogger<UpdateOtherPersistingIncomeHandler> logger) : IRequestHandler<UpdateOtherPersistingIncomeCommand, UpdateOtherPersistingIncomeResult>
{
    public async Task<UpdateOtherPersistingIncomeResult> Handle(UpdateOtherPersistingIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);
        try
        {
            var income = new OtherPersistingIncome
            {
                Id = request.Id,
                Name = request.Name,
                Amount = request.Amount,
                FrequencyId = request.FrequencyId,
                Taxable = request.Taxable
            };
            await repository.UpdateAsync(income);
            LogSuccessfullyCompleted(logger, request.Id);
            return new UpdateOtherPersistingIncomeResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new UpdateOtherPersistingIncomeResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateOtherPersistingIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateOtherPersistingIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated other persisting income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateOtherPersistingIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error updating other persisting income for ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateOtherPersistingIncomeHandler> logger, string Exception, long Id);
}

