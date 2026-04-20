using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Debt
{
    public class GenericDebt
    {
        public long Id { get; set; }
        public long ScenarioId { get; set; }
        public long DebtTypeId { get; set; }
        public bool IsHomeMortgage { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal? Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public int FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
        public int? TermInYears { get; set; }

        // Nullable FK — links this debt to a specific asset when applicable
        public long? DebtAgainstAssetId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Frequency Frequency { get; set; } = null!;
        public DashboardScenario Scenario { get; set; } = null!;
        public DebtType DebtType { get; set; } = null!;
    }

    public class DebtType
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum DebtTypeEnum
    {
        Mortgage               = 1,
        HomeEquityLineOfCredit = 2,
        CarLoan                = 3,
        StudentLoan            = 4,
        CreditCard             = 5,
        PersonalLoan           = 6,
        LineOfCredit           = 7,
        Other                  = 8,
        Medical                = 9,
    }
}
