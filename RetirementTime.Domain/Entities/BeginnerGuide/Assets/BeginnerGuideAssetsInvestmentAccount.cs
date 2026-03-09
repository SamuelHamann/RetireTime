using System;
using System.Collections.Generic;

namespace RetirementTime.Domain.Entities.BeginnerGuide.Assets;

public class BeginnerGuideAssetsInvestmentAccount
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }
    public bool IsBulkAmount { get; set; }
    public decimal? BulkAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public required User User { get; set; }
    public required BeginnerGuideAccountType AccountType { get; set; }
    public virtual ICollection<BeginnerGuideAssetsStockData> Stocks { get; set; } = new List<BeginnerGuideAssetsStockData>();
}

