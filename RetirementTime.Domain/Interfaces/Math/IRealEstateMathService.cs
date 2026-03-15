using RetirementTime.Domain.Entities.RealEstate;

namespace RetirementTime.Domain.Interfaces.Math;

public interface IRealEstateMathService
{
    public Tuple<Dictionary<string, double>, Dictionary<string, double>> BuyOrRent(BuyOrRent buyOrRent);

    public double BasicCompoundInterestWithContributions(double initialValue, double annualInterestRate,
        double contributionAmount, int numberOfPeriods, int numberOfYears);

    public double MortgagePayment(double principal, double annualInterestRate, int amortizationInYears,
        int numberOfPaymentsPerYear);
}