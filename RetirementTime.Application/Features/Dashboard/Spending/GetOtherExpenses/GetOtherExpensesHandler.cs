using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetOtherExpenses;

public partial class GetOtherExpensesHandler(
    ISpendingRepository repository,
    ILogger<GetOtherExpensesHandler> logger) : IRequestHandler<GetOtherExpensesQuery, GetOtherExpensesResult>,
                                               IRequestHandler<CreateOtherExpenseCommand, CreateSpendingItemResult>,
                                               IRequestHandler<UpdateOtherExpenseCommand, BaseResult>,
                                               IRequestHandler<DeleteOtherExpenseCommand, BaseResult>
{
    public async Task<GetOtherExpensesResult> Handle(GetOtherExpensesQuery request, CancellationToken cancellationToken)
    {
        LogStartingGet(logger, request.ScenarioId);
        try
        {
            var expenses    = await repository.GetOtherExpensesAsync(request.ScenarioId, request.TimelineId);
            var frequencies = await repository.GetFrequenciesAsync();
            LogSuccessGet(logger, request.ScenarioId);
            return new GetOtherExpensesResult { Expenses = expenses, Frequencies = frequencies };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new GetOtherExpensesResult();
        }
    }

    public async Task<CreateSpendingItemResult> Handle(CreateOtherExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingOtherExpense
            {
                ScenarioId           = request.ScenarioId,
                RetirementTimelineId = request.TimelineId,
                Name                 = string.Empty,
                FrequencyId          = (int)FrequencyEnum.Monthly,
            };
            var created = await repository.CreateOtherExpenseAsync(item);
            return new CreateSpendingItemResult { Success = true, ItemId = created.Id };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new CreateSpendingItemResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(UpdateOtherExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingOtherExpense
            {
                Id          = request.Id,
                Name        = request.Name,
                Amount      = request.Amount,
                FrequencyId = request.FrequencyId,
            };
            await repository.UpdateOtherExpenseAsync(item);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(DeleteOtherExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteOtherExpenseAsync(request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetOtherExpenses for ScenarioId: {ScenarioId}")]
    static partial void LogStartingGet(ILogger<GetOtherExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved other expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessGet(ILogger<GetOtherExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error in other expenses handler for Id: {Id} | Exception: {Exception}")]
    static partial void LogError(ILogger<GetOtherExpensesHandler> logger, string Exception, long Id);
}
