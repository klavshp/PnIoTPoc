using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public interface IDeviceTelemetryRepository
    {
        Task<IEnumerable<DeviceTelemetryModel>> LoadLatestDeviceTelemetryAsync(
            string deviceId,
            IList<DeviceTelemetryFieldModel> telemetryFields,
            DateTime minTime);

        Task<DeviceTelemetrySummaryModel> LoadLatestDeviceTelemetrySummaryAsync(
            string deviceId,
            DateTime? minTime);
    }
}
