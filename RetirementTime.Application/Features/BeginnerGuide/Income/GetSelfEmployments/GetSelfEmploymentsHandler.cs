using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetSelfEmployments;

public partial class GetSelfEmploymentsHandler(
    ISelfEmploymentRepository repository,
    ILogger<GetSelfEmploymentsHandler> logger) : IRequestHandler<GetSelfEmploymentsQuery, GetSelfEmploymentsResult>
{
    public async Task<GetSelfEmploymentsResult> Handle(GetSelfEmploymentsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var selfEmployments = await repository.GetByUserIdAsync(request.UserId);
            
            var selfEmploymentDtos = selfEmployments.Select(e => new SelfEmploymentDto
            {
                Id = e.Id,
                BusinessName = e.BusinessName,
                AnnualSalary = e.AnnualSalary,
                AverageAnnualRevenueIncrease = e.AverageAnnualRevenueIncrease,
                MonthlyDividends = e.MonthlyDividends,
                AdditionalCompensations = e.AdditionalCompensations.Select(c => new AdditionalCompensationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Amount = c.Amount,
                    FrequencyId = c.FrequencyId
                }).ToList()
            }).ToList();
            
            LogSuccessfullyCompleted(logger, request.UserId, selfEmploymentDtos.Count);
            
            return new GetSelfEmploymentsResult
            {
                Success = true,
                SelfEmployments = selfEmploymentDtos
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetSelfEmploymentsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading self-employment data. Please try again later.",
                SelfEmployments = new List<SelfEmploymentDto>()
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetSelfEmployments handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<GetSelfEmploymentsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} self-employments for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetSelfEmploymentsHandler> logger, long UserId, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetSelfEmploymentsHandler> logger, string Exception, long UserId);
}
