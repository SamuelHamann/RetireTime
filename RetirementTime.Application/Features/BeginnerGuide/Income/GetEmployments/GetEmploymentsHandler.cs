using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetEmployments;

public partial class GetEmploymentsHandler(
    IEmploymentRepository repository,
    ILogger<GetEmploymentsHandler> logger) : IRequestHandler<GetEmploymentsQuery, GetEmploymentsResult>
{
    public async Task<GetEmploymentsResult> Handle(GetEmploymentsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var employments = await repository.GetByUserIdAsync(request.UserId);
            
            var employmentDtos = employments.Select(e => new EmploymentDto
            {
                Id = e.Id,
                EmployerName = e.EmployerName,
                AnnualSalary = e.AnnualSalary,
                AverageAnnualWageIncrease = e.AverageAnnualWageIncrease,
                AdditionalCompensations = e.AdditionalCompensations.Select(c => new AdditionalCompensationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Amount = c.Amount,
                    FrequencyId = c.FrequencyId
                }).ToList()
            }).ToList();
            
            LogSuccessfullyCompleted(logger, request.UserId, employmentDtos.Count);
            
            return new GetEmploymentsResult
            {
                Success = true,
                Employments = employmentDtos
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetEmploymentsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading employment data. Please try again later.",
                Employments = new List<EmploymentDto>()
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetEmployments handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<GetEmploymentsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} employments for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetEmploymentsHandler> logger, long UserId, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetEmploymentsHandler> logger, string Exception, long UserId);
}
