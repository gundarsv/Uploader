using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Uploader.Infrastructure.Data.Contexts;
using Uploader.Infrastructure.Data.Entities;

namespace Uploader.Core.Repositories
{
    public class AsyncSettingsRepository : AsyncRepository<UploaderSettings>, IAsyncSettingsRepository
    {
        private readonly UploaderContext uploaderContext;
        public AsyncSettingsRepository(UploaderContext uploaderContext)
            : base(uploaderContext)
        {
            this.uploaderContext = uploaderContext;
        }

        public async Task RemoveExtensionFromSettingsAsync(UploaderSettings settings, UploaderFileExtension uploaderFileExtension)
        {
            var setting = await uploaderContext.UploaderSettings
                .Include(s => s.AllowedFileExtensions)
                .SingleOrDefaultAsync(s => s.Id == settings.Id);

            var fileExtension = await uploaderContext.UploaderFileExtensions
                .Include(x => x.UploaderSettings)
                .SingleOrDefaultAsync(e => e.Id == uploaderFileExtension.Id);

            setting.AllowedFileExtensions.Remove(uploaderFileExtension);
            fileExtension.UploaderSettings.Remove(setting);

            await uploaderContext.SaveChangesAsync();
        }

    }
}
