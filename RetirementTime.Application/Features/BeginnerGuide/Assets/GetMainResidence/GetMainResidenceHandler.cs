using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetMainResidence;

public partial class GetMainResidenceHandler(
    IMainResidenceRepository mainResidenceRepository,
    ILogger<GetMainResidenceHandler> logger) : IRequestHandler<GetMainResidenceQuery, GetMainResidenceResult>
{
    public async Task<GetMainResidenceResult> Handle(GetMainResidenceQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetMainResidenceHandler(logger, request.UserId);

        try
        {
            var mainResidence = await mainResidenceRepository.GetByUserIdAsync(request.UserId);

            if (mainResidence == null)
            {
                LogMainResidenceNotFound(logger, request.UserId);
                return new GetMainResidenceResult
                {
                    MainResidence = null
                };
            }

            LogSuccessfullyRetrievedMainResidence(logger, mainResidence.Id, request.UserId);

            return new GetMainResidenceResult
            {
                MainResidence = mainResidence
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            // Return null on error to avoid breaking the UI
            return new GetMainResidenceResult
            {
                MainResidence = null
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetMainResidence handler for UserId: {userId}")]
    static partial void LogStartingGetMainResidenceHandler(ILogger<GetMainResidenceHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "MainResidence not found for UserId: {userId}")]
    static partial void LogMainResidenceNotFound(ILogger<GetMainResidenceHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved MainResidence with ID: {mainResidenceId} for UserId: {userId}")]
    static partial void LogSuccessfullyRetrievedMainResidence(ILogger<GetMainResidenceHandler> logger, long mainResidenceId, long userId);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {userId} | Exception: {exception}")]
    static partial void LogErrorOccurred(ILogger<GetMainResidenceHandler> logger, string exception, long userId);
}

