using System;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Devices;
using PnIotPoc.Device.SimulatorCore.CommandProcessors;
using PnIotPoc.Device.SimulatorCore.Transport;

namespace PnIotPoc.Device.Cooler.CommandProcessors
{
    /// <summary>
    /// Command processor to stop telemetry data
    /// </summary>
    public class StopCommandProcessor : CommandProcessor
    {
        private const string STOP_TELEMETRY = "StopTelemetry";

        public StopCommandProcessor(CoolerDevice device)
            : base(device)
        {

        }

        public override async Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == STOP_TELEMETRY)
            {
                try
                {
                    var device = Device as CoolerDevice;
                    device.StopTelemetryData();
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
