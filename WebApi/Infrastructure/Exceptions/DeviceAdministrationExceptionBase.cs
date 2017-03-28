using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PnIotPoc.WebApi.Infrastructure.Exceptions
{
    public abstract class DeviceAdministrationExceptionBase : Exception
    {
        // TODO: Localize this, if neccessary.
        private const string DeviceIdMessageFormatString = "DeviceId: {0}";

        public string DeviceId { get; set; }

        public DeviceAdministrationExceptionBase() : base()
        {
        }

        public DeviceAdministrationExceptionBase(string deviceId)
            : base(string.Format(CultureInfo.CurrentCulture, DeviceIdMessageFormatString, deviceId))
        {
            DeviceId = deviceId;
        }

        public DeviceAdministrationExceptionBase(string deviceId, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, DeviceIdMessageFormatString, deviceId), innerException)
        {
            DeviceId = deviceId;
        }

        // protected constructor for deserialization
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected DeviceAdministrationExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.DeviceId = info.GetString("DeviceId");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("DeviceId", DeviceId);
            base.GetObjectData(info, context);
        }

    }
}