using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public interface IAlertsRepository
    {
        Task<IEnumerable<AlertHistoryItemModel>> LoadLatestAlertHistoryAsync(DateTime minTime, int minResults);
    }
}
