using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.UpsertPersonalInfo;

public partial class UpsertPersonalInfoHandler(
    IOnboardingPersonalInfoRepository personalInfoRepository,
    IUserRepository userRepository,
    ILogger<UpsertPersonalInfoHandler> logger) : IRequestHandler<UpsertPersonalInfoCommand, UpsertPersonalInfoResult>
{
    public async Task<UpsertPersonalInfoResult> Handle(UpsertPersonalInfoCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsertPersonalInfoHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new UpsertPersonalInfoResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            // Create or update personal info
            var personalInfo = new OnboardingPersonalInfo
            {
                UserId = request.UserId,
                DateOfBirth = request.DateOfBirth,
                CitizenshipStatus = request.CitizenshipStatus,
                MaritalStatus = request.MaritalStatus,
                HasCurrentChildren = request.HasCurrentChildren,
                PlansFutureChildren = request.PlansFutureChildren,
                IncludePartner = request.IncludePartner,
                User = user
            };

            var result = await personalInfoRepository.Upsert(personalInfo);

            LogSuccessfullyUpsertedPersonalInfoForUserId(logger, result.Id, request.UserId);

            return new UpsertPersonalInfoResult
            {
                Success = true,
                PersonalInfoId = result.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileUpsertingPersonalInfoForUserId(logger, ex.Message, request.UserId);
            return new UpsertPersonalInfoResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your personal information. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertPersonalInfo handler for UserId: {UserId}")]
    static partial void LogStartingUpsertPersonalInfoHandlerForUserId(ILogger<UpsertPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<UpsertPersonalInfoHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted personal info with ID: {PersonalInfoId} for UserId: {UserId}")]
    static partial void LogSuccessfullyUpsertedPersonalInfoForUserId(ILogger<UpsertPersonalInfoHandler> logger, long PersonalInfoId, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while upserting personal info for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileUpsertingPersonalInfoForUserId(ILogger<UpsertPersonalInfoHandler> logger, string Exception, long UserId);
}
