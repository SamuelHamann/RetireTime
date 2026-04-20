using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.UpsertEmployment;

public partial class UpsertEmploymentHandler(
    IOnboardingEmploymentRepository employmentRepository,
    IUserRepository userRepository,
    ILogger<UpsertEmploymentHandler> logger) : IRequestHandler<UpsertEmploymentCommand, UpsertEmploymentResult>
{
    public async Task<UpsertEmploymentResult> Handle(UpsertEmploymentCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsertEmploymentHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new UpsertEmploymentResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            // Create or update employment
            var employment = new OnboardingEmployment
            {
                UserId = request.UserId,
                IsEmployed = request.IsEmployed,
                IsSelfEmployed = request.IsSelfEmployed,
                PlannedRetirementAge = request.PlannedRetirementAge,
                CppContributionYears = request.CppContributionYears,
                HasRoyalties = request.HasRoyalties,
                HasDividends = request.HasDividends,
                HasRentalIncome = request.HasRentalIncome,
                HasOtherIncome = request.HasOtherIncome,
                User = user
            };

            var result = await employmentRepository.Upsert(employment);

            LogSuccessfullyUpsertedEmploymentForUserId(logger, result.Id, request.UserId);

            return new UpsertEmploymentResult
            {
                Success = true,
                EmploymentId = result.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileUpsertingEmploymentForUserId(logger, ex.Message, request.UserId);
            return new UpsertEmploymentResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your employment information. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertEmployment handler for UserId: {UserId}")]
    static partial void LogStartingUpsertEmploymentHandlerForUserId(ILogger<UpsertEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<UpsertEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted employment with ID: {EmploymentId} for UserId: {UserId}")]
    static partial void LogSuccessfullyUpsertedEmploymentForUserId(ILogger<UpsertEmploymentHandler> logger, long EmploymentId, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while upserting employment for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileUpsertingEmploymentForUserId(ILogger<UpsertEmploymentHandler> logger, string Exception, long UserId);
}
