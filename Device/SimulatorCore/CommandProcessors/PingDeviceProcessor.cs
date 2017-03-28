using System;
using System.Threading.Tasks;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Transport;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.SimulatorCore.CommandProcessors
{
    public class PingDeviceProcessor : CommandProcessor
    {
        public PingDeviceProcessor(IDevice device)
            : base(device)
        {

        }

        public override async Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == "PingDevice")
            {
                CommandHistory command = deserializableCommand.CommandHistory;

                try
                {
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
