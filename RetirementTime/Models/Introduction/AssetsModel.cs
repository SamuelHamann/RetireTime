namespace RetirementTime.Models.Introduction;

public class AssetsModel
{
    // ---- Savings & Investment Accounts ----
    public bool HasSavingsAccount { get; set; }

    // Registered accounts — only shown when HasSavingsAccount is true
    public bool HasTFSA { get; set; }
    public bool HasRRSP { get; set; }
    public bool HasRRIF { get; set; }
    public bool HasFHSA { get; set; }
    public bool HasRESP { get; set; }
    public bool HasRDSP { get; set; }
    public bool HasNonRegistered { get; set; }
    public bool HasPension { get; set; }

    // ---- Physical Assets ----
    public bool HasPrincipalResidence { get; set; }
    public bool HasCar { get; set; }
    public bool HasInvestmentProperty { get; set; }
    public bool HasBusiness { get; set; }
    public bool HasIncorporation { get; set; }
    public bool HasPreciousMetals { get; set; }
    public bool HasOtherHardAssets { get; set; }
}

