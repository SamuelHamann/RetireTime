namespace RetirementTime.Models.Introduction;

public class DebtModel
{
    public bool HasPrimaryMortgage { get; set; }
    public bool HasInvestmentPropertyMortgage { get; set; }
    public bool HasCarPayments { get; set; }
    public bool HasStudentLoans { get; set; }
    public bool HasCreditCardDebt { get; set; }
    public bool HasPersonalLoans { get; set; }
    public bool HasBusinessLoans { get; set; }
    public bool HasTaxDebt { get; set; }
    public bool HasMedicalDebt { get; set; }
    public bool HasInformalDebt { get; set; }
}

