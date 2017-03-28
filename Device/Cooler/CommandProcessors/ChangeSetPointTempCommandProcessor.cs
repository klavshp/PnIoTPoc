using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Devices;
using PnIotPoc.Device.SimulatorCore.CommandProcessors;
using PnIotPoc.Device.SimulatorCore.Transport;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.Cooler.CommandProcessors
{
    /// <summary>
    /// Command processor to handle the change in the temperature range
    /// </summary>
    public class ChangeSetPointTempCommandProcessor : CommandProcessor
    {
        private const string CHANGE_SET_POINT_TEMP = "ChangeSetPointTemp";

        public ChangeSetPointTempCommandProcessor(CoolerDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == CHANGE_SET_POINT_TEMP)
            {
                CommandHistory commandHistory = deserializableCommand.CommandHistory;

                try
                {
                    var device = Device as CoolerDevice;
                    if (device != null)
                    {
                        dynamic parameters = commandHistory.Parameters;
                        if (parameters != null)
                        {
                            dynamic setPointTempDynamic = ReflectionHelper.GetNamedPropertyValue(
                                parameters,
                                "SetPointTemp",
                                usesCaseSensitivePropertyNameMatch: true,
                                exceptionThrownIfNoMatch: true);

                            if (setPointTempDynamic != null)
                            {
                                double setPointTemp;
                                if (Double.TryParse(setPointTempDynamic.ToString(), out setPointTemp))
                                {
                                    device.ChangeSetPointTemp(setPointTemp);

                                    return CommandProcessingResult.Success;
                                }
                                else
                                {
                                    // SetPointTemp cannot be parsed as a double.
                                    return CommandProcessingResult.CannotComplete;
                                }
                            }
                            else
                            {
                                // setPointTempDynamic is a null reference.
                                return CommandProcessingResult.CannotComplete;
                            }
                        }
                        else
                        {
                            // parameters is a null reference.
                            return CommandProcessingResult.CannotComplete;
                        }
                    }
                    else
                    {
                        // Unsupported Device type.
                        return CommandProcessingResult.CannotComplete;
                    }
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
