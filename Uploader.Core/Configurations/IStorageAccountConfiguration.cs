namespace Uploader.Core.Configurations
{
    public interface IStorageAccountConfiguration
    {
        string StorageAccountConnection { get; }
        string ContainerName { get; }
    }
}