using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Locations.GetAccountTypes;

public partial class GetAccountTypesHandler(
    IAccountTypeRepository accountTypeRepository,
    ILogger<GetAccountTypesHandler> logger) : IRequestHandler<GetAccountTypesQuery, GetAccountTypesResult>
{
    public async Task<GetAccountTypesResult> Handle(GetAccountTypesQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.CountryId);

        try
        {
            var accountTypes = request.CountryId.HasValue
                ? await accountTypeRepository.GetByCountryIdAsync(request.CountryId.Value)
                : await accountTypeRepository.GetAllAsync();

            var accountTypeDtos = accountTypes.Select(a => new AccountTypeDto
            {
                Id = a.Id,
                Name = a.Name,
                CountryId = a.CountryId
            }).ToList();

            LogQuerySuccessful(logger, accountTypeDtos.Count);

            return new GetAccountTypesResult
            {
                AccountTypes = accountTypeDtos
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message);
            return new GetAccountTypesResult
            {
                AccountTypes = new List<AccountTypeDto>()
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting query for CountryId: {CountryId}")]
    static partial void LogStartingQuery(ILogger logger, int? countryId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved account types, Count: {Count}")]
    static partial void LogQuerySuccessful(ILogger logger, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception);
}

