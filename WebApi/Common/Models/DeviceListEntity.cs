using Microsoft.WindowsAzure.Storage.Table;

namespace PnIotPoc.WebApi.Common.Models
{
    public class DeviceListEntity : TableEntity
    {
        public DeviceListEntity(string hostName, string deviceId)
        {
            PartitionKey = deviceId;
            RowKey = hostName;
        }

        public DeviceListEntity() { }

        [IgnoreProperty]
        public string HostName
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        [IgnoreProperty]
        public string DeviceId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public string Key { get; set; }
    }
}