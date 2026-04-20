using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedBenefits;

public partial class UpdatePensionDefinedBenefitsHandler(
    IPensionDefinedBenefitsRepository repository,
    ILogger<UpdatePensionDefinedBenefitsHandler> logger) : IRequestHandler<UpdatePensionDefinedBenefitsCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdatePensionDefinedBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var pension = new PensionDefinedBenefits
            {
                Id = request.Id,
                Name = request.Name,
                StartAge = request.StartAge,
                BenefitsPre65 = request.BenefitsPre65,
                BenefitsPost65 = request.BenefitsPost65,
                PercentIndexedToInflation = request.PercentIndexedToInflation,
                PercentSurvivorBenefits = request.PercentSurvivorBenefits,
                RrspAdjustment = request.RrspAdjustment
            };

            var success = await repository.UpdateAsync(pension);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Pension record not found." };
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
                ErrorMessage = "An error occurred while saving the pension. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdatePensionDefinedBenefits handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdatePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Defined benefits pension not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdatePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated defined benefits pension with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdatePensionDefinedBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating defined benefits pension with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdatePensionDefinedBenefitsHandler> logger, string Exception, long Id);
}
