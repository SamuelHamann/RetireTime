using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.UpdateRetirementSpending;

public partial class UpdateRetirementSpendingHandler(
    IRetirementSpendingRepository repository,
    ILogger<UpdateRetirementSpendingHandler> logger) : IRequestHandler<UpdateRetirementSpendingCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateRetirementSpendingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);
        try
        {
            // Validate: no overlap with other retirement spendings in the same scenario
            var siblings = await repository.GetByScenarioIdAsync(request.ScenarioId);
            var overlapping = siblings.FirstOrDefault(e =>
                e.Id != request.Id &&
                request.AgeFrom <= e.AgeTo &&
                request.AgeTo >= e.AgeFrom);

            if (overlapping is not null)
            {
                return new BaseResult
                {
                    Success      = false,
                    ErrorMessage = $"Age range {request.AgeFrom}–{request.AgeTo} overlaps with an existing retirement expense ({overlapping.AgeFrom}–{overlapping.AgeTo})."
                };
            }

            var entity = new Domain.Entities.Dashboard.Spending.RetirementSpending
            {
                Id             = request.Id,
                Name           = request.Name,
                AgeFrom        = request.AgeFrom,
                AgeTo          = request.AgeTo,
                IsFullyCreated = request.IsFullyCreated
            };
            await repository.UpdateAsync(entity);
            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateRetirementSpending for Id: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateRetirementSpendingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Updated RetirementSpending Id: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateRetirementSpendingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error updating RetirementSpending Id: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateRetirementSpendingHandler> logger, string Exception, long Id);
}

