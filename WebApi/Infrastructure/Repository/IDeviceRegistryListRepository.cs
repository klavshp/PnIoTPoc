using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public interface IDeviceRegistryListRepository
    {
        /// <summary>
        /// Gets a list of type Device depending on search parameters, sort column, sort direction,
        /// starting point, page size, and filters.
        /// </summary>
        /// <param name="query">The device query.</param>
        /// <returns></returns>
        Task<DeviceListQueryResult> GetDeviceList(DeviceListQuery query);
    }
}
