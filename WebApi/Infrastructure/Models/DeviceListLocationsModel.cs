using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PnIotPoc.WebApi.Infrastructure.Models
{
    public class DeviceListLocationsModel
    {
        public List<DeviceLocationModel> DeviceLocationList { get; set; }
        public double MinimumLatitude { get; set; }
        public double MaximumLatitude { get; set; }
        public double MinimumLongitude { get; set; }
        public double MaximumLongitude { get; set; }
    }
}