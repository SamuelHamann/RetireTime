namespace RetirementTime.Domain.Interfaces.Math;

public interface IBuyOrRentService
{
    public Tuple<Dictionary<string, double>, Dictionary<string, double>> BuyOrRent();
}