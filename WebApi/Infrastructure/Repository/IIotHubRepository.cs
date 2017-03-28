using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    /// Interface to expose methods that can be called against the underlying identity repository
    /// </summary>
    public interface IIotHubRepository
    {
        Task<Device> GetIotHubDeviceAsync(string deviceId);
        Task<DeviceModel> AddDeviceAsync(DeviceModel device, SecurityKeys securityKeys);
        Task<bool> TryAddDeviceAsync(Device oldIotHubDevice);
        Task RemoveDeviceAsync(string deviceId);
        Task<bool> TryRemoveDeviceAsync(string deviceId);
        Task<Device> UpdateDeviceEnabledStatusAsync(string deviceId, bool isEnabled);
        Task SendCommand(string deviceId, CommandHistory command);
        Task<SecurityKeys> GetDeviceKeysAsync(string id);
    }
}
