namespace RetirementTime.Models.Income;

public class OtherEmploymentIncomeItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Gross { get; set; }
    public decimal? Net { get; set; }
}

public class EmploymentItemModel
{
    public long Id { get; set; }
    public string EmployerName { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? GrossCommissions { get; set; }
    public decimal? NetCommissions { get; set; }
    public decimal? GrossBonus { get; set; }
    public decimal? NetBonus { get; set; }
    public List<OtherEmploymentIncomeItemModel> OtherIncomes { get; set; } = [];
}
