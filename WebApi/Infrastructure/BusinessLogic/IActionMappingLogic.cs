using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface IActionMappingLogic
    {
        Task<bool> IsInitializationNeededAsync();
        Task<bool> InitializeDataIfNecessaryAsync();
        Task<List<ActionMappingExtended>> GetAllMappingsAsync();
        Task<string> GetActionIdFromRuleOutputAsync(string ruleOutput);
        Task SaveMappingAsync(ActionMapping action);
        Task<List<string>> GetAvailableRuleOutputsAsync();
    }
}
