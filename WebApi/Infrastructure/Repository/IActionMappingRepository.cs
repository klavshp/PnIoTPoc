using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    /// <summary>
    /// Defines the interface to the actions repository, which stores the 
    /// mappings of ActionId values to logic app actions.
    /// </summary>
    public interface IActionMappingRepository
    {
        Task<List<ActionMapping>> GetAllMappingsAsync();
        Task SaveMappingAsync(ActionMapping m);
    }
}
