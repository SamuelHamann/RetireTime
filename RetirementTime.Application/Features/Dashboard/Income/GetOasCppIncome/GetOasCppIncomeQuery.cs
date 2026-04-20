using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetOasCppIncome;

public record GetOasCppIncomeQuery(long ScenarioId) : IRequest<OasCppIncome?>;
