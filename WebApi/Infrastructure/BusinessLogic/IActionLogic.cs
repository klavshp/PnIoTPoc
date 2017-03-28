using System.Collections.Generic;
using System.Threading.Tasks;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface IActionLogic
    {
        Task<List<string>> GetAllActionIdsAsync();

        Task<bool> ExecuteLogicAppAsync(string actionId, string deviceId, string measurementName, double measuredValue);
    }
}
