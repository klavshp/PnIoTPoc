using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    /// Interface to expose methods that can be called against the underlying identity repository
    /// </summary>
    public interface IIoTHubDeviceManager
    {
        Task<Device> AddDeviceAsync(Device device);
        Task<Device> GetDeviceAsync(string deviceId);
        Task RemoveDeviceAsync(string deviceId);
        Task<Device> UpdateDeviceAsync(Device device);
        Task SendAsync(string deviceId, Message message);
        Task CloseAsyncService();
        Task CloseAsyncDevice();
    }
}
