using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web;

namespace PnIotPoc.WebApi.Infrastructure.Exceptions
{
    [Serializable]
    public class DeviceNotRegisteredException : DeviceAdministrationExceptionBase
    {
        public DeviceNotRegisteredException(string deviceId) : base(deviceId)
        {
        }

        // protected constructor for deserialization
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected DeviceNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message => string.Format(CultureInfo.CurrentCulture, $"Device {DeviceId} is not registered.");
    }
}