using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.CreateRetirementSpending;

public partial class CreateRetirementSpendingHandler(
    IRetirementTimelineRepository repository,
    IDashboardScenarioRepository scenarioRepository,
    IOnboardingEmploymentRepository employmentRepository,
    IOnboardingPersonalInfoRepository personalInfoRepository,
    IDashboardAssumptionsRepository assumptionsRepository,
    ILogger<CreateRetirementSpendingHandler> logger) : IRequestHandler<CreateRetirementSpendingCommand, CreateRetirementSpendingResult>
{
    private const int FallbackLifeExpectancy = 90;

    public async Task<CreateRetirementSpendingResult> Handle(CreateRetirementSpendingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);

            var existing = await repository.GetByScenarioIdAsync(request.ScenarioId);
            var existingOfType = existing.Where(e => e.TimelineType == request.TimelineType).ToList();

            int ageFrom, ageTo;

            if (request.TimelineType == RetirementTimelineTypeEnum.Expenses && existingOfType.Count == 0)
            {
                // First expense timeline: current age → life expectancy from assumptions
                ageFrom = 0;
                ageTo   = FallbackLifeExpectancy;

                if (scenario is not null)
                {
                    var assumptions = await assumptionsRepository.GetByScenarioIdAsync(request.ScenarioId);
                    if (assumptions is not null && assumptions.LifeExpectancy > 0)
                        ageTo = assumptions.LifeExpectancy;

                    var personalInfo = await personalInfoRepository.GetByUserId(scenario.UserId);
                    if (personalInfo is not null)
                    {
                        var today = DateOnly.FromDateTime(DateTime.UtcNow);
                        var dob   = personalInfo.DateOfBirth;
                        var age   = today.Year - dob.Year - (today < dob.AddYears(today.Year - dob.Year) ? 1 : 0);
                        ageFrom   = Math.Max(0, age);
                    }
                }
            }
            else if (request.TimelineType == RetirementTimelineTypeEnum.Income && existingOfType.Count == 0)
            {
                // First income timeline: current age → planned retirement age
                ageFrom = 0;
                ageTo   = 65;

                if (scenario is not null)
                {
                    var personalInfo = await personalInfoRepository.GetByUserId(scenario.UserId);
                    if (personalInfo is not null)
                    {
                        var today = DateOnly.FromDateTime(DateTime.UtcNow);
                        var dob   = personalInfo.DateOfBirth;
                        var age   = today.Year - dob.Year - (today < dob.AddYears(today.Year - dob.Year) ? 1 : 0);
                        ageFrom   = Math.Max(0, age);
                    }

                    var employment = await employmentRepository.GetByUserId(scenario.UserId);
                    if (employment?.PlannedRetirementAge is > 0)
                        ageTo = employment.PlannedRetirementAge.Value;
                }
            }
            else
            {
                // Subsequent timelines: chain from last + 10-year span
                int defaultAge = existingOfType.Count > 0 ? existingOfType.Max(e => e.AgeTo) + 1 : 65;
                ageFrom = defaultAge;
                ageTo   = ageFrom + 10;
            }

            var entity = new RetirementTimeline
            {
                ScenarioId   = request.ScenarioId,
                Name         = request.Name,
                AgeFrom      = ageFrom,
                AgeTo        = ageTo,
                TimelineType = request.TimelineType
            };
            var created = await repository.CreateAsync(entity);
            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateRetirementSpendingResult { Success = true, RetirementSpendingId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateRetirementSpendingResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateRetirementSpending for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateRetirementSpendingHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Created RetirementSpending {Id} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateRetirementSpendingHandler> logger, long Id, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error creating RetirementSpending for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateRetirementSpendingHandler> logger, string Exception, long ScenarioId);
}
