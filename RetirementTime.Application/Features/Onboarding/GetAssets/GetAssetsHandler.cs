using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Onboarding.GetAssets;

public partial class GetAssetsHandler(
    IOnboardingAssetsRepository assetsRepository,
    IUserRepository userRepository,
    ILogger<GetAssetsHandler> logger) : IRequestHandler<GetAssetsQuery, GetAssetsResult>
{
    public async Task<GetAssetsResult> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetAssetsHandlerForUserId(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new GetAssetsResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            var assets = await assetsRepository.GetByUserId(request.UserId);

            if (assets == null)
            {
                LogNoAssetsFoundForUserId(logger, request.UserId);
                return new GetAssetsResult
                {
                    Success = true,
                    Assets = null
                };
            }

            var dto = new AssetsDto
            {
                Id = assets.Id,
                HasSavingsAccount = assets.HasSavingsAccount,
                HasTFSA = assets.HasTFSA,
                HasRRSP = assets.HasRRSP,
                HasRRIF = assets.HasRRIF,
                HasFHSA = assets.HasFHSA,
                HasRESP = assets.HasRESP,
                HasRDSP = assets.HasRDSP,
                HasNonRegistered = assets.HasNonRegistered,
                HasPension = assets.HasPension,
                HasPrincipalResidence = assets.HasPrincipalResidence,
                HasCar = assets.HasCar,
                HasInvestmentProperty = assets.HasInvestmentProperty,
                HasBusiness = assets.HasBusiness,
                HasIncorporation = assets.HasIncorporation,
                HasPreciousMetals = assets.HasPreciousMetals,
                HasOtherHardAssets = assets.HasOtherHardAssets
            };

            LogSuccessfullyRetrievedAssetsForUserId(logger, request.UserId);

            return new GetAssetsResult
            {
                Success = true,
                Assets = dto
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileGettingAssetsForUserId(logger, ex.Message, request.UserId);
            return new GetAssetsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading your assets information."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssets handler for UserId: {UserId}")]
    static partial void LogStartingGetAssetsHandlerForUserId(ILogger<GetAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<GetAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "No assets found for UserId: {UserId}")]
    static partial void LogNoAssetsFoundForUserId(ILogger<GetAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved assets for UserId: {UserId}")]
    static partial void LogSuccessfullyRetrievedAssetsForUserId(ILogger<GetAssetsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting assets for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileGettingAssetsForUserId(ILogger<GetAssetsHandler> logger, string Exception, long UserId);
}
