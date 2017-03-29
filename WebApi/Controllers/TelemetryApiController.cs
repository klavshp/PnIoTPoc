using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Infrastructure.BusinessLogic;
using PnIotPoc.WebApi.Models;
using PnIotPoc.WebApi.Security;

namespace PnIotPoc.WebApi.Controllers
{
    /// <summary>
    /// A WebApiControllerBase-derived class for telemetry-related end points.
    /// </summary>
    [RoutePrefix("api/v1/telemetry")]
    public class TelemetryApiController : WebApiControllerBase
    {
        private const double MaxDeviceSummaryAgeMinutes = 10.0;
        private const int DisplayedHistoryItems = 18;
        private const int MaxDevicesToDisplayOnDashboard = 200;

        private static readonly TimeSpan CautionAlertMaxDelta = TimeSpan.FromMinutes(91.0);
        private static readonly TimeSpan CriticalAlertMaxDelta = TimeSpan.FromMinutes(11.0);

        private readonly IAlertsLogic _alertsLogic;
        private readonly IDeviceLogic _deviceLogic;
        private readonly IDeviceTelemetryLogic _deviceTelemetryLogic;
        private readonly IConfigurationProvider _configProvider;

        /// <summary>
        /// Initializes a new instance of the TelemetryApiController class.
        /// </summary>
        /// <param name="deviceTelemetryLogic">
        /// The IDeviceRegistryListLogic implementation that the new instance
        /// will use.
        /// </param>
        /// <param name="alertsLogic">
        /// The IAlertsLogic implementation that the new instance will use.
        /// </param>
        /// <param name="deviceLogic">
        /// The IDeviceLogic implementation that the new instance will use.
        /// </param>
        /// <param name="configProvider"></param>
        public TelemetryApiController(IDeviceTelemetryLogic deviceTelemetryLogic, IAlertsLogic alertsLogic, IDeviceLogic deviceLogic, IConfigurationProvider configProvider)
        {
            if (deviceTelemetryLogic == null)
            {
                throw new ArgumentNullException(nameof(deviceTelemetryLogic));
            }

            if (alertsLogic == null)
            {
                throw new ArgumentNullException(nameof(alertsLogic));
            }

            if (deviceLogic == null)
            {
                throw new ArgumentNullException(nameof(deviceLogic));
            }

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _deviceTelemetryLogic = deviceTelemetryLogic;
            _alertsLogic = alertsLogic;
            _deviceLogic = deviceLogic;
            _configProvider = configProvider;
        }

        [HttpGet]
        [Route("list")]
//        [WebApiRequirePermission(Permission.ViewTelemetry)]
//        public async Task<HttpResponseMessage> GetDeviceTelemetryAsync(string deviceId, DateTime minTime)
        public async Task<HttpResponseMessage> GetDeviceTelemetryAsync()
        {
            // TESTING
            var deviceId = "SampleDevice001_972";
            var minTime = DateTime.Now.AddHours(-1);

            Func<Task<DeviceTelemetryModel[]>> getTelemetry =
                async () =>
                {
                    DeviceModel device = await _deviceLogic.GetDeviceAsync(deviceId);

                    IList<DeviceTelemetryFieldModel> telemetryFields = null;

                    try
                    {
                        telemetryFields = _deviceLogic.ExtractTelemetry(device);
                    }
                    catch
                    {
                        HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent($"Device {deviceId} has an invalid Telemetry specification on its DeviceInfo")
                        };
                        throw new HttpResponseException(message);
                    }

                    var telemetryModels = await _deviceTelemetryLogic.LoadLatestDeviceTelemetryAsync(deviceId, telemetryFields, minTime);

                    return telemetryModels.ToArray();
                };

            return await GetServiceResponseAsync<DeviceTelemetryModel[]>(getTelemetry, false);
        }
    }
}
