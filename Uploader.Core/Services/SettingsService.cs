using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uploader.Core.Mappings;
using Uploader.Core.Models.Result;
using Uploader.Core.Repositories;
using Uploader.Core.Repositories.Interfaces;
using Uploader.Core.Services.Interfaces;
using Entity = Uploader.Infrastructure.Data.Entities;
using Model = Uploader.Core.Models;

namespace Uploader.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IAsyncSettingsRepository settingsRepository;

        private readonly IAsyncRepository<Entity.UploaderFileExtension> fileExtensionsRepository;

        private readonly IAsyncRepository<Entity.EnabledUploaderSettings> enabledSettingsRepository;

        public SettingsService(
            IAsyncSettingsRepository settingsRepository,
            IAsyncRepository<Entity.EnabledUploaderSettings> enabledSettingsRepository,
            IAsyncRepository<Entity.UploaderFileExtension> fileExtensionsRepository)
        {
            this.settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            this.enabledSettingsRepository = enabledSettingsRepository ?? throw new ArgumentNullException(nameof(enabledSettingsRepository));
            this.fileExtensionsRepository = fileExtensionsRepository ?? throw new ArgumentNullException(nameof(fileExtensionsRepository));
        }

        public async Task<Result<List<Model.UploaderSettings>>> GetUploaderSettingsAsync()
        {
            try
            {
                var settings = await settingsRepository
                    .GetAllAsync(includes: source => source.Include(x => x.AllowedFileExtensions).Include(x => x.EnabledUserSettings))
                    .ConfigureAwait(false);

                var settingsDto = settings.Adapt<List<Model.UploaderSettings>>(UploaderMappings.UploaderSettingsAdapterConfig);

                return new SuccessResult<List<Model.UploaderSettings>>(settingsDto);
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<List<Model.UploaderSettings>>(ex.Message);
            }
        }

        public async Task<Result<List<Model.UploaderFileExtension>>> GetUploaderFileExtensionAsync()
        {
            try
            {
                var fileExtensions = await fileExtensionsRepository.GetAllAsync()
                    .ConfigureAwait(false);

                var fileExtensionsDto = fileExtensions.Adapt<List<Model.UploaderFileExtension>>(UploaderMappings.UploaderSettingsAdapterConfig);

                return new SuccessResult<List<Model.UploaderFileExtension>>(fileExtensionsDto);
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<List<Model.UploaderFileExtension>>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderSettings>> EnableUploaderSettingsAsync(int id)
        {
            try
            {
                var setting = await settingsRepository
                    .GetByIdAsync(id)
                    .ConfigureAwait(false);

                if (setting is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"Setting with id: {id} could not be found");
                }

                var enabledUploaderSettings = await enabledSettingsRepository
                    .GetAllAsync()
                    .ConfigureAwait(false);

                var currentEnabled = enabledUploaderSettings.FirstOrDefault();

                if (currentEnabled is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"{nameof(currentEnabled)} could not be found");
                }

                currentEnabled.EnabledSettingsId = setting.Id;

                await enabledSettingsRepository
                    .UpdateAsync(currentEnabled)
                    .ConfigureAwait(false);

                return new SuccessResult<Model.UploaderSettings>(currentEnabled.EnabledSettings.Adapt<Model.UploaderSettings>(UploaderMappings.UploaderSettingsAdapterConfig));
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderSettings>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderSettings>> AddSettingsAsync(Model.UploaderSettings settings)
        {
            try
            {
                if (settings is null)
                {
                    return new InvalidResult<Model.UploaderSettings>($"Invalid settings");
                }

                var result = await settingsRepository.CreateAsync(
                    new Entity.UploaderSettings
                    {
                        MaxFileSize = settings.MaxFileSize,
                        MaxHeight = settings.MaxHeight,
                        MaxWidth = settings.MaxWidth,
                        MinHeight = settings.MinHeight,
                        MinWidth = settings.MinWidth,
                    })
                    .ConfigureAwait(false);

                return new SuccessResult<Model.UploaderSettings>(result.Adapt<Model.UploaderSettings>(UploaderMappings.UploaderSettingsAdapterConfig));

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderSettings>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderSettings>> RemoveSettingsAsync(int id)
        {
            try
            {
                var setting = await settingsRepository
                    .GetByIdAsync(id)
                    .ConfigureAwait(false);

                if (setting is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"Setting with id: {id} could not be found");
                }

                var isEnabled = await enabledSettingsRepository.GetByWhere(x => x.EnabledSettingsId == id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (isEnabled != null)
                {
                    return new InvalidResult<Model.UploaderSettings>($"Cannot remove enabled settings");
                }

                await settingsRepository.DeleteAsync(setting);

                return new SuccessResult<Model.UploaderSettings>();

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderSettings>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderFileExtension>> AddFileExtesionAsync(Model.UploaderFileExtension fileExtension)
        {
            try
            {
                if (fileExtension is null)
                {
                    return new InvalidResult<Model.UploaderFileExtension>($"Invalid file extension");
                }

                var result = await fileExtensionsRepository.CreateAsync(
                    new Entity.UploaderFileExtension
                    {
                        FileExtension = fileExtension.FileExtension
                    })
                    .ConfigureAwait(false);

                return new SuccessResult<Model.UploaderFileExtension>(result.Adapt<Model.UploaderFileExtension>());

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderFileExtension>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderSettings>> AddFileExtensionToSettingAsync(int settingsId, int fileExtensionId)
        {
            try
            {
                var setting = await settingsRepository
                    .GetByIdAsync(settingsId)
                    .ConfigureAwait(false);

                if (setting is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"Setting with id: {settingsId} could not be found");
                }

                var fileExtension = await fileExtensionsRepository
                    .GetByIdAsync(fileExtensionId)
                    .ConfigureAwait(false);

                if (fileExtension is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"File extension with id: {fileExtensionId} could not be found");
                }

                var settingAllowedExtensions = await settingsRepository
                    .GetByWhere(x => x.Id == settingsId)
                    .Include(x => x.AllowedFileExtensions)
                    .Select(x => x.AllowedFileExtensions)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (settingAllowedExtensions != null && settingAllowedExtensions.Any(x => x.Id == fileExtensionId))
                {
                    return new InvalidResult<Model.UploaderSettings>($"File extension with id: {fileExtensionId} already exists for settings with id: {settingsId}");
                }

                setting.AllowedFileExtensions.Add(fileExtension);

                await settingsRepository
                    .UpdateAsync(setting)
                    .ConfigureAwait(false);

                return new SuccessResult<Model.UploaderSettings>(setting.Adapt<Model.UploaderSettings>(UploaderMappings.UploaderSettingsAdapterConfig));

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderSettings>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderSettings>> RemoveFileExtensionFromSettings(int settingsId, int fileExtensionId)
        {
            try
            {
                var setting = await settingsRepository
                    .GetByWhere(x => x.Id == settingsId)
                    .Include(x => x.AllowedFileExtensions)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (setting is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"Setting with id: {settingsId} could not be found");
                }

                var fileExtension = await fileExtensionsRepository
                    .GetByIdAsync(fileExtensionId)
                    .ConfigureAwait(false);

                if (fileExtension is null)
                {
                    return new NotFoundResult<Model.UploaderSettings>($"File extension with id: {fileExtensionId} could not be found");
                }

                if (setting.AllowedFileExtensions != null && !setting.AllowedFileExtensions.Any(x => x.Id == fileExtensionId))
                {
                    return new InvalidResult<Model.UploaderSettings>($"File extension with id: {fileExtensionId} does not exists for settings with id: {settingsId}");
                }

                await settingsRepository.RemoveExtensionFromSettingsAsync(setting, fileExtension);

                return new SuccessResult<Model.UploaderSettings>(setting.Adapt<Model.UploaderSettings>(UploaderMappings.UploaderSettingsAdapterConfig));

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderSettings>(ex.Message);
            }
        }
    }
}
