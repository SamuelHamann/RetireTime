using MediatR;

namespace RetirementTime.Application.Features.Locations.GetAccountTypes;

public class GetAccountTypesQuery : IRequest<GetAccountTypesResult>
{
    public int? CountryId { get; set; }
}

public class GetAccountTypesResult
{
    public List<AccountTypeDto> AccountTypes { get; set; } = new();
}

public class AccountTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
}

