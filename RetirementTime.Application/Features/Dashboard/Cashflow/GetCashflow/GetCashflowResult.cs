namespace RetirementTime.Application.Features.Dashboard.Cashflow.GetCashflow;

public record GetCashflowResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public CashflowSankeyDto Data { get; init; } = new();
    public List<YearlyCashFlowDto> YearlyCashFlows { get; init; } = [];
}

public class YearlyCashFlowDto
{
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
}

public class CashflowSankeyDto
{
    public List<CashflowNodeDto> Nodes { get; set; } = [];
    public List<CashflowLinkDto> Links { get; set; } = [];
}

public class CashflowNodeDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class CashflowLinkDto
{
    public string Source { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

