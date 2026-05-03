using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Application.Features.Dashboard.Spending.GetOtherExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetInvestmentExpenses;

public partial class GetInvestmentExpensesHandler(
    ISpendingRepository repository,
    IAssetsInvestmentAccountRepository investmentAccountRepository,
    ILogger<GetInvestmentExpensesHandler> logger)
    : IRequestHandler<GetInvestmentExpensesQuery, GetInvestmentExpensesResult>,
      IRequestHandler<CreateInvestmentExpenseCommand, CreateSpendingItemResult>,
      IRequestHandler<UpdateInvestmentExpenseCommand, BaseResult>,
      IRequestHandler<DeleteInvestmentExpenseCommand, BaseResult>
{
    public async Task<GetInvestmentExpensesResult> Handle(GetInvestmentExpensesQuery request, CancellationToken cancellationToken)
    {
        LogStartingGet(logger, request.ScenarioId);
        try
        {
            var expenses  = await repository.GetInvestmentExpensesAsync(request.ScenarioId, request.TimelineId);
            var freqs     = await repository.GetFrequenciesAsync();
            var accounts  = await investmentAccountRepository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessGet(logger, request.ScenarioId);
            return new GetInvestmentExpensesResult { Expenses = expenses, Frequencies = freqs, InvestmentAccounts = accounts };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new GetInvestmentExpensesResult();
        }
    }

    public async Task<CreateSpendingItemResult> Handle(CreateInvestmentExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingInvestmentExpense
            {
                ScenarioId           = request.ScenarioId,
                RetirementTimelineId = request.TimelineId,
                FrequencyId          = (int)FrequencyEnum.Monthly,
            };
            var created = await repository.CreateInvestmentExpenseAsync(item);
            return new CreateSpendingItemResult { Success = true, ItemId = created.Id };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new CreateSpendingItemResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(UpdateInvestmentExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingInvestmentExpense
            {
                Id                   = request.Id,
                Amount               = request.Amount,
                FrequencyId          = request.FrequencyId,
                InvestmentAccountId  = request.InvestmentAccountId,
                RetirementTimelineId = request.RetirementTimelineId,
            };
            await repository.UpdateInvestmentExpenseAsync(item);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(DeleteInvestmentExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteInvestmentExpenseAsync(request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetInvestmentExpenses for ScenarioId: {ScenarioId}")]
    static partial void LogStartingGet(ILogger<GetInvestmentExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved investment expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessGet(ILogger<GetInvestmentExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error in investment expenses handler for Id: {Id} | Exception: {Exception}")]
    static partial void LogError(ILogger<GetInvestmentExpensesHandler> logger, string Exception, long Id);
}

