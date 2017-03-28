using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    /// Class for working with persistence of Device Rules data
    /// </summary>
    public interface IDeviceRulesRepository
    {
        Task<List<DeviceRule>> GetAllRulesAsync();
        Task<DeviceRule> GetDeviceRuleAsync(string deviceId, string ruleId);
        Task<List<DeviceRule>> GetAllRulesForDeviceAsync(string deviceId);
        Task<TableStorageResponse<DeviceRule>> SaveDeviceRuleAsync(DeviceRule updatedRule);
        Task<TableStorageResponse<DeviceRule>> DeleteDeviceRuleAsync(DeviceRule ruleToDelete);
    }
}
