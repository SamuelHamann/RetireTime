namespace RetirementTime.Domain.Entities.Common;

/// <summary>
/// Government-defined constants used to calculate Old Age Security (OAS) benefits.
/// Stored in the database so values can be updated as CRA rates change quarterly
/// without requiring a code deployment.
/// </summary>
public class OasConstants
{
    public long Id { get; set; }

    /// <summary>Maximum monthly OAS payment for recipients aged 65–74.</summary>
    public decimal MonthlyRate65To74 { get; set; }

    /// <summary>Maximum monthly OAS payment for recipients aged 75 and over.</summary>
    public decimal MonthlyRate75Plus { get; set; }

    /// <summary>
    /// Annual net world income threshold above which the OAS recovery tax (clawback) begins.
    /// </summary>
    public decimal ClawbackThreshold { get; set; }

    /// <summary>
    /// Annual net world income at which OAS is fully eliminated for recipients aged 65–74.
    /// </summary>
    public decimal EliminationThreshold65To74 { get; set; }

    /// <summary>
    /// Annual net world income at which OAS is fully eliminated for recipients aged 75+.
    /// </summary>
    public decimal EliminationThreshold75Plus { get; set; }

    /// <summary>
    /// Monthly deferral increase rate (e.g. 0.006 = 0.6% per month deferred past age 65).
    /// </summary>
    public decimal DeferralRatePerMonth { get; set; }

    /// <summary>Minimum years of Canadian residency after age 18 required to receive OAS.</summary>
    public int MinResidencyYears { get; set; }

    /// <summary>Years of Canadian residency after age 18 required for a full OAS pension.</summary>
    public int FullResidencyYears { get; set; }

    /// <summary>Standard age at which OAS payments begin (65).</summary>
    public int StandardStartAge { get; set; }

    /// <summary>Maximum age to which OAS payments can be deferred (70).</summary>
    public int MaxDeferralAge { get; set; }

    /// <summary>Label identifying which rate period this row represents (e.g. "2026-Q1").</summary>
    public string PeriodLabel { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

