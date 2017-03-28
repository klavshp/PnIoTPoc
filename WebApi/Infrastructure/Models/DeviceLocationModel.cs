using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PnIotPoc.WebApi.Infrastructure.Models
{
    public class DeviceLocationModel
    {
        public string DeviceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}