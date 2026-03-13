using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetOtherRecurringGains;

public class GetOtherRecurringGainsQuery : IRequest<List<OtherRecurringGainDto>>
{
    public long UserId { get; set; }
}

public class OtherRecurringGainDto
{
    public long Id { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int FrequencyId { get; set; }
}

