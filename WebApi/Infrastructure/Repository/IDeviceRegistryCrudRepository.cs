using System.Threading.Tasks;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public interface IDeviceRegistryCrudRepository
    {
        /// <summary>
        /// Adds a device asynchronously.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        Task<DeviceModel> AddDeviceAsync(DeviceModel device);

        /// <summary>
        /// Removes a device asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns></returns>
        Task RemoveDeviceAsync(string deviceId);

        /// <summary>
        /// Gets a device asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns></returns>
        Task<DeviceModel> GetDeviceAsync(string deviceId);

        /// <summary>
        /// Updates a device asynchronously.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        Task<DeviceModel> UpdateDeviceAsync(DeviceModel device);

        /// <summary>
        /// Updates a device enabled/diabled status asynchronously.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <returns></returns>
        Task<DeviceModel> UpdateDeviceEnabledStatusAsync(string deviceId, bool isEnabled);
    }
}
