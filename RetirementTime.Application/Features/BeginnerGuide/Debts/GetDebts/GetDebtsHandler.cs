using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Debts.GetDebts;

public partial class GetDebtsHandler(
    IBeginnerGuideDebtRepository debtRepository,
    ILogger<GetDebtsHandler> logger) : IRequestHandler<GetDebtsQuery, List<DebtDto>>
{
    public async Task<List<DebtDto>> Handle(GetDebtsQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var debts = await debtRepository.GetByUserIdAsync(request.UserId);

            var result = debts.Select(d => new DebtDto
            {
                Id = d.Id,
                Type = d.Type,
                Amount = d.Amount,
                MonthlyPayment = d.MonthlyPayment,
                InterestRate = d.InterestRate
            }).ToList();

            LogQuerySuccessful(logger, request.UserId, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetDebts query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetDebtsHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} debts for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetDebtsHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving debts for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetDebtsHandler> logger, string exception, long userId);
}

