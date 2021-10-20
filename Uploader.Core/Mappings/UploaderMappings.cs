using Mapster;
using Entity = Uploader.Infrastructure.Data.Entities;
using Model = Uploader.Core.Models;

namespace Uploader.Core.Mappings
{
    public static class UploaderMappings
    {
        public static TypeAdapterConfig UploaderSettingsAdapterConfig => new TypeAdapterConfig()
                .NewConfig<Entity.UploaderSettings, Model.UploaderSettings>()
                .Map(x => x.IsEnabled, y => y.EnabledUserSettings != null)
                .Config;

        public static TypeAdapterConfig UploaderFileAdapterConfig => new TypeAdapterConfig()
                .NewConfig<Entity.UploaderFile, Model.UploaderFile>()
                .Map(x => x.Extension, y => y.Extension.FileExtension)
                .Config;
    }
}
