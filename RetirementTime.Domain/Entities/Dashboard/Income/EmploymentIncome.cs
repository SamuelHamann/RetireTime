namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class EmploymentIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long UserId { get; set; }
    public string EmployerName { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? GrossCommissions { get; set; }
    public decimal? NetCommissions { get; set; }
    public decimal? GrossBonus { get; set; }
    public decimal? NetBonus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public List<OtherEmploymentIncome> OtherIncomes { get; set; } = [];
}
