using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;

namespace RetirementTime.Application.Features.BeginnerGuide.Debts.UpsertDebts;

public class UpsertDebtsCommand : IRequest<UpsertDebtsResult>
{
    public long UserId { get; set; }
    public bool HasDebts { get; set; }
    public List<DebtInputDto> Debts { get; set; } = new();
}

public class DebtInputDto
{
    public long? Id { get; set; }
    public DebtType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal InterestRate { get; set; }
}

public record UpsertDebtsResult : BaseResult
{
    public List<long> DebtIds { get; init; } = new();
}

