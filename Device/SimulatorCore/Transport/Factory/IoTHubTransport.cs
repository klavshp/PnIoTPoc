using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Helpers;

namespace PnIotPoc.Device.SimulatorCore.Transport.Factory
{
    /// <summary>
    /// Implementation of ITransport that talks to IoT Hub.
    /// </summary>
    public class IoTHubTransport : ITransport
    {
        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IDevice _device;
        private DeviceClient _deviceClient;
        private bool _disposed;

        public IoTHubTransport(ILogger logger, IConfigurationProvider configurationProvider, IDevice device)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
            _device = device;
        }

        public void Open()
        {
            if (string.IsNullOrWhiteSpace(_device.DeviceID))
            {
                throw new ArgumentException("DeviceID value cannot be missing, null, or whitespace");
            }

            _deviceClient = DeviceClient.CreateFromConnectionString(GetConnectionString());
        }

        public async Task CloseAsync()
        {
            await _deviceClient.CloseAsync();
        }

        /// <summary>
        /// Builds the IoT Hub connection string
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string key = _device.PrimaryAuthKey;
            string deviceId = _device.DeviceID;
            string hostName = _device.HostName;

            var authMethod = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, key);
            return IotHubConnectionStringBuilder.Create(hostName, authMethod).ToString();
        }

        /// <summary>
        /// Sends an event to the IoT Hub
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task SendEventAsync(dynamic eventData)
        {
            var eventId = Guid.NewGuid();
            await SendEventAsync(eventId, eventData);
        }

        /// <summary>
        /// Sends an event to IoT Hub using the provided eventId GUID
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task SendEventAsync(Guid eventId, dynamic eventData)
        {
            string objectType = this.GetObjectType(eventData);
            var objectTypePrefix = _configurationProvider.GetConfigurationSettingValue("ObjectTypePrefix");

            if (!string.IsNullOrWhiteSpace(objectType) && !string.IsNullOrEmpty(objectTypePrefix))
            {
                eventData.ObjectType = objectTypePrefix + objectType;
            }

            // sample code to trace the raw JSON that is being sent
            string rawJson = JsonConvert.SerializeObject(eventData);
            _logger.LogInfo("Sending message: " + rawJson);

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventData));

            var message = new Message(bytes);
            message.Properties["EventId"] = eventId.ToString();

            await AzureRetryHelper.OperationWithBasicRetryAsync(async () =>
            {
                try
                {
                    await _deviceClient.SendEventAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "{0}{0}*** Exception: SendEventAsync ***{0}{0}EventId: {1}{0}Event Data: {2}{0}Exception: {3}{0}{0}",
                        Console.Out.NewLine,
                        eventId,
                        eventData,
                        ex);
                }
            });
        }


        //public Task SendEventBatchAsync(IEnumerable<Microsoft.Azure.Devices.Message> messages)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task SendEventBatchAsync(IEnumerable<Message> messages)
        {
            await _deviceClient.SendEventBatchAsync(messages);
        }

        /// <summary>
        /// Retrieves the next message from the IoT Hub
        /// </summary>
        /// <returns>Returns a DeserializableCommand that wraps the byte array of the message from IoT Hub</returns>
        public async Task<DeserializableCommand> ReceiveAsync()
        {
            Microsoft.Azure.Devices.Client.Message message = await AzureRetryHelper.OperationWithBasicRetryAsync(
                async () =>
                {
                    Exception exp = null;
                    Microsoft.Azure.Devices.Client.Message msg = null;
                    try
                    {
                        msg = await _deviceClient.ReceiveAsync();
                    }
                    catch (Exception exception)
                    {
                        exp = exception;
                    }

                    if (exp != null)
                    {
                        _logger.LogError(
                            "{0}{0}*** Exception: ReceiveAsync ***{0}{0}{1}{0}{0}",
                            Console.Out.NewLine,
                            exp);

                        if (msg != null)
                        {
                            await _deviceClient.AbandonAsync(msg);
                        }
                    }

                    return msg;
                });

            if (message != null)
            {
                return new DeserializableCommand(message);
            }

            return null;
        }

        public async Task SignalAbandonedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Debug.Assert(
                !string.IsNullOrEmpty(command.LockToken),
                "command.LockToken is a null reference or empty string.");

            await AzureRetryHelper.OperationWithBasicRetryAsync(
                async () =>
                {
                    try
                    {
                        await _deviceClient.AbandonAsync(command.LockToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            "{0}{0}*** Exception: Abandon Command ***{0}{0}Command Name: {1}{0}Command: {2}{0}Exception: {3}{0}{0}",
                            Console.Out.NewLine,
                            command.CommandName,
                            command.CommandHistory,
                            ex);
                    }
                });
        }

        public async Task SignalCompletedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Debug.Assert(
                !string.IsNullOrEmpty(command.LockToken),
                "command.LockToken is a null reference or empty string.");

            await AzureRetryHelper.OperationWithBasicRetryAsync(
                async () =>
                {
                    try
                    {
                        await _deviceClient.CompleteAsync(command.LockToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            "{0}{0}*** Exception: Complete Command ***{0}{0}Command Name: {1}{0}Command: {2}{0}Exception: {3}{0}{0}",
                            Console.Out.NewLine,
                            command.CommandName,
                            command.CommandHistory,
                            ex);
                    }
                });
        }

        public async Task SignalRejectedCommand(DeserializableCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Debug.Assert(
                !string.IsNullOrEmpty(command.LockToken),
                "command.LockToken is a null reference or empty string.");

            await AzureRetryHelper.OperationWithBasicRetryAsync(
                async () =>
                {
                    try
                    {
                        await _deviceClient.RejectAsync(command.LockToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            "{0}{0}*** Exception: Reject Command ***{0}{0}Command Name: {1}{0}Command: {2}{0}Exception: {3}{0}{0}",
                            Console.Out.NewLine,
                            command.CommandName,
                            command.CommandHistory,
                            ex);
                    }
                });
        }

        private string GetObjectType(dynamic eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException("eventData");
            }

            var propertyInfo = eventData.GetType().GetProperty("ObjectType");
            if (propertyInfo == null)
                return "";
            var value = propertyInfo.GetValue(eventData, null);
            return value == null ? "" : value.ToString();
        }

        /// <summary>
        /// Implement the IDisposable interface in order to close the device manager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_deviceClient != null)
                {
                    _deviceClient.CloseAsync().Wait();
                }
            }

            _disposed = true;
        }

        ~IoTHubTransport()
        {
            Dispose(false);
        }
    }
}
