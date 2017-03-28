using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    /// <summary>
    /// Logic class for retrieving, manipulating and persisting Device Rules
    /// </summary>
    public interface IDeviceRulesLogic
    {
        Task<List<DeviceRule>> GetAllRulesAsync();
        Task<DeviceRule> GetDeviceRuleOrDefaultAsync(string deviceId, string ruleId);
        Task<DeviceRule> GetDeviceRuleAsync(string deviceId, string ruleId);
        Task<TableStorageResponse<DeviceRule>> SaveDeviceRuleAsync(DeviceRule updatedRule);
        Task<DeviceRule> GetNewRuleAsync(string deviceId);
        Task<TableStorageResponse<DeviceRule>> UpdateDeviceRuleEnabledStateAsync(string deviceId, string ruleId, bool enabled);
        Task<Dictionary<string, List<string>>> GetAvailableFieldsForDeviceRuleAsync(string deviceId, string ruleId);
        Task<bool> CanNewRuleBeCreatedForDeviceAsync(string deviceId);
        Task BootstrapDefaultRulesAsync(List<string> existingDeviceIds);
        Task<TableStorageResponse<DeviceRule>> DeleteDeviceRuleAsync(string deviceId, string ruleId);
        Task<bool> RemoveAllRulesForDeviceAsync(string deviceId);
    }
}
