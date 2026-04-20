using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.UpsertDebt;

public partial class UpsertDebtHandler(
    IOnboardingDebtRepository debtRepository,
    IUserRepository userRepository,
    ILogger<UpsertDebtHandler> logger) : IRequestHandler<UpsertDebtCommand, UpsertDebtResult>
{
    public async Task<UpsertDebtResult> Handle(UpsertDebtCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsertDebtHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new UpsertDebtResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            // Create or update debt
            var debt = new OnboardingDebt
            {
                UserId = request.UserId,
                HasPrimaryMortgage = request.HasPrimaryMortgage,
                HasInvestmentPropertyMortgage = request.HasInvestmentPropertyMortgage,
                HasCarPayments = request.HasCarPayments,
                HasStudentLoans = request.HasStudentLoans,
                HasCreditCardDebt = request.HasCreditCardDebt,
                HasPersonalLoans = request.HasPersonalLoans,
                HasBusinessLoans = request.HasBusinessLoans,
                HasTaxDebt = request.HasTaxDebt,
                HasMedicalDebt = request.HasMedicalDebt,
                HasInformalDebt = request.HasInformalDebt,
                User = user
            };

            var result = await debtRepository.Upsert(debt);

            LogSuccessfullyUpsertedDebtForUserId(logger, result.Id, request.UserId);

            return new UpsertDebtResult
            {
                Success = true,
                DebtId = result.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileUpsertingDebtForUserId(logger, ex.Message, request.UserId);
            return new UpsertDebtResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your debt information. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertDebt handler for UserId: {UserId}")]
    static partial void LogStartingUpsertDebtHandlerForUserId(ILogger<UpsertDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<UpsertDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted debt with ID: {DebtId} for UserId: {UserId}")]
    static partial void LogSuccessfullyUpsertedDebtForUserId(ILogger<UpsertDebtHandler> logger, long DebtId, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while upserting debt for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileUpsertingDebtForUserId(ILogger<UpsertDebtHandler> logger, string Exception, long UserId);
}
