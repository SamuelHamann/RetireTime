using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteInvestmentAccount;

public record DeleteInvestmentAccountCommand(long Id) : IRequest<BaseResult>;
