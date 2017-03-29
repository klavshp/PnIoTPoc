using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Infrastructure.Models;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Infrastructure.Repository
{
    public class DeviceTelemetryRepository : IDeviceTelemetryRepository
    {
        private readonly string _telemetryDataPrefix;
        private readonly string _telemetrySummaryPrefix;
        private readonly IBlobStorageClient _blobStorageManager;

        /// <summary>
        /// Initializes a new instance of the DeviceTelemetryRepository class.
        /// </summary>
        /// <param name="configProvider">
        /// The IConfigurationProvider implementation with which to initialize 
        /// the new instance.
        /// </param>
        /// <param name="blobStorageClientFactory"></param>
        public DeviceTelemetryRepository(IConfigurationProvider configProvider, IBlobStorageClientFactory blobStorageClientFactory)
        {
            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            var telemetryContainerName = configProvider.GetConfigurationSettingValue("TelemetryStoreContainerName");
            _telemetryDataPrefix = configProvider.GetConfigurationSettingValue("TelemetryDataPrefix");
            var telemetryStoreConnectionString = configProvider.GetConfigurationSettingValue("device.StorageConnectionString");
            _telemetrySummaryPrefix = configProvider.GetConfigurationSettingValue("TelemetrySummaryPrefix");
            _blobStorageManager = blobStorageClientFactory.CreateClient(telemetryStoreConnectionString, telemetryContainerName);
        }

        /// <summary>
        /// Loads the most recent Device telemetry.
        /// </summary>
        /// <param name="deviceId">
        /// The ID of the Device for which telemetry should be returned.
        /// </param>
        /// <param name="telemetryFields"></param>
        /// The Fields contained in the telemetry
        /// <param name="minTime">
        /// The minimum time of record of the telemetry that should be returned.
        /// </param>
        /// <returns>
        /// Telemetry for the Device specified by deviceId, inclusively since 
        /// minTime.
        /// </returns>
        public async Task<IEnumerable<DeviceTelemetryModel>> LoadLatestDeviceTelemetryAsync(string deviceId, IList<DeviceTelemetryFieldModel> telemetryFields, DateTime minTime)
        {
            IEnumerable<DeviceTelemetryModel> result = new DeviceTelemetryModel[0];

            var telemetryBlobReader = await _blobStorageManager.GetReader(_telemetryDataPrefix, minTime);
            foreach (var telemetryStream in telemetryBlobReader)
            {
                IEnumerable<DeviceTelemetryModel> blobModels;
                try
                {
                    blobModels = LoadBlobTelemetryModels(telemetryStream.Data, telemetryFields);
                }
                catch
                {
                    continue;
                }

                if (blobModels == null)
                {
                    break;
                }

                var preFilterCount = blobModels.Count();

                blobModels =
                    blobModels.Where(
                        t =>
                            (t != null) &&
                            t.Timestamp.HasValue &&
                            t.Timestamp.Value >= minTime);

                if (preFilterCount == 0)
                {
                    break;
                }

                result = result.Concat(blobModels);
            }

            if (!string.IsNullOrEmpty(deviceId))
            {
                result = result.Where(t => t.DeviceId == deviceId);
            }

            return result;
        }

        private static List<DeviceTelemetryModel> LoadBlobTelemetryModels(Stream stream, IList<DeviceTelemetryFieldModel> telemetryFields)
        {
            Debug.Assert(stream != null, "stream is a null reference.");

            List<DeviceTelemetryModel> models = new List<DeviceTelemetryModel>();

            TextReader reader = null;
            try
            {
                stream.Position = 0;
                reader = new StreamReader(stream);

                
                var strdicts = ParsingHelper.ParseCsv(reader).ToDictionaries();
                foreach (var strdict in strdicts)
                {
                    var model = new DeviceTelemetryModel();

                    string str;
                    if (strdict.TryGetValue("deviceid", out str))
                    {
                        model.DeviceId = str;
                    }

                    model.Timestamp = DateTime.Parse(
                        strdict["eventenqueuedutctime"],
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AllowWhiteSpaces);

                    IEnumerable<DeviceTelemetryFieldModel> fields;

                    if (telemetryFields != null && telemetryFields.Count > 0)
                    {
                        fields = telemetryFields;
                    }
                    else
                    {
                        List<string> reservedColumns = new List<string>
                        {
                            "DeviceId",
                            "EventEnqueuedUtcTime",
                            "EventProcessedUtcTime",
                            "IoTHub",
                            "PartitionId",
                            "partitionid"
                        };

                        fields = strdict.Keys
                            .Where((key) => !reservedColumns.Contains(key))
                            .Select((name) => new DeviceTelemetryFieldModel
                            {
                                Name = name,
                                Type = "double"
                            });
                    }

                    foreach (var field in fields)
                    {
                        if (strdict.TryGetValue(field.Name, out str))
                        {
                            switch (field.Type.ToLowerInvariant())
                            {
                                case "int":
                                case "int16":
                                case "int32":
                                case "int64":
                                case "sbyte":
                                case "byte":
                                    int intValue;
                                    if (
                                        int.TryParse(
                                            str,
                                            NumberStyles.Integer,
                                            CultureInfo.InvariantCulture,
                                            out intValue) &&
                                        !model.Values.ContainsKey(field.Name))
                                    {
                                        model.Values.Add(field.Name, intValue);
                                    }
                                    break;

                                case "double":
                                case "decimal":
                                case "single":
                                    double dblValue;
                                    if (
                                        double.TryParse(
                                            str,
                                            NumberStyles.Float,
                                            CultureInfo.InvariantCulture,
                                            out dblValue) &&
                                        !model.Values.ContainsKey(field.Name))
                                    {
                                        model.Values.Add(field.Name, dblValue);
                                    }
                                    break;
                            }
                        }
                    }

                    models.Add(model);
                }
            }
            finally
            {
                IDisposable disp;

                if ((disp = stream) != null)
                {
                    disp.Dispose();
                }

                if ((disp = reader) != null)
                {
                    disp.Dispose();
                }
            }

            return models;
        }

        public Task<DeviceTelemetrySummaryModel> LoadLatestDeviceTelemetrySummaryAsync(string deviceId, DateTime? minTime)
        {
            throw new NotImplementedException();
        }
    }
}