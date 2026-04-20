using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteAssetsInvestmentProperty;

public record DeleteAssetsInvestmentPropertyCommand(long Id) : IRequest<BaseResult>;
