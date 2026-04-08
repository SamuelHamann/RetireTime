using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class EmploymentIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long UserId { get; set; }
    public string EmployerName { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public int GrossSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? NetSalary { get; set; }
    public int NetSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? GrossCommissions { get; set; }
    public int GrossCommissionsFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? NetCommissions { get; set; }
    public int NetCommissionsFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? GrossBonus { get; set; }
    public int GrossBonusFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? NetBonus { get; set; }
    public int NetBonusFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? PensionContributions { get; set; }
    public int PensionContributionFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    public decimal? TaxDeductions { get; set; }
    public int TaxDeductionFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    public decimal? CppDeductions { get; set; }
    public int CppDeductionFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    public decimal? OtherDeductions { get; set; }
    public int OtherDeductionFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public List<OtherEmploymentIncome> OtherIncomes { get; set; } = [];
    public Frequency GrossSalaryFrequency { get; set; } = null!;
    public Frequency NetSalaryFrequency { get; set; } = null!;
    public Frequency GrossCommissionsFrequency { get; set; } = null!;
    public Frequency NetCommissionsFrequency { get; set; } = null!;
    public Frequency GrossBonusFrequency { get; set; } = null!;
    public Frequency NetBonusFrequency { get; set; } = null!;
    public Frequency PensionContributionFrequency { get; set; } = null!;
    public Frequency TaxDeductionFrequency { get; set; } = null!;
    public Frequency CppDeductionFrequency { get; set; } = null!;
    public Frequency OtherDeductionFrequency { get; set; } = null!;
}
