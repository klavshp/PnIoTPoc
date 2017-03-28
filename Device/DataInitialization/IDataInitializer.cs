namespace PnIotPoc.Device.DataInitialization
{
    /// <summary>
    /// Represents component to create initial data for the system
    /// </summary>
    public interface IDataInitializer
    {
        void CreateInitialDataIfNeeded();
    }
}
