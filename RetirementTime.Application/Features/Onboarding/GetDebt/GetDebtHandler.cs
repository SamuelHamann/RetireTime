using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.GetDebt;

public partial class GetDebtHandler(
    IOnboardingDebtRepository debtRepository,
    IUserRepository userRepository,
    ILogger<GetDebtHandler> logger) : IRequestHandler<GetDebtQuery, GetDebtResult>
{
    public async Task<GetDebtResult> Handle(GetDebtQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetDebtHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new GetDebtResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            var debt = await debtRepository.GetByUserId(request.UserId);

            if (debt == null)
            {
                LogNoDebtFoundForUserId(logger, request.UserId);
                return new GetDebtResult
                {
                    Success = true,
                    Debt = null
                };
            }

            var dto = new DebtDto
            {
                Id = debt.Id,
                HasPrimaryMortgage = debt.HasPrimaryMortgage,
                HasInvestmentPropertyMortgage = debt.HasInvestmentPropertyMortgage,
                HasCarPayments = debt.HasCarPayments,
                HasStudentLoans = debt.HasStudentLoans,
                HasCreditCardDebt = debt.HasCreditCardDebt,
                HasPersonalLoans = debt.HasPersonalLoans,
                HasBusinessLoans = debt.HasBusinessLoans,
                HasTaxDebt = debt.HasTaxDebt,
                HasMedicalDebt = debt.HasMedicalDebt,
                HasInformalDebt = debt.HasInformalDebt
            };

            LogSuccessfullyRetrievedDebtForUserId(logger, request.UserId);

            return new GetDebtResult
            {
                Success = true,
                Debt = dto
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileGettingDebtForUserId(logger, ex.Message, request.UserId);
            return new GetDebtResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading your debt information."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetDebt handler for UserId: {UserId}")]
    static partial void LogStartingGetDebtHandlerForUserId(ILogger<GetDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<GetDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "No debt found for UserId: {UserId}")]
    static partial void LogNoDebtFoundForUserId(ILogger<GetDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved debt for UserId: {UserId}")]
    static partial void LogSuccessfullyRetrievedDebtForUserId(ILogger<GetDebtHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting debt for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileGettingDebtForUserId(ILogger<GetDebtHandler> logger, string Exception, long UserId);
}
