using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PnIotPoc.WebApi.Infrastructure.Exceptions
{
    [Serializable]
    public class UnsupportedCommandException : DeviceAdministrationExceptionBase
    {
        public UnsupportedCommandException(string deviceId, string commandName) : base(deviceId)
        {
            CommandName = commandName;
        }

        // protected constructor for deserialization
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected UnsupportedCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.CommandName = info.GetString("CommandName");
        }

        public string CommandName { get; set; }

        public override string Message => string.Format(CultureInfo.CurrentCulture, $"Device {DeviceId} does not support the command {CommandName}.");

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("CommandName", CommandName);
            base.GetObjectData(info, context);
        }
    }
}