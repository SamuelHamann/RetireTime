using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.UpsertDebt;

public record UpsertDebtCommand : IRequest<UpsertDebtResult>
{
    public required long UserId { get; init; }
    
    public required bool HasPrimaryMortgage { get; init; }
    public required bool HasInvestmentPropertyMortgage { get; init; }
    public required bool HasCarPayments { get; init; }
    public required bool HasStudentLoans { get; init; }
    public required bool HasCreditCardDebt { get; init; }
    public required bool HasPersonalLoans { get; init; }
    public required bool HasBusinessLoans { get; init; }
    public required bool HasTaxDebt { get; init; }
    public required bool HasMedicalDebt { get; init; }
    public required bool HasInformalDebt { get; init; }
}

public record UpsertDebtResult : BaseResult
{
    public long? DebtId { get; init; }
}
