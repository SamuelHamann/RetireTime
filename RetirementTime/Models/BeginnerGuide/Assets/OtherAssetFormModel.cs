using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Assets;

public class OtherAssetFormModel
{
    public bool HasOtherAssets { get; set; }
    public List<OtherAssetItemModel> Assets { get; set; } = new();
}

public class OtherAssetItemModel
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Asset name is required")]
    [StringLength(200, ErrorMessage = "Asset name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Asset type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select an asset type")]
    public int AssetTypeId { get; set; }
    
    [Required(ErrorMessage = "Current value is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Current value must be greater than 0")]
    public decimal CurrentValue { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Purchase price cannot be negative")]
    public decimal? PurchasePrice { get; set; }
}
