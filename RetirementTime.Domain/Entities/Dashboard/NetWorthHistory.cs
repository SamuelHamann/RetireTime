using System;
using System.Collections.Generic;
using System.Text;

namespace RetirementTime.Domain.Entities.Dashboard
{
    public class NetWorthHistory
    {
        public long Id { get; set; }
        public long ScenarioId { get; set; }
        public DateTime DateOfSnapShot { get; set; }
        public decimal? Debt { get; set; }
        public decimal? Asset { get; set; }

        public string Debts { get; set; } = "[]";
        public string Assets { get; set; } = "[]";


        public DashboardScenario Scenario { get; set; } = null!;
    }

    public class NetWorthDebtItem
    {
        public string Name { get; set; } = string.Empty;
        public string DebtType { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
    }

    public class  NetWorthAssetItem
    {
        public string Name { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
    }
}
