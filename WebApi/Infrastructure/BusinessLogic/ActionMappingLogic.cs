using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Infrastructure.Repository;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public class ActionMappingLogic : IActionMappingLogic
    {
        private readonly IActionMappingRepository _actionMappingRepository;
        private readonly IDeviceRulesRepository _rulesRepository;

        private const string RuleOutputAlarmTemp = "AlarmTemp";
        private const string RuleOutputAlarmHumidity = "AlarmHumidity";

        private readonly List<string> _availableRuleOutputs = new List<string>()
        {
            RuleOutputAlarmTemp,
            RuleOutputAlarmHumidity
        };

        public ActionMappingLogic(IActionMappingRepository actionMappingRepository, IDeviceRulesRepository rulesRepository)
        {
            _actionMappingRepository = actionMappingRepository;
            _rulesRepository = rulesRepository;
        }

        public async Task<bool> IsInitializationNeededAsync()
        {
            var existingMappings = await _actionMappingRepository.GetAllMappingsAsync();

            if (existingMappings.Count > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates initial, default action mapping data.
        /// </summary>
        /// <returns>True if needed to create data, false otherwise</returns>
        public async Task<bool> InitializeDataIfNecessaryAsync()
        {
            var existingMappings = await _actionMappingRepository.GetAllMappingsAsync();

            if (existingMappings.Count > 0)
            {
                return false;
            }

            var am1 = new ActionMapping()
            {
                RuleOutput = RuleOutputAlarmTemp,
                ActionId = "Send Message"
            };

            await _actionMappingRepository.SaveMappingAsync(am1);

            var am2 = new ActionMapping()
            {
                RuleOutput = RuleOutputAlarmHumidity,
                ActionId = "Raise Alarm"
            };

            await _actionMappingRepository.SaveMappingAsync(am2);

            return true;
        }

        public async Task<List<ActionMappingExtended>> GetAllMappingsAsync()
        {
            // call into both repositories (rules and actions) and then join data

            // call these in parallel
            var rawMappingsTask = _actionMappingRepository.GetAllMappingsAsync();
            var rulesTask = _rulesRepository.GetAllRulesAsync();

            List<ActionMapping> mappings = await rawMappingsTask;
            List<DeviceRule> rules = await rulesTask;

            var results = new List<ActionMappingExtended>();
            foreach (var mapping in mappings)
            {
                var mappingExtended = new ActionMappingExtended();
                mappingExtended.RuleOutput = mapping.RuleOutput;
                mappingExtended.ActionId = mapping.ActionId;

                mappingExtended.NumberOfDevices = rules.Where(r => r.RuleOutput == mapping.RuleOutput).Count();

                // TODO: add parameters? (likely hardcode in switch)

                results.Add(mappingExtended);
            }

            return results;
        }

        public async Task SaveMappingAsync(ActionMapping action)
        {
            await _actionMappingRepository.SaveMappingAsync(action);
        }

        public async Task<string> GetActionIdFromRuleOutputAsync(string ruleOutput)
        {
            var mappings = await _actionMappingRepository.GetAllMappingsAsync();

            var correctMapping = mappings.SingleOrDefault(m => m.RuleOutput == ruleOutput);

            if (correctMapping == null)
            {
                return "";
            }

            return correctMapping.ActionId;
        }

        public async Task<List<string>> GetAvailableRuleOutputsAsync()
        {
            return await Task.Run(() => _availableRuleOutputs);
        }
    }
}