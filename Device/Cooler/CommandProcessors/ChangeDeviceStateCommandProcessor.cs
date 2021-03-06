﻿using System;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Devices;
using PnIotPoc.Device.SimulatorCore.CommandProcessors;
using PnIotPoc.Device.SimulatorCore.Transport;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.Cooler.CommandProcessors
{
    /// <summary>
    /// Command processor to handle the change in device state.
    /// Currently this just changes the DeviceState string on the device.
    /// </summary>
    public class ChangeDeviceStateCommandProcessor : CommandProcessor
    {
        private const string CHANGE_DEVICE_STATE = "ChangeDeviceState";

        public ChangeDeviceStateCommandProcessor(CoolerDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == CHANGE_DEVICE_STATE)
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
                            dynamic deviceState = ReflectionHelper.GetNamedPropertyValue(
                                parameters,
                                "DeviceState",
                                usesCaseSensitivePropertyNameMatch: true,
                                exceptionThrownIfNoMatch: true);

                            if (deviceState != null)
                            {
                                await device.ChangeDeviceState(deviceState.ToString());

                                return CommandProcessingResult.Success;
                            }
                            else
                            {
                                // DeviceState is a null reference.
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
