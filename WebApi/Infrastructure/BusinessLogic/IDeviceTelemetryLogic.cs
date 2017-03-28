using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface IDeviceTelemetryLogic
    {
        Task<IEnumerable<DeviceTelemetryModel>> LoadLatestDeviceTelemetryAsync(
            string deviceId,
            IList<DeviceTelemetryFieldModel> telemetryFields,
            DateTime minTime);

        Task<DeviceTelemetrySummaryModel> LoadLatestDeviceTelemetrySummaryAsync(
            string deviceId,
            DateTime? minTime);

        Func<string, DateTime?> ProduceGetLatestDeviceAlertTime(
            IEnumerable<AlertHistoryItemModel> alertHistoryModels);
    }
}
