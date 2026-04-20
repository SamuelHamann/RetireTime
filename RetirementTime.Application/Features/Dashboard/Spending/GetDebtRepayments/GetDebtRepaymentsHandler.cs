using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetDebtRepayments;

public partial class GetDebtRepaymentsHandler(
    ISpendingRepository spendingRepository,
    IGenericDebtRepository debtRepository,
    ILogger<GetDebtRepaymentsHandler> logger) : IRequestHandler<GetDebtRepaymentsQuery, GetDebtRepaymentsResult>,
                                                IRequestHandler<CreateDebtRepaymentCommand, CreateSpendingItemResult>,
                                                IRequestHandler<UpdateDebtRepaymentCommand, BaseResult>,
                                                IRequestHandler<DeleteDebtRepaymentCommand, BaseResult>
{
    public async Task<GetDebtRepaymentsResult> Handle(GetDebtRepaymentsQuery request, CancellationToken cancellationToken)
    {
        LogStartingGet(logger, request.ScenarioId);
        try
        {
            var repayments  = await spendingRepository.GetDebtRepaymentsAsync(request.ScenarioId);
            var debts       = await debtRepository.GetAllByScenarioIdAsync(request.ScenarioId);
            var frequencies = await spendingRepository.GetFrequenciesAsync();
            LogSuccessGet(logger, request.ScenarioId);
            return new GetDebtRepaymentsResult { Repayments = repayments, Debts = debts, Frequencies = frequencies };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new GetDebtRepaymentsResult();
        }
    }

    public async Task<CreateSpendingItemResult> Handle(CreateDebtRepaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingDebtRepayment
            {
                ScenarioId   = request.ScenarioId,
                GenericDebtId = request.GenericDebtId,
                Name         = string.Empty,
                FrequencyId  = (int)FrequencyEnum.Monthly,
            };
            var created = await spendingRepository.CreateDebtRepaymentAsync(item);
            return new CreateSpendingItemResult { Success = true, ItemId = created.Id };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new CreateSpendingItemResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(UpdateDebtRepaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingDebtRepayment
            {
                Id           = request.Id,
                Name         = request.Name,
                Amount       = request.Amount,
                FrequencyId  = request.FrequencyId,
                GenericDebtId = request.GenericDebtId,
            };
            await spendingRepository.UpdateDebtRepaymentAsync(item);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(DeleteDebtRepaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await spendingRepository.DeleteDebtRepaymentAsync(request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetDebtRepayments for ScenarioId: {ScenarioId}")]
    static partial void LogStartingGet(ILogger<GetDebtRepaymentsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved debt repayments for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessGet(ILogger<GetDebtRepaymentsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error in debt repayments handler for Id: {Id} | Exception: {Exception}")]
    static partial void LogError(ILogger<GetDebtRepaymentsHandler> logger, string Exception, long Id);
}
