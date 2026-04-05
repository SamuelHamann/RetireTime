using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.GetDebt;

public record GetDebtQuery : IRequest<GetDebtResult>
{
    public required long UserId { get; init; }
}

public record GetDebtResult : BaseResult
{
    public DebtDto? Debt { get; init; }
}

public record DebtDto
{
    public long Id { get; init; }
    
    public bool HasPrimaryMortgage { get; init; }
    public bool HasInvestmentPropertyMortgage { get; init; }
    public bool HasCarPayments { get; init; }
    public bool HasStudentLoans { get; init; }
    public bool HasCreditCardDebt { get; init; }
    public bool HasPersonalLoans { get; init; }
    public bool HasBusinessLoans { get; init; }
    public bool HasTaxDebt { get; init; }
    public bool HasMedicalDebt { get; init; }
    public bool HasInformalDebt { get; init; }
}
