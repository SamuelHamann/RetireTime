using System.ComponentModel;

namespace RetirementTime.Domain.Entities.Common;

public enum FrequencyEnum
{
    [Description("Weekly")]
    Weekly = 1,
    [Description("Bi-Weekly")]
    BiWeekly = 2,
    [Description("Monthly")]
    Monthly = 3,
    [Description("Bi-Monthly")]
    BiMonthly = 4,
    [Description("Quarterly")]
    Quarterly = 5,
    [Description("Semi-Annually")]
    SemiAnnually = 6,
    [Description("Annually")]
    Annually = 7
}