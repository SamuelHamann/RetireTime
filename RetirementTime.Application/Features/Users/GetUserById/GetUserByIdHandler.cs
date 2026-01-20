using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.GetUserById;

public partial class GetUserByIdHandler(
    IUserRepository userRepository,
    ILogger<GetUserByIdHandler> logger) : IRequestHandler<GetUserByIdQuery, GetUserByIdResult?>
{
    public async Task<GetUserByIdResult?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetUserByIdHandler(logger, request.UserId);

        try
        {
            var user = await userRepository.GetUserById(request.UserId);

            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return null;
            }

            LogSuccessfullyRetrievedUser(logger, user.Id);

            return new GetUserByIdResult
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LanguageCode = user.LanguageCode
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileRetrievingUser(logger, ex.Message, request.UserId);
            return null;
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetUserById handler for UserId: {userId}")]
    static partial void LogStartingGetUserByIdHandler(ILogger<GetUserByIdHandler> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "User not found for UserId: {userId}")]
    static partial void LogUserNotFound(ILogger<GetUserByIdHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved user with Id: {userId}")]
    static partial void LogSuccessfullyRetrievedUser(ILogger<GetUserByIdHandler> logger, long userId);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving user for UserId: {userId} | Exception: {exception}")]
    static partial void LogErrorOccurredWhileRetrievingUser(ILogger<GetUserByIdHandler> logger, string exception, long userId);
}

