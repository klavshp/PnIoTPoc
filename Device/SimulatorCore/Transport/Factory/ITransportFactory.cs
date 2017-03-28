using PnIotPoc.Device.SimulatorCore.Devices;

namespace PnIotPoc.Device.SimulatorCore.Transport.Factory
{
    public interface ITransportFactory
    {
        ITransport CreateTransport(IDevice device);
    }
}
