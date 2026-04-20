using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateOtherIncomeOrBenefits;

public partial class UpdateOtherIncomeOrBenefitsHandler(
    IOtherIncomeOrBenefitsRepository repository,
    ILogger<UpdateOtherIncomeOrBenefitsHandler> logger) : IRequestHandler<UpdateOtherIncomeOrBenefitsCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateOtherIncomeOrBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var income = new OtherIncomeOrBenefits { Id = request.Id, Name = request.Name, Amount = request.Amount };
            var success = await repository.UpdateAsync(income);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Income record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateOtherIncomeOrBenefits handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Other income/benefit not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated other income/benefit with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating other income/benefit with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateOtherIncomeOrBenefitsHandler> logger, string Exception, long Id);
}
