using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentProperties;

public partial class GetInvestmentPropertiesHandler(
    IInvestmentPropertyRepository investmentPropertyRepository,
    ILogger<GetInvestmentPropertiesHandler> logger) : IRequestHandler<GetInvestmentPropertiesQuery, List<InvestmentPropertyDto>>
{
    public async Task<List<InvestmentPropertyDto>> Handle(GetInvestmentPropertiesQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var properties = await investmentPropertyRepository.GetByUserIdAsync(request.UserId);
            
            var result = properties.Select(p => new InvestmentPropertyDto
            {
                Id = p.Id,
                Name = p.Name,
                PurchasePrice = p.PurchasePrice,
                MonthlyMortgagePayments = p.MonthlyMortgagePayments,
                MortgageLeft = p.MortgageLeft,
                YearlyInsurance = p.YearlyInsurance,
                MonthlyElectricityCosts = p.MonthlyElectricityCosts,
                MortgageDuration = p.MortgageDuration,
                MortgageStartDate = p.MortgageStartDate,
                EstimatedValue = p.EstimatedValue,
                MonthlyCost = p.MonthlyCost,
                MonthlyRevenue = p.MonthlyRevenue
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

    [LoggerMessage(LogLevel.Information, "Starting GetInvestmentProperties query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetInvestmentPropertiesHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} investment properties for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetInvestmentPropertiesHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving investment properties for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetInvestmentPropertiesHandler> logger, string exception, long userId);
}

