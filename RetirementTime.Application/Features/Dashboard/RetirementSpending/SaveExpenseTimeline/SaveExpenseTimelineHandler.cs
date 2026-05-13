using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.SaveExpenseTimeline;

public partial class SaveExpenseTimelineHandler(
    IRetirementTimelineRepository timelineRepository,
    ISpendingRepository spendingRepository,
    ILogger<SaveExpenseTimelineHandler> logger) : IRequestHandler<SaveExpenseTimelineCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveExpenseTimelineCommand request, CancellationToken cancellationToken)
    {
        LogStarting(logger, request.Id, request.ScenarioId);
        try
        {
            var entity = new RetirementTimeline
            {
                Id             = request.Id,
                Name           = request.Name,
                AgeFrom        = request.AgeFrom,
                AgeTo          = request.AgeTo,
                TimelineType   = request.TimelineType,
                IsFullyCreated = true,
            };
            await timelineRepository.UpdateAsync(entity);

            if (request.IsFirstSave)
            {
                if (request.CloneFromTimelineId.HasValue)
                {
                    if (request.TimelineType == RetirementTimelineTypeEnum.Income)
                        await timelineRepository.CloneIncomeFromTimelineAsync(request.ScenarioId, request.CloneFromTimelineId.Value, request.Id);
                    else
                        await spendingRepository.CloneExpensesFromTimelineAsync(request.ScenarioId, request.CloneFromTimelineId.Value, request.Id);
                }
                else
                {
                    if (request.TimelineType == RetirementTimelineTypeEnum.Expenses)
                        await spendingRepository.CreateEmptyExpensesForTimelineAsync(request.ScenarioId, request.Id);
                    // Income: nothing to pre-create (records are created on demand per sub-page)
                }
            }

            LogSuccess(logger, request.Id, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveExpenseTimeline for Id: {Id}, ScenarioId: {ScenarioId}")]
    static partial void LogStarting(ILogger<SaveExpenseTimelineHandler> logger, long Id, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved expense timeline Id: {Id}, ScenarioId: {ScenarioId}")]
    static partial void LogSuccess(ILogger<SaveExpenseTimelineHandler> logger, long Id, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error saving expense timeline Id: {Id}, ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogError(ILogger<SaveExpenseTimelineHandler> logger, string Exception, long Id, long ScenarioId);
}
