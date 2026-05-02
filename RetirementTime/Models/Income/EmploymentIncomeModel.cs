using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Income;

public class OtherEmploymentIncomeItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Gross { get; set; }
    public int GrossFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? Net { get; set; }
    public int NetFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
}

public class EmploymentItemModel
{
    public long Id { get; set; }
    public string EmployerName { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public int GrossSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetSalary { get; set; }
    public int NetSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? GrossCommissions { get; set; }
    public int GrossCommissionsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetCommissions { get; set; }
    public int NetCommissionsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? GrossBonus { get; set; }
    public int GrossBonusFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetBonus { get; set; }
    public int NetBonusFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public List<OtherEmploymentIncomeItemModel> OtherIncomes { get; set; } = [];
}
