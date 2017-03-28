using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Infrastructure.Repository;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    /// <summary>
    /// An IAlertsLogic implementation that holds business logic, related to 
    /// Alerts.
    /// </summary>
    public class AlertsLogic : IAlertsLogic
    {
        private readonly IAlertsRepository _alertsRepository;

        /// <summary>
        /// Initializes a new instance of the AlertsLogic class.
        /// </summary>
        /// <param name="alertsRepository">
        /// The IAlertsRepository implementation that the new instance will use.
        /// </param>
        public AlertsLogic(IAlertsRepository alertsRepository)
        {
            if (alertsRepository == null)
            {
                throw new ArgumentNullException(nameof(alertsRepository));
            }

            _alertsRepository = alertsRepository;
        }

        /// <summary>
        /// Loads the latest Device Alert History items.
        /// </summary>
        /// <param name="minTime">
        /// The cutoff time for Device Alert History items that should be returned.
        /// </param>
        /// <param name="minResults">
        /// The minimum number of items that should be returned, if possible, 
        /// after <paramref name="minTime"/> or otherwise.
        /// </param>
        /// <returns>
        /// The latest Device Alert History items.
        /// </returns>
        public async Task<IEnumerable<AlertHistoryItemModel>> LoadLatestAlertHistoryAsync(DateTime minTime, int minResults)
        {
            if (minResults <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minResults),
                    minResults,
                    "minResults must be a positive integer.");
            }

            return await this._alertsRepository.LoadLatestAlertHistoryAsync(minTime, minResults);
        }
    }
}