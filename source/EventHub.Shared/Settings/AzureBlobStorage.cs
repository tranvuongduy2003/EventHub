namespace EventHub.Shared.Settings;

public class AzureBlobStorage
{
    public string StorageAccount { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public string ConnectionString { get; set; } = string.Empty;

    public string ContainerName { get; set; } = string.Empty;
}