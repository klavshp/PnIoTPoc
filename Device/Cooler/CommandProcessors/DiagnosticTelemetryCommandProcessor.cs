using System;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Devices;
using PnIotPoc.Device.SimulatorCore.CommandProcessors;
using PnIotPoc.Device.SimulatorCore.Transport;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.Cooler.CommandProcessors
{
    /// <summary>
    /// Command processor to handle activating external temperature
    /// </summary>
    public class DiagnosticTelemetryCommandProcessor : CommandProcessor
    {
        private const string DIAGNOSTIC_TELEMETRY = "DiagnosticTelemetry";

        public DiagnosticTelemetryCommandProcessor(CoolerDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == DIAGNOSTIC_TELEMETRY)
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
                            dynamic activeAsDynamic =
                                ReflectionHelper.GetNamedPropertyValue(
                                    parameters,
                                    "Active",
                                    usesCaseSensitivePropertyNameMatch: true,
                                    exceptionThrownIfNoMatch: true);

                            if (activeAsDynamic != null)
                            {
                                var active = Convert.ToBoolean(activeAsDynamic.ToString());

                                if (active != null)
                                {
                                    device.DiagnosticTelemetry(active);
                                    return CommandProcessingResult.Success;
                                }
                                else
                                {
                                    // Active is not a boolean.
                                    return CommandProcessingResult.CannotComplete;
                                }
                            }
                            else
                            {
                                // Active is a null reference.
                                return CommandProcessingResult.CannotComplete;
                            }
                        }
                        else
                        {
                            // parameters is a null reference.
                            return CommandProcessingResult.CannotComplete;
                        }
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
