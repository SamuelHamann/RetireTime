using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedContribution;

public partial class UpdatePensionDefinedContributionHandler(
    IPensionDefinedContributionRepository repository,
    ILogger<UpdatePensionDefinedContributionHandler> logger) : IRequestHandler<UpdatePensionDefinedContributionCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdatePensionDefinedContributionCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var plan = new PensionDefinedContribution
            {
                Id = request.Id,
                Name = request.Name,
                PercentOfSalaryEmployee = request.PercentOfSalaryEmployee,
                PercentOfSalaryEmployer = request.PercentOfSalaryEmployer
            };

            var success = await repository.UpdateAsync(plan);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Plan record not found." };
            }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving the plan. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdatePensionDefinedContribution handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdatePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Defined contribution plan not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdatePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated defined contribution plan with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdatePensionDefinedContributionHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating defined contribution plan with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdatePensionDefinedContributionHandler> logger, string Exception, long Id);
}
