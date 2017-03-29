using System;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Devices;
using PnIotPoc.Device.SimulatorCore.CommandProcessors;
using PnIotPoc.Device.SimulatorCore.Transport;

namespace PnIotPoc.Device.Cooler.CommandProcessors
{
    /// <summary>
    /// Command processor to start telemetry data</summary>
    public class StartCommandProcessor : CommandProcessor
    {
        private const string START_TELEMETRY = "StartTelemetry";

        public StartCommandProcessor(CoolerDevice device)
            : base(device)
        {

        }

        public override async Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == START_TELEMETRY)
            {
                try
                {
                    var device = Device as CoolerDevice;
                    device.StartTelemetryData();
                    return CommandProcessingResult.Success;
                }
                catch (Exception)
                {
                    return CommandProcessingResult.RetryLater;
                }

            }
            else if (NextCommandProcessor != null)
            {
                return await NextCommandProcessor.HandleCommandAsync(deserializableCommand);
            }

            return CommandProcessingResult.CannotComplete;
        }
    }
}
