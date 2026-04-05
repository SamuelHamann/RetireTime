using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateEmploymentIncome;

public partial class UpdateEmploymentIncomeHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<UpdateEmploymentIncomeHandler> logger) : IRequestHandler<UpdateEmploymentIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateEmploymentIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var employment = new EmploymentIncome
            {
                Id = request.Id,
                EmployerName = request.EmployerName,
                GrossSalary = request.GrossSalary,
                NetSalary = request.NetSalary,
                GrossCommissions = request.GrossCommissions,
                NetCommissions = request.NetCommissions,
                GrossBonus = request.GrossBonus,
                NetBonus = request.NetBonus
            };

            var success = await employmentIncomeRepository.UpdateAsync(employment);

            if (!success)
            {
                LogNotFound(logger, request.Id);
                return new BaseResult { Success = false, ErrorMessage = "Employment income record not found." };
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
                ErrorMessage = "An error occurred while saving employment income. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateEmploymentIncome handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Employment income not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated employment income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateEmploymentIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating employment income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateEmploymentIncomeHandler> logger, string Exception, long Id);
}
