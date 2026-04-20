using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateGroupRrsp;

public partial class UpdateGroupRrspHandler(
    IGroupRrspRepository repository,
    ILogger<UpdateGroupRrspHandler> logger) : IRequestHandler<UpdateGroupRrspCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateGroupRrspCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var plan = new GroupRrsp { Id = request.Id, Name = request.Name, PercentOfSalaryEmployee = request.PercentOfSalaryEmployee, PercentOfSalaryEmployer = request.PercentOfSalaryEmployer };
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

    [LoggerMessage(LogLevel.Information, "Starting UpdateGroupRrsp handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateGroupRrspHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Group RRSP plan not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateGroupRrspHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated Group RRSP plan with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateGroupRrspHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating Group RRSP plan with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateGroupRrspHandler> logger, string Exception, long Id);
}
