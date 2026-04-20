using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncomeOrBenefits;

public partial class DeleteOtherIncomeOrBenefitsHandler(
    IOtherIncomeOrBenefitsRepository repository,
    ILogger<DeleteOtherIncomeOrBenefitsHandler> logger) : IRequestHandler<DeleteOtherIncomeOrBenefitsCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteOtherIncomeOrBenefitsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Income record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteOtherIncomeOrBenefits handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Other income/benefit not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted other income/benefit with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteOtherIncomeOrBenefitsHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting other income/benefit with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteOtherIncomeOrBenefitsHandler> logger, string Exception, long Id);
}
