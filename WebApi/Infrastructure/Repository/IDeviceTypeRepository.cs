using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public interface IDeviceTypeRepository
    {
        Task<List<DeviceType>> GetAllDeviceTypesAsync();
        Task<DeviceType> GetDeviceTypeAsync(int deviceTypeId);
    }
}
