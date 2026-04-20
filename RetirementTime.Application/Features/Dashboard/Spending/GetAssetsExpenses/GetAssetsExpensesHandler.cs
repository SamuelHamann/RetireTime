using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetAssetsExpenses;

public partial class GetAssetsExpensesHandler(
    ISpendingRepository spendingRepository,
    IAssetsHomeRepository homeRepository,
    IAssetsInvestmentPropertyRepository investmentPropertyRepository,
    IAssetsInvestmentAccountRepository investmentAccountRepository,
    IAssetsPhysicalAssetRepository physicalAssetRepository,
    ILogger<GetAssetsExpensesHandler> logger) : IRequestHandler<GetAssetsExpensesQuery, GetAssetsExpensesResult>,
                                                IRequestHandler<CreateAssetsExpenseCommand, CreateSpendingItemResult>,
                                                IRequestHandler<UpdateAssetsExpenseCommand, BaseResult>,
                                                IRequestHandler<DeleteAssetsExpenseCommand, BaseResult>
{
    public async Task<GetAssetsExpensesResult> Handle(GetAssetsExpensesQuery request, CancellationToken cancellationToken)
    {
        LogStartingGet(logger, request.ScenarioId);
        try
        {
            var expenses            = await spendingRepository.GetAssetsExpensesAsync(request.ScenarioId);
            var home                = await homeRepository.GetByScenarioIdAsync(request.ScenarioId);
            var investmentProps     = await investmentPropertyRepository.GetByScenarioIdAsync(request.ScenarioId);
            var investmentAccounts  = await investmentAccountRepository.GetByScenarioIdAsync(request.ScenarioId);
            var physicalAssets      = await physicalAssetRepository.GetByScenarioIdAsync(request.ScenarioId);
            var frequencies         = await spendingRepository.GetFrequenciesAsync();

            LogSuccessGet(logger, request.ScenarioId);
            return new GetAssetsExpensesResult
            {
                Expenses            = expenses,
                Home                = home,
                InvestmentProperties = investmentProps,
                InvestmentAccounts  = investmentAccounts,
                PhysicalAssets      = physicalAssets,
                Frequencies         = frequencies,
            };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new GetAssetsExpensesResult();
        }
    }

    public async Task<CreateSpendingItemResult> Handle(CreateAssetsExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingAssetsExpense
            {
                ScenarioId                  = request.ScenarioId,
                Name                        = string.Empty,
                FrequencyId                 = (int)FrequencyEnum.Monthly,
                AssetsHomeId                = request.AssetsHomeId,
                AssetsInvestmentPropertyId  = request.AssetsInvestmentPropertyId,
                AssetsInvestmentAccountId   = request.AssetsInvestmentAccountId,
                AssetsPhysicalAssetId       = request.AssetsPhysicalAssetId,
            };
            var created = await spendingRepository.CreateAssetsExpenseAsync(item);
            return new CreateSpendingItemResult { Success = true, ItemId = created.Id };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new CreateSpendingItemResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(UpdateAssetsExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingAssetsExpense
            {
                Id                          = request.Id,
                Name                        = request.Name,
                Expense                     = request.Expense,
                FrequencyId                 = request.FrequencyId,
                AssetsHomeId                = request.AssetsHomeId,
                AssetsInvestmentPropertyId  = request.AssetsInvestmentPropertyId,
                AssetsInvestmentAccountId   = request.AssetsInvestmentAccountId,
                AssetsPhysicalAssetId       = request.AssetsPhysicalAssetId,
            };
            await spendingRepository.UpdateAssetsExpenseAsync(item);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    public async Task<BaseResult> Handle(DeleteAssetsExpenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await spendingRepository.DeleteAssetsExpenseAsync(request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssetsExpenses for ScenarioId: {ScenarioId}")]
    static partial void LogStartingGet(ILogger<GetAssetsExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved assets expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessGet(ILogger<GetAssetsExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error in assets expenses handler for Id: {Id} | Exception: {Exception}")]
    static partial void LogError(ILogger<GetAssetsExpensesHandler> logger, string Exception, long Id);
}
