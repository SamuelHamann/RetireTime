using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.UpsertAssets;

public partial class UpsertAssetsHandler(
    IOnboardingAssetsRepository assetsRepository,
    IUserRepository userRepository,
    ILogger<UpsertAssetsHandler> logger) : IRequestHandler<UpsertAssetsCommand, UpsertAssetsResult>
{
    public async Task<UpsertAssetsResult> Handle(UpsertAssetsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsertAssetsHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new UpsertAssetsResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            // Create or update assets
            var assets = new OnboardingAssets
            {
                UserId = request.UserId,
                HasSavingsAccount = request.HasSavingsAccount,
                HasTFSA = request.HasTFSA,
                HasRRSP = request.HasRRSP,
                HasRRIF = request.HasRRIF,
                HasFHSA = request.HasFHSA,
                HasRESP = request.HasRESP,
                HasRDSP = request.HasRDSP,
                HasNonRegistered = request.HasNonRegistered,
                HasPension = request.HasPension,
                HasPrincipalResidence = request.HasPrincipalResidence,
                HasCar = request.HasCar,
                HasInvestmentProperty = request.HasInvestmentProperty,
                HasBusiness = request.HasBusiness,
                HasIncorporation = request.HasIncorporation,
                HasPreciousMetals = request.HasPreciousMetals,
                HasOtherHardAssets = request.HasOtherHardAssets,
                User = user
            };

            var result = await assetsRepository.Upsert(assets);

            LogSuccessfullyUpsertedAssetsForUserId(logger, result.Id, request.UserId);

            return new UpsertAssetsResult
            {
                Success = true,
                AssetsId = result.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileUpsertingAssetsForUserId(logger, ex.Message, request.UserId);
            return new UpsertAssetsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your assets information. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertAssets handler for UserId: {UserId}")]
    static partial void LogStartingUpsertAssetsHandlerForUserId(ILogger<UpsertAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<UpsertAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted assets with ID: {AssetsId} for UserId: {UserId}")]
    static partial void LogSuccessfullyUpsertedAssetsForUserId(ILogger<UpsertAssetsHandler> logger, long AssetsId, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while upserting assets for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileUpsertingAssetsForUserId(ILogger<UpsertAssetsHandler> logger, string Exception, long UserId);
}
