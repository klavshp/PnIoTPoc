using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnIotPoc.WebApi.Common.Helpers
{
    public interface IBlobStorageClientFactory
    {
        IBlobStorageClient CreateClient(string storageConnectionString, string containerName);
    }
}
