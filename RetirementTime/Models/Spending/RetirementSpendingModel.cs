namespace RetirementTime.Models.Spending;

public class RetirementSpendingModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AgeFrom { get; set; } = 65;
    public int AgeTo { get; set; } = 75;
    public long? CloneFromId { get; set; }
    public bool IsFullyCreated { get; set; }
}

