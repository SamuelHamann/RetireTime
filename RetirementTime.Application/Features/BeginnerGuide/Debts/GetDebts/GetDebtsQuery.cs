using MediatR;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;

namespace RetirementTime.Application.Features.BeginnerGuide.Debts.GetDebts;

public class GetDebtsQuery : IRequest<List<DebtDto>>
{
    public long UserId { get; set; }
}

public class DebtDto
{
    public long Id { get; set; }
    public DebtType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal InterestRate { get; set; }
}

