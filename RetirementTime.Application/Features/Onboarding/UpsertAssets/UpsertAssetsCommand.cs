using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.UpsertAssets;

public record UpsertAssetsCommand : IRequest<UpsertAssetsResult>
{
    public required long UserId { get; init; }
    
    // Financial Assets
    public required bool HasSavingsAccount { get; init; }
    public required bool HasTFSA { get; init; }
    public required bool HasRRSP { get; init; }
    public required bool HasRRIF { get; init; }
    public required bool HasFHSA { get; init; }
    public required bool HasRESP { get; init; }
    public required bool HasRDSP { get; init; }
    public required bool HasNonRegistered { get; init; }
    public required bool HasPension { get; init; }
    
    // Physical Assets
    public required bool HasPrincipalResidence { get; init; }
    public required bool HasCar { get; init; }
    public required bool HasInvestmentProperty { get; init; }
    public required bool HasBusiness { get; init; }
    public required bool HasIncorporation { get; init; }
    public required bool HasPreciousMetals { get; init; }
    public required bool HasOtherHardAssets { get; init; }
}

public record UpsertAssetsResult : BaseResult
{
    public long? AssetsId { get; init; }
}
