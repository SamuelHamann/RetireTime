using RetirementTime.Domain.Entities.RealEstate;
using RetirementTime.Domain.Interfaces.Math;

namespace RetirementTime.Domain.Services;

public class RealEstateMathService : IRealEstateMathService
{
    private const string ErrorLocationBasicCompound = "RealEstateMathService|BasicCompoundInterestWithContributions|";
    private const string ErrorLocationBuyOrRent = "RealEstateMathService|BuyOrRent|";
    private const string ErrorLocationMortgage = "RealEstateMathService|MortgagePayment|";
    
    public Tuple<Dictionary<string, double>, Dictionary<string, double>> BuyOrRent(BuyOrRent buyOrRent)
    {
        throw new NotImplementedException();
    }

    public double BasicCompoundInterestWithContributions(double initialValue, double annualInterestRate, double contributionAmount,
        int numberOfPeriods, int numberOfYears)
    {
        if (initialValue < 0)
            throw new ApplicationException($"{ErrorLocationBasicCompound}Initial value must be greater than or equal to 0.");
        if (annualInterestRate < 0)
            throw new ApplicationException($"{ErrorLocationBasicCompound}Annual interest rate must be greater than or equal to 0.");
        if (contributionAmount < 0)
            throw new ApplicationException($"{ErrorLocationBasicCompound}Contribution amount must be greater than or equal to 0.");
        if (numberOfPeriods < 0)
            throw new ApplicationException($"{ErrorLocationBasicCompound}Number of periods must be greater than or equal to 0.");
        if (numberOfYears < 0)
            throw new ApplicationException($"{ErrorLocationBasicCompound}Number of years must be greater than or equal to 0.");
        
        var initialValueGrowth = initialValue * Math.Pow(1 + annualInterestRate/numberOfPeriods, numberOfPeriods * numberOfYears);
        var monthlyContributionGrowth = contributionAmount * ((Math.Pow(1 +  annualInterestRate/numberOfPeriods, numberOfPeriods * numberOfYears) - 1) / (annualInterestRate/numberOfPeriods));
        return initialValueGrowth + monthlyContributionGrowth;
    }

    public double MortgagePayment(double principal, double annualInterestRate, int amortizationInYears,
        int numberOfPaymentsPerYear)
    {
        if (principal < 0)
            throw new ApplicationException($"{ErrorLocationMortgage}Principak value must be greater than 0");
        if (annualInterestRate < 0)
            throw new ApplicationException($"{ErrorLocationMortgage}Annual interest rate value must be greater than 0");
        if (amortizationInYears < 0)
            throw new ApplicationException($"{ErrorLocationMortgage}Amortization value must be greater than 0");
        if (numberOfPaymentsPerYear < 0)
            throw new ApplicationException($"{ErrorLocationMortgage}Number of payments value must be greater than 0");

        var ratePerPeriod = annualInterestRate / numberOfPaymentsPerYear;
        var totalNumberOfPayments = amortizationInYears * numberOfPaymentsPerYear;

        if (annualInterestRate == 0)
            return principal / totalNumberOfPayments;
        
        return principal * (ratePerPeriod * Math.Pow(1 + ratePerPeriod, totalNumberOfPayments / (Math.Pow(1 + ratePerPeriod, totalNumberOfPayments) - 1)));
    }

    private double CalculateDifferenceToReinvest(BuyOrRent buyOrRent)
    {
        var mortgagePayments = MortgagePayment(buyOrRent.Mortgage.RealEstate.Price,
            buyOrRent.Mortgage.InterestRate, buyOrRent.Mortgage.TermInYears, 12);
        return mortgagePayments;
    }
    
}