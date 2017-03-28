using System.Collections.Generic;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.WebApi.Infrastructure.Models
{
    public class DeviceListQueryResult
    {
        public int TotalDeviceCount { get; set; }
        public int TotalFilteredCount { get; set; }
        public List<DeviceModel> Results { get; set; }
    }
}