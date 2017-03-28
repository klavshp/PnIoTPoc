using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface IDeviceLogic
    {
        void ApplyDevicePropertyValueModels(DeviceModel device, IEnumerable<DevicePropertyValueModel> devicePropertyValueModels);
        Task<DeviceListQueryResult> GetDevices(DeviceListQuery q);
        Task<DeviceModel> GetDeviceAsync(string deviceId);
        Task<DeviceWithKeys> AddDeviceAsync(DeviceModel device);
        IEnumerable<DevicePropertyValueModel> ExtractDevicePropertyValuesModels(DeviceModel device);
        Task RemoveDeviceAsync(string deviceId);
        Task<DeviceModel> UpdateDeviceAsync(DeviceModel device);
        Task<DeviceModel> UpdateDeviceFromDeviceInfoPacketAsync(DeviceModel device);
        Task<DeviceModel> UpdateDeviceEnabledStatusAsync(string deviceId, bool isEnabled);
        Task<SecurityKeys> GetIoTHubKeysAsync(string id);
        Task GenerateNDevices(int deviceCount);
        Task SendCommandAsync(string deviceId, string commandName, dynamic parameters);
        Task<List<string>> BootstrapDefaultDevices();
        DeviceListLocationsModel ExtractLocationsData(List<DeviceModel> devices);
        IList<DeviceTelemetryFieldModel> ExtractTelemetry(DeviceModel device);
    }
}