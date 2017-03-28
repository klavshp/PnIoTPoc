namespace PnIotPoc.WebApi.Common.Helpers
{
    public interface IAzureTableStorageClientFactory
    {
        IAzureTableStorageClient CreateClient(string storageConnectionString, string tableName);
    }
}
