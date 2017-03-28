using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PnIotPoc.WebApi.Infrastructure.Exceptions
{
    [Serializable]
    public class DeviceAlreadyRegisteredException : DeviceAdministrationExceptionBase
    {
        public DeviceAlreadyRegisteredException(string deviceId) : base(deviceId)
        {
        }

        // protected constructor for deserialization
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected DeviceAlreadyRegisteredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message => string.Format(CultureInfo.CurrentCulture, $"Device {DeviceId} is already registered.");
    }
}