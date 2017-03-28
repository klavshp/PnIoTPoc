using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.WebApi.Common.Configurations;

namespace PnIotPoc.Device.SimulatorCore.Transport.Factory
{
    public class IotHubTransportFactory : ITransportFactory
    {
        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;

        public IotHubTransportFactory(ILogger logger,
            IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
        }

        public ITransport CreateTransport(IDevice device)
        {
            return new IoTHubTransport(_logger, _configurationProvider, device);
        }
    }
}
