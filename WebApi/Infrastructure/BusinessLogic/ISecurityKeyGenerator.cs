using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface ISecurityKeyGenerator
    {
        /// <summary>
        /// Creates a random security key pair
        /// </summary>
        /// <returns>Populated SecurityKeys object</returns>
        SecurityKeys CreateRandomKeys();
    }
}
