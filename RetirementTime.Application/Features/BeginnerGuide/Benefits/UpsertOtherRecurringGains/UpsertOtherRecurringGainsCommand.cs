using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertOtherRecurringGains;

public class UpsertOtherRecurringGainsCommand : IRequest<UpsertOtherRecurringGainsResult>
{
    public long UserId { get; set; }
    public bool HasOtherRecurringGains { get; set; }
    public List<OtherRecurringGainInputDto> Gains { get; set; } = new();
}

public class OtherRecurringGainInputDto
{
    public long? Id { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int FrequencyId { get; set; }
}

public record UpsertOtherRecurringGainsResult : BaseResult
{
    public List<long> GainIds { get; init; } = new();
}

