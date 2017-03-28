using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using PnIotPoc.WebApi.Common.Configurations;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    ///     Wraps calls to the IoT hub identity store.
    ///     IDisposable is implemented in order to close out the connection to the IoT Hub when this object is no longer in use
    /// </summary>
    public class IoTHubDeviceManager : IIoTHubDeviceManager, IDisposable
    {
        private readonly RegistryManager _deviceManager;
        private readonly ServiceClient _serviceClient;
        private bool _disposed;

        public IoTHubDeviceManager(IConfigurationProvider configProvider)
        {
            // Temporary code to bypass https cert validation till DNS on IotHub is configured
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            var iotHubConnectionString = configProvider.GetConfigurationSettingValue("iotHub.ConnectionString");
            _deviceManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(iotHubConnectionString);
        }

        public async Task<Device> AddDeviceAsync(Device device)
        {
            return await _deviceManager.AddDeviceAsync(device);
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            return await _deviceManager.GetDeviceAsync(deviceId);
        }

        public async Task RemoveDeviceAsync(string deviceId)
        {
            await _deviceManager.RemoveDeviceAsync(deviceId);
        }

        public async Task<Device> UpdateDeviceAsync(Device device)
        {
            return await _deviceManager.UpdateDeviceAsync(device);
        }

        public async Task SendAsync(string deviceId, Message message)
        {
            await _serviceClient.SendAsync(deviceId, message);
        }

        public async Task CloseAsyncDevice()
        {
            await _serviceClient.CloseAsync();
        }

        public async Task CloseAsyncService()
        {
            await _deviceManager.CloseAsync();
        }

        /// <summary>
        ///     Implement the IDisposable interface in order to close the device manager
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
                if (_deviceManager != null)
                {
                    _deviceManager.CloseAsync().Wait();
                }
            }

            _disposed = true;
        }

        ~IoTHubDeviceManager()
        {
            Dispose(false);
        }
    }
}