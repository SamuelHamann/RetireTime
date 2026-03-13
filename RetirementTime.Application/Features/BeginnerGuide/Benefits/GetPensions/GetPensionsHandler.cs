using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensions;

public partial class GetPensionsHandler(
    IBeginnerGuidePensionRepository pensionRepository,
    ILogger<GetPensionsHandler> logger) : IRequestHandler<GetPensionsQuery, List<PensionDto>>
{
    public async Task<List<PensionDto>> Handle(GetPensionsQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var pensions = await pensionRepository.GetByUserIdAsync(request.UserId);

            var result = pensions.Select(p => new PensionDto
            {
                Id = p.Id,
                PensionTypeId = p.PensionTypeId,
                EmployerName = p.EmployerName
            }).ToList();

            LogQuerySuccessful(logger, request.UserId, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetPensions query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetPensionsHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} pensions for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetPensionsHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving pensions for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPensionsHandler> logger, string exception, long userId);
}

