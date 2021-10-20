using System.Threading.Tasks;
using Uploader.Core.Repositories.Interfaces;
using Uploader.Infrastructure.Data.Entities;

namespace Uploader.Core.Repositories
{
    public interface IAsyncSettingsRepository : IAsyncRepository<UploaderSettings>
    {
        Task RemoveExtensionFromSettingsAsync(UploaderSettings settings, UploaderFileExtension uploaderFileExtension);
    }
}