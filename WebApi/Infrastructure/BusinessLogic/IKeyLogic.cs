using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface IKeyLogic
    {
        Task<SecurityKeys> GetKeysAsync();
    }
}
