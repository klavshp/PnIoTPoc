using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.WebApi.Infrastructure.Models
{
    public class DeviceWithKeys
    {
        public DeviceModel Device { get; set; }
        public SecurityKeys SecurityKeys { get; set; }

        public DeviceWithKeys(DeviceModel device, SecurityKeys securityKeys)
        {
            Device = device;
            SecurityKeys = securityKeys;
        }
    }
}