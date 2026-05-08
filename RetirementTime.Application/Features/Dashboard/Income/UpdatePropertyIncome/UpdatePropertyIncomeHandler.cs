using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdatePropertyIncome;

public partial class UpdatePropertyIncomeHandler(
    IPropertyIncomeRepository repository,
    ILogger<UpdatePropertyIncomeHandler> logger) : IRequestHandler<UpdatePropertyIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdatePropertyIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var income = new PropertyIncome
            {
                Id          = request.Id,
                Name        = request.Name,
                Amount      = request.Amount,
                FrequencyId = request.FrequencyId,
            };

            var success = await repository.UpdateAsync(income);

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = success };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while updating the property income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdatePropertyIncome handler for Id: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdatePropertyIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated property income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdatePropertyIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating property income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdatePropertyIncomeHandler> logger, string Exception, long Id);
}

