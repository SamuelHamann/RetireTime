namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class OtherEmploymentIncome
{
    public long Id { get; set; }
    public long EmploymentIncomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Gross { get; set; }
    public decimal? Net { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required EmploymentIncome EmploymentIncome { get; set; }
}
