using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateRealEstateIncome;

public partial class UpdateRealEstateIncomeHandler(
    IRealEstateIncomeRepository repository,
    ILogger<UpdateRealEstateIncomeHandler> logger) : IRequestHandler<UpdateRealEstateIncomeCommand, UpdateRealEstateIncomeResult>
{
    public async Task<UpdateRealEstateIncomeResult> Handle(UpdateRealEstateIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);
        try
        {
            var income = new RealEstateIncome
            {
                Id = request.Id,
                InvestmentPropertyId = request.InvestmentPropertyId,
                PropertyName = request.PropertyName,
                Amount = request.Amount,
                FrequencyId = request.FrequencyId
            };
            await repository.UpdateAsync(income);
            LogSuccessfullyCompleted(logger, request.Id);
            return new UpdateRealEstateIncomeResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new UpdateRealEstateIncomeResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateRealEstateIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateRealEstateIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated real estate income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateRealEstateIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error updating real estate income for ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateRealEstateIncomeHandler> logger, string Exception, long Id);
}

