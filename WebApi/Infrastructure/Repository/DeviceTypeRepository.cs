using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    /// Sample implemementation of Device Type data
    /// In a real world implementation this data may be backed by storage such as Table Storage or SQL Server
    /// </summary>
    public class SampleDeviceTypeRepository : IDeviceTypeRepository
    {
        List<DeviceType> DeviceTypes = new List<DeviceType>
        {
            new DeviceType
            {
                Name = "Simulated Device",
                DeviceTypeId = 1,
                Description = "Software to simulate a device. Easily extensible for arbitrary events and commands; can run in a Windows Azure worker role. To create a simulated device, please follow the cooler sample instructions.",
                InstructionsUrl = null,
                IsSimulatedDevice = true
            },
            new DeviceType
            {
                Name = "Custom Device",
                DeviceTypeId = 2,
                Description = "A physical hardware device.",
                InstructionsUrl = "https://azure.microsoft.com/documentation/articles/iot-suite-connecting-devices/"
            }
        };

        /// <summary>
        /// Return the full list of device types available
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceType>> GetAllDeviceTypesAsync()
        {
            return await Task.Run(() => { return DeviceTypes; });
        }

        /// <summary>
        /// Return a single device type
        /// </summary>
        /// <param name="deviceTypeId"></param>
        /// <returns></returns>
        public async Task<DeviceType> GetDeviceTypeAsync(int deviceTypeId)
        {
            return await Task.Run(() =>
            {
                return DeviceTypes.FirstOrDefault(dt => dt.DeviceTypeId == deviceTypeId);
            });
        }

    }
}