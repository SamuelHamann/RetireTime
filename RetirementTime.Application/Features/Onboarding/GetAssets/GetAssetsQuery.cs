using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.GetAssets;

public record GetAssetsQuery : IRequest<GetAssetsResult>
{
    public required long UserId { get; init; }
}

public record GetAssetsResult : BaseResult
{
    public AssetsDto? Assets { get; init; }
}

public record AssetsDto
{
    public long Id { get; init; }
    
    // Financial Assets
    public bool HasSavingsAccount { get; init; }
    public bool HasTFSA { get; init; }
    public bool HasRRSP { get; init; }
    public bool HasRRIF { get; init; }
    public bool HasFHSA { get; init; }
    public bool HasRESP { get; init; }
    public bool HasRDSP { get; init; }
    public bool HasNonRegistered { get; init; }
    public bool HasPension { get; init; }
    
    // Physical Assets
    public bool HasPrincipalResidence { get; init; }
    public bool HasCar { get; init; }
    public bool HasInvestmentProperty { get; init; }
    public bool HasBusiness { get; init; }
    public bool HasIncorporation { get; init; }
    public bool HasPreciousMetals { get; init; }
    public bool HasOtherHardAssets { get; init; }
}
