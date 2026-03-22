using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.GetPersonalInfo;

public partial class GetPersonalInfoHandler(
    IOnboardingPersonalInfoRepository personalInfoRepository,
    IUserRepository userRepository,
    ILogger<GetPersonalInfoHandler> logger) : IRequestHandler<GetPersonalInfoQuery, GetPersonalInfoResult>
{
    public async Task<GetPersonalInfoResult> Handle(GetPersonalInfoQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetPersonalInfoHandlerForUserId(logger, request.UserId);

        try
        {
            // Get user info first (always needed for Email, FirstName, LastName)
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new GetPersonalInfoResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            var personalInfo = await personalInfoRepository.GetByUserId(request.UserId);

            if (personalInfo == null)
            {
                LogNoPersonalInfoFoundForUserId(logger, request.UserId);
                return new GetPersonalInfoResult
                {
                    Success = true,
                    PersonalInfo = null
                };
            }

            var dto = new PersonalInfoDto
            {
                Id = personalInfo.Id,
                // From User table
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                // From OnboardingPersonalInfo table
                DateOfBirth = personalInfo.DateOfBirth,
                CitizenshipStatus = personalInfo.CitizenshipStatus,
                MaritalStatus = personalInfo.MaritalStatus,
                HasCurrentChildren = personalInfo.HasCurrentChildren,
                PlansFutureChildren = personalInfo.PlansFutureChildren,
                IncludePartner = personalInfo.IncludePartner
            };

            LogSuccessfullyRetrievedPersonalInfoForUserId(logger, request.UserId);

            return new GetPersonalInfoResult
            {
                Success = true,
                PersonalInfo = dto
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileGettingPersonalInfoForUserId(logger, ex.Message, request.UserId);
            return new GetPersonalInfoResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading your personal information."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetPersonalInfo handler for UserId: {UserId}")]
    static partial void LogStartingGetPersonalInfoHandlerForUserId(ILogger<GetPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<GetPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "No personal info found for UserId: {UserId}")]
    static partial void LogNoPersonalInfoFoundForUserId(ILogger<GetPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved personal info for UserId: {UserId}")]
    static partial void LogSuccessfullyRetrievedPersonalInfoForUserId(ILogger<GetPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting personal info for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileGettingPersonalInfoForUserId(ILogger<GetPersonalInfoHandler> logger, string Exception, long UserId);
}
