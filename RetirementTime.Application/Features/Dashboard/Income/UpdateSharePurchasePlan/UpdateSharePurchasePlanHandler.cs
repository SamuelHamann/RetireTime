using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateSharePurchasePlan;

public partial class UpdateSharePurchasePlanHandler(
    ISharePurchasePlanRepository repository,
    ILogger<UpdateSharePurchasePlanHandler> logger) : IRequestHandler<UpdateSharePurchasePlanCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateSharePurchasePlanCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var plan = new SharePurchasePlan
            {
                Id = request.Id,
                Name = request.Name,
                PercentOfSalaryEmployee = request.PercentOfSalaryEmployee,
                PercentOfSalaryEmployer = request.PercentOfSalaryEmployer,
                UseFlatAmountInsteadOfPercent = request.UseFlatAmountInsteadOfPercent
            };
            var success = await repository.UpdateAsync(plan);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Plan record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the plan. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateSharePurchasePlan handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Share purchase plan not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated share purchase plan with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateSharePurchasePlanHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating share purchase plan with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateSharePurchasePlanHandler> logger, string Exception, long Id);
}
