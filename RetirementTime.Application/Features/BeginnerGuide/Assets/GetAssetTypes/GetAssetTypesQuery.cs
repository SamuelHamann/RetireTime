using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetAssetTypes;

public class GetAssetTypesQuery : IRequest<List<AssetTypeDto>>
{
}

public class AssetTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

