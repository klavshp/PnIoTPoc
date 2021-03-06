﻿using System;
using System.Collections.Generic;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Common.Models.Commands;

namespace PnIotPoc.WebApi.Common.Helpers
{
    /// <summary>
    /// Helper class to encapsulate interactions with the device schema.
    ///
    /// behind the schema.
    /// </summary>
    public static class DeviceCreatorHelper
    {
        /// <summary>
        /// Build a valid device representation used throughout the app.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="isSimulated"></param>
        /// <param name="iccid"></param>
        /// <returns></returns>
        public static DeviceModel BuildDeviceStructure(string deviceId, bool isSimulated, string iccid)
        {
            DeviceModel device = new DeviceModel();

            InitializeDeviceProperties(device, deviceId, isSimulated);
            InitializeSystemProperties(device, iccid);

            device.Commands = new List<Command>();
            device.CommandHistory = new List<CommandHistory>();
            device.IsSimulatedDevice = isSimulated;

            return device;
        }

        /// <summary>
        /// Initialize the device properties for a new device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="deviceId"></param>
        /// <param name="isSimulated"></param>
        /// <returns></returns>
        private static void InitializeDeviceProperties(DeviceModel device, string deviceId, bool isSimulated)
        {
            var deviceProps = new DeviceProperties
            {
                DeviceID = deviceId,
                HubEnabledState = null,
                CreatedTime = DateTime.UtcNow,
                DeviceState = "normal",
                UpdatedTime = null
            };

            device.DeviceProperties = deviceProps;
        }

        /// <summary>
        /// Initialize the system properties for a new device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="iccid"></param>
        /// <returns></returns>
        private static void InitializeSystemProperties(DeviceModel device, string iccid)
        {
            SystemProperties systemProps = new SystemProperties();
            systemProps.ICCID = iccid;

            device.SystemProperties = systemProps;
        }
    }
}