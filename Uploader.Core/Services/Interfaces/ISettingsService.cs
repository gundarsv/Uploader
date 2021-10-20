using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Core.Models;
using Uploader.Core.Models.Result;

namespace Uploader.Core.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<Result<UploaderSettings>> EnableUploaderSettingsAsync(int id);
        Task<Result<List<UploaderSettings>>> GetUploaderSettingsAsync();
        Task<Result<UploaderSettings>> AddSettingsAsync(UploaderSettings settings);
        Task<Result<UploaderSettings>> AddFileExtensionToSettingAsync(int settingsId, int fileExtensionId);
        Task<Result<UploaderFileExtension>> AddFileExtesionAsync(UploaderFileExtension fileExtension);
        Task<Result<UploaderSettings>> RemoveSettingsAsync(int id);
        Task<Result<List<UploaderFileExtension>>> GetUploaderFileExtensionAsync();
        Task<Result<UploaderSettings>> RemoveFileExtensionFromSettings(int settingsId, int fileExtensionId);
    }
}