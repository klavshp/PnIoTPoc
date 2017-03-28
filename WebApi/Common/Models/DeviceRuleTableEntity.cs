using Microsoft.WindowsAzure.Storage.Table;

namespace PnIotPoc.WebApi.Common.Models
{
    public class DeviceRuleTableEntity : TableEntity
    {
        public DeviceRuleTableEntity(string deviceId, string ruleId)
        {
            PartitionKey = deviceId;
            RowKey = ruleId;
        }

        public DeviceRuleTableEntity() { }

        [IgnoreProperty]
        public string DeviceId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        [IgnoreProperty]
        public string RuleId
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string DataField { get; set; }

        public double Threshold { get; set; }

        public string RuleOutput { get; set; }

        public bool Enabled { get; set; }

        public string RuleName { get; set; }
    }
}