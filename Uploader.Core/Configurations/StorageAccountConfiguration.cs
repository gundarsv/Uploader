using Microsoft.Extensions.Configuration;

namespace Uploader.Core.Configurations
{
    public class StorageAccountConfiguration : ConfigurationBase, IStorageAccountConfiguration
    {
        public StorageAccountConfiguration(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override string SectionName => nameof(StorageAccountConfiguration);

        public string StorageAccountConnection => GetString(nameof(StorageAccountConnection));

        public string ContainerName => GetString(nameof(ContainerName));
    }
}
