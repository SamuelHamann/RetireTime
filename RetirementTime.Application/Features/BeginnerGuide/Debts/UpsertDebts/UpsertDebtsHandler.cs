using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Debts.UpsertDebts;

public partial class UpsertDebtsHandler(
    IBeginnerGuideDebtRepository debtRepository,
    ILogger<UpsertDebtsHandler> logger) : IRequestHandler<UpsertDebtsCommand, UpsertDebtsResult>
{
    public async Task<UpsertDebtsResult> Handle(UpsertDebtsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.HasDebts, request.Debts.Count);

        try
        {
            if (!request.HasDebts)
            {
                await debtRepository.DeleteByUserIdAsync(request.UserId);
                LogDeletedAllDebts(logger, request.UserId);

                return new UpsertDebtsResult { Success = true, DebtIds = [] };
            }

            foreach (var debtDto in request.Debts)
            {
                if (debtDto.Amount <= 0)
                {
                    LogValidationFailed(logger, "Debt amount must be greater than 0");
                    return new UpsertDebtsResult { Success = false, ErrorMessage = "Debt amount must be greater than 0." };
                }

                if (debtDto.MonthlyPayment < 0)
                {
                    LogValidationFailed(logger, "Monthly payment cannot be negative");
                    return new UpsertDebtsResult { Success = false, ErrorMessage = "Monthly payment cannot be negative." };
                }

                if (debtDto.InterestRate < 0 || debtDto.InterestRate > 100)
                {
                    LogValidationFailed(logger, "Interest rate must be between 0 and 100");
                    return new UpsertDebtsResult { Success = false, ErrorMessage = "Interest rate must be between 0 and 100." };
                }
            }

            var debts = request.Debts.Select(d => new BeginnerGuideDebt
            {
                UserId = request.UserId,
                Type = d.Type,
                Amount = d.Amount,
                MonthlyPayment = d.MonthlyPayment,
                InterestRate = d.InterestRate
            }).ToList();

            var savedDebts = await debtRepository.UpsertDebtsAsync(request.UserId, debts);

            LogUpsertSuccessful(logger, request.UserId, savedDebts.Count);

            return new UpsertDebtsResult { Success = true, DebtIds = savedDebts.Select(d => d.Id).ToList() };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertDebtsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your debts. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertDebts for UserId: {UserId}, HasDebts: {HasDebts}, Count: {Count}")]
    static partial void LogStartingUpsert(ILogger logger, long userId, bool hasDebts, int count);

    [LoggerMessage(LogLevel.Information, "Deleted all debts for UserId: {UserId}")]
    static partial void LogDeletedAllDebts(ILogger logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted debts for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}

