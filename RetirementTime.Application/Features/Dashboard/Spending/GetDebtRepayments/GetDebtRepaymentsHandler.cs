using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Domain.Interfaces.Services;
using RetirementTime.Domain.Services;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetDebtRepayments;

public partial class GetDebtRepaymentsHandler(
    ISpendingRepository spendingRepository,
    IGenericDebtRepository debtRepository,
    IRetirementTimelineRepository timelineRepository,
    IDebtPayoffCalculationService payoffService,
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
            var repayments  = await spendingRepository.GetDebtRepaymentsAsync(request.ScenarioId, request.TimelineId);
            var debts       = await debtRepository.GetAllByScenarioIdAsync(request.ScenarioId);
            var frequencies = await spendingRepository.GetFrequenciesAsync();

            // Get all fully-created expense timelines ordered by AgeFrom to build payment segments
            var allTimelines = (await timelineRepository.GetByScenarioIdAsync(request.ScenarioId))
                .Where(t => t.IsFullyCreated && t.TimelineType == RetirementTimelineTypeEnum.Expenses)
                .ToList();
            var allRepayments = await spendingRepository.GetAllDebtRepaymentsForScenarioAsync(request.ScenarioId);

            // Build a frequency lookup (id → payments per year)
            var freqLookup = frequencies.ToDictionary(f => f.Id, f => f.FrequencyPerYear);

            // Compute yearly balance schedule for each linked debt
            var yearlyBalances = new Dictionary<long, List<DebtYearlyBalance>>();
            foreach (var debt in debts.Where(d => d.Balance is > 0))
            {
                var segments = BuildPaymentSegments(debt.Id, allTimelines, allRepayments, freqLookup);
                var schedule = payoffService.CalculateYearlyBalances(
                    debt.Balance ?? 0,
                    debt.InterestRate ?? 0,
                    segments);
                yearlyBalances[debt.Id] = schedule;
            }

            LogSuccessGet(logger, request.ScenarioId);
            return new GetDebtRepaymentsResult
            {
                Repayments              = repayments,
                Debts                   = debts,
                Frequencies             = frequencies,
                YearlyBalancesByDebtId  = yearlyBalances,
            };
        }
        catch (Exception ex)
        {
            LogError(logger, ex.Message, request.ScenarioId);
            return new GetDebtRepaymentsResult();
        }
    }

    /// <summary>
    /// Builds an ordered list of payment segments for a given debt across all timelines.
    /// Each segment spans one timeline (AgeFrom→AgeTo) and carries the annual payment
    /// amount entered for that timeline/debt combination.
    /// </summary>
    private static List<DebtPaymentSegment> BuildPaymentSegments(
        long debtId,
        List<RetirementTimeline> timelines,
        List<SpendingDebtRepayment> allRepayments,
        Dictionary<int, int> freqLookup)
    {
        var segments = new List<DebtPaymentSegment>();

        foreach (var tl in timelines.OrderBy(t => t.AgeFrom))
        {
            var repayment = allRepayments.FirstOrDefault(r =>
                r.RetirementTimelineId == tl.Id && r.GenericDebtId == debtId);

            decimal annualPayment = 0;
            if (repayment?.Amount is > 0)
            {
                var paymentsPerYear = freqLookup.GetValueOrDefault(repayment.FrequencyId, 12);
                annualPayment       = repayment.Amount.Value * paymentsPerYear;
            }

            segments.Add(new DebtPaymentSegment(tl.AgeFrom, tl.AgeTo, annualPayment));
        }

        return segments;
    }

    public async Task<CreateSpendingItemResult> Handle(CreateDebtRepaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new SpendingDebtRepayment
            {
                ScenarioId           = request.ScenarioId,
                RetirementTimelineId = request.TimelineId,
                GenericDebtId        = request.GenericDebtId,
                Name                 = string.Empty,
                FrequencyId          = (int)FrequencyEnum.Monthly,
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
