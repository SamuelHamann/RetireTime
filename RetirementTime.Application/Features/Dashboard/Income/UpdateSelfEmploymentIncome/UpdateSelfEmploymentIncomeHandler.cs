using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateSelfEmploymentIncome;

public partial class UpdateSelfEmploymentIncomeHandler(
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    ILogger<UpdateSelfEmploymentIncomeHandler> logger) : IRequestHandler<UpdateSelfEmploymentIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateSelfEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var selfEmployment = new SelfEmploymentIncome
            {
                Id = request.Id,
                Name = request.Name,
                GrossSalary = request.GrossSalary,
                GrossSalaryFrequencyId = request.GrossSalaryFrequencyId,
                NetSalary = request.NetSalary,
                NetSalaryFrequencyId = request.NetSalaryFrequencyId,
                GrossDividends = request.GrossDividends,
                GrossDividendsFrequencyId = request.GrossDividendsFrequencyId,
                NetDividends = request.NetDividends,
                NetDividendsFrequencyId = request.NetDividendsFrequencyId
            };

            var success = await selfEmploymentIncomeRepository.UpdateAsync(selfEmployment);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Self-employment income record not found." };
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
                ErrorMessage = "An error occurred while saving self-employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateSelfEmploymentIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Self-employment income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated self-employment income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateSelfEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating self-employment income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateSelfEmploymentIncomeHandler> logger, string Exception, long Id);
}
