using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.GetEmployment;

public partial class GetEmploymentHandler(
    IOnboardingEmploymentRepository employmentRepository,
    IUserRepository userRepository,
    ILogger<GetEmploymentHandler> logger) : IRequestHandler<GetEmploymentQuery, GetEmploymentResult>
{
    public async Task<GetEmploymentResult> Handle(GetEmploymentQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetEmploymentHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new GetEmploymentResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            var employment = await employmentRepository.GetByUserId(request.UserId);

            if (employment == null)
            {
                LogNoEmploymentFoundForUserId(logger, request.UserId);
                return new GetEmploymentResult
                {
                    Success = true,
                    Employment = null
                };
            }

            var dto = new EmploymentDto
            {
                Id = employment.Id,
                IsEmployed = employment.IsEmployed,
                IsSelfEmployed = employment.IsSelfEmployed,
                PlannedRetirementAge = employment.PlannedRetirementAge,
                CppContributionYears = employment.CppContributionYears,
                HasRoyalties = employment.HasRoyalties,
                HasDividends = employment.HasDividends,
                HasRentalIncome = employment.HasRentalIncome,
                HasOtherIncome = employment.HasOtherIncome
            };

            LogSuccessfullyRetrievedEmploymentForUserId(logger, request.UserId);

            return new GetEmploymentResult
            {
                Success = true,
                Employment = dto
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileGettingEmploymentForUserId(logger, ex.Message, request.UserId);
            return new GetEmploymentResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading your employment information."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetEmployment handler for UserId: {UserId}")]
    static partial void LogStartingGetEmploymentHandlerForUserId(ILogger<GetEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<GetEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "No employment found for UserId: {UserId}")]
    static partial void LogNoEmploymentFoundForUserId(ILogger<GetEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved employment for UserId: {UserId}")]
    static partial void LogSuccessfullyRetrievedEmploymentForUserId(ILogger<GetEmploymentHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting employment for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileGettingEmploymentForUserId(ILogger<GetEmploymentHandler> logger, string Exception, long UserId);
}
