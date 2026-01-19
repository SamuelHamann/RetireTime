using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Features.Users.DTOs;

namespace RetirementTime.Application.Features.Users.GetUsers;

public partial class GetUsersHandler(ILogger<GetUsersHandler> logger) 
    : IRequestHandler<GetUsersQuery, List<GetUserDTO>>
{
    public async Task<List<GetUserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetUsersHandler(logger);

        try
        {
            // TODO: Implement GetUsers logic
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileRetrievingUsers(logger, ex.Message);
            return new List<GetUserDTO>();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetUsers handler")]
    static partial void LogStartingGetUsersHandler(ILogger<GetUsersHandler> logger);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving users | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileRetrievingUsers(ILogger<GetUsersHandler> logger, string Exception);
}