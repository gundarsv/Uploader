using Azure.Storage.Blobs.Models;
using ImageMagick;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uploader.Core.Clients;
using Uploader.Core.Helpers;
using Uploader.Core.Mappings;
using Uploader.Core.Models.Result;
using Uploader.Core.Repositories.Interfaces;
using Uploader.Core.Services.Interfaces;
using Entity = Uploader.Infrastructure.Data.Entities;
using Model = Uploader.Core.Models;

namespace Uploader.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IAsyncRepository<Entity.UploaderFileExtension> fileExtensionRepository;

        private readonly IAsyncRepository<Entity.EnabledUploaderSettings> enabledUploaderSettingsRepository;

        private readonly IAsyncRepository<Entity.UploaderFile> uploaderFileRepository;

        private readonly AzureStorageBlobClient azureStorageBlobClient;

        public FileService(
            IAsyncRepository<Entity.EnabledUploaderSettings> enabledUploaderSettingsRepository,
            AzureStorageBlobClient azureStorageBlobClient,
            IAsyncRepository<Entity.UploaderFileExtension> fileExtensionRepository,
            IAsyncRepository<Entity.UploaderFile> uploaderFileRepository)
        {
            this.enabledUploaderSettingsRepository = enabledUploaderSettingsRepository ?? throw new ArgumentNullException(nameof(enabledUploaderSettingsRepository));
            this.azureStorageBlobClient = azureStorageBlobClient ?? throw new ArgumentNullException(nameof(azureStorageBlobClient));
            this.fileExtensionRepository = fileExtensionRepository ?? throw new ArgumentNullException(nameof(fileExtensionRepository));
            this.uploaderFileRepository = uploaderFileRepository ?? throw new ArgumentNullException(nameof(uploaderFileRepository));
        }

        public async Task<Result<Model.UploaderFile>> AddFileAsync(Model.UploadUploaderFile uploaderFile)
        {
            try
            {
                if (uploaderFile is null || uploaderFile?.File is null || uploaderFile?.FileName is null)
                {
                    return new InvalidResult<Model.UploaderFile>("Please upload file and specify file name");
                }

                var fileExtension = Path.GetExtension(uploaderFile.File.FileName).Replace(".", "");

                var uploaderFileExtension = await fileExtensionRepository
                    .GetByWhere(x => x.FileExtension == fileExtension.ToLower())
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                var enabledUploaderSettings = (await enabledUploaderSettingsRepository
                    .GetAllAsync(includes: source => source.Include(x => x.EnabledSettings).ThenInclude(x => x.AllowedFileExtensions))
                    .ConfigureAwait(false))
                    .SingleOrDefault();

                if (!TryValidateFile(uploaderFile.File.Length, fileExtension, enabledUploaderSettings, uploaderFileExtension, out var validationErrors))
                {
                    return new InvalidResult<Model.UploaderFile>(validationErrors);
                }

                var isImage = FileHelper.IsImage(fileExtension);

                if (!isImage)
                {
                    return await UploadFileAsync(uploaderFile.File, uploaderFileExtension, uploaderFile.FileName, uploaderFile.Comment)
                        .ConfigureAwait(false);
                }

                return await UploadImageAsync(uploaderFile.File, uploaderFileExtension, enabledUploaderSettings, uploaderFile.FileName, uploaderFile.Comment)
                    .ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderFile>(ex.Message);
            }
        }

        public async Task<Result<Model.FileResponse>> GetFileAsync(int uploaderFileId)
        {
            try
            {
                var uploaderFile = await uploaderFileRepository
                    .GetByWhere(x => x.Id == uploaderFileId)
                    .Include(x => x.Extension)
                    .FirstOrDefaultAsync();

                if (uploaderFile is null)
                {
                    return new NotFoundResult<Model.FileResponse>($"File with id: {uploaderFileId} was not found");
                }

                var blob = azureStorageBlobClient.Client.GetBlobClient($"{uploaderFile.BlobName}.{uploaderFile.Extension.FileExtension}");

                Stream stream = new MemoryStream();

                var downloadResult = await blob.DownloadToAsync(stream)
                    .ConfigureAwait(false);

                // TODO: Fix this
                stream.Position = 0;

                return new FileResult(
                    new Model.FileResponse {
                        Stream = stream,
                        ContentType = downloadResult.Headers.ContentType,
                        FileName = uploaderFile.FileName 
                    });
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.FileResponse>(ex.Message);
            }
        }

        private async Task<Result<Model.UploaderFile>> UploadFileAsync(IFormFile file, Entity.UploaderFileExtension uploaderFileExtension, string fileName, string comments)
        {
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            var uploadResult = await UploadAsync(file.OpenReadStream(), uploaderFileExtension, fileName, httpHeaders, comments)
                .ConfigureAwait(false);

            var result = await uploaderFileRepository.CreateAsync(uploadResult)
                .ConfigureAwait(false);

            var mappedResult = result.Adapt<Model.UploaderFile>(UploaderMappings.UploaderFileAdapterConfig);

            return new SuccessResult<Model.UploaderFile>(mappedResult);
        }

        private async Task<Result<Model.UploaderFile>> UploadImageAsync(IFormFile file, Entity.UploaderFileExtension uploaderFileExtension, Entity.EnabledUploaderSettings enabledUploaderSettings, string fileName, string comments)
        {
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            using var image = new MagickImage(file.OpenReadStream());

            if (!TryResizeImage(enabledUploaderSettings, image, out var resizeErrors))
            {
                return new InvalidResult<Model.UploaderFile>(resizeErrors);
            }

            Stream fileStream = new MemoryStream();

            await image.WriteAsync(fileStream)
                .ConfigureAwait(false); ;

            // TODO: Fix this
            fileStream.Position = 0;

            var uploadResult = await UploadAsync(fileStream, uploaderFileExtension, fileName, httpHeaders, comments)
                .ConfigureAwait(false);

            var result = await uploaderFileRepository.CreateAsync(uploadResult)
                .ConfigureAwait(false);

            var mappedResult = result.Adapt<Model.UploaderFile>(UploaderMappings.UploaderFileAdapterConfig);

            return new SuccessResult<Model.UploaderFile>(mappedResult);
        }

        private static bool TryValidateFile(
            long fileSize,
            string fileExtension,
            Entity.EnabledUploaderSettings enabledUploaderSettings,
            Entity.UploaderFileExtension uploaderFileExtensions,
            out List<string> errors)
        {
            var isValid = true;
            errors = new List<string>();

            if (uploaderFileExtensions is null)
            {
                errors.Add($"Unsupported file extension: {fileExtension}");
                isValid = false;
            }

            if (enabledUploaderSettings is null)
            {
                errors.Add("Enabled settings could not be found");
                isValid = false;
            }

            if (!enabledUploaderSettings.EnabledSettings.AllowedFileExtensions.Any(x => x.FileExtension == fileExtension.ToLower()))
            {
                errors.Add($"Unsupported file extension: {fileExtension}");
                isValid = false;
            }

            if (enabledUploaderSettings.EnabledSettings.MaxFileSize < fileSize)
            {
                errors.Add($"Unsupported file size: {fileSize}");
                isValid = false;
            }

            return isValid;
        }

        private static bool TryResizeImage(Entity.EnabledUploaderSettings enabledUploaderSettings, MagickImage image, out List<string> errors)
        {
            var isSuccess = true;
            errors = new List<string>();

            if (enabledUploaderSettings is null)
            {
                errors.Add("Enabled settings could not be found");
                isSuccess = false;
            }

            if (enabledUploaderSettings.EnabledSettings.MinWidth > image.Width)
            {
                errors.Add("Image width is too small");
                isSuccess = false;
            }

            if (enabledUploaderSettings.EnabledSettings.MinHeight > image.Width)
            {
                errors.Add("Image width is too small");
                isSuccess = false;
            }

            if (enabledUploaderSettings.EnabledSettings.MaxHeight < image.Height || enabledUploaderSettings.EnabledSettings.MaxWidth < image.Width)
            {
                try
                {
                    image.AdaptiveResize(enabledUploaderSettings.EnabledSettings.MaxWidth, enabledUploaderSettings.EnabledSettings.MaxHeight);
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                    isSuccess = false;
                }
            }


            return isSuccess;
        }

        private async Task<Entity.UploaderFile> UploadAsync(Stream file, Entity.UploaderFileExtension fileExtension, string fileName, BlobHttpHeaders blobHttpHeaders, string comments)
        {
            await azureStorageBlobClient.Client.CreateIfNotExistsAsync();

            var blobName = Guid.NewGuid().ToString();

            var blob = azureStorageBlobClient.Client.GetBlobClient($"{blobName}.{fileExtension.FileExtension}");

            var uploadResult = await blob.UploadAsync(file, blobHttpHeaders)
                .ConfigureAwait(false);

            if (!(uploadResult.GetRawResponse().Status >= 200 && (uploadResult.GetRawResponse().Status <= 299)))
            {
                throw new Exception("File could not be uploaded");
            }

            var uploaderFile = new Entity.UploaderFile
            {
                BlobName = blobName,
                FileName = fileName,
                ExtensionId = fileExtension.Id,
                Comments = comments
            };

            return uploaderFile;
        }

        public async Task<Result<List<Model.UploaderFile>>> GetUploaderFilesAsync()
        {
            try
            {
                var uploaderFiles = await uploaderFileRepository
                    .GetAllAsync(includes: source => source.Include(x => x.Extension))
                    .ConfigureAwait(false);

                var uploaderFilesDto = uploaderFiles.Adapt<List<Model.UploaderFile>>(UploaderMappings.UploaderFileAdapterConfig);

                return new SuccessResult<List<Model.UploaderFile>>(uploaderFilesDto);
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<List<Model.UploaderFile>>(ex.Message);
            }
        }

        public async Task<Result<Model.PaginatedList<Model.UploaderFile>>> GetUploaderFilesPaginatedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = await uploaderFileRepository
                    .GetAll()
                    .CountAsync()
                    .ConfigureAwait(false);

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                if (totalPages < pageNumber)
                {
                    return new InvalidResult<Model.PaginatedList<Model.UploaderFile>>("Page number can not be more than total pages");
                }

                var uploaderFiles = await uploaderFileRepository
                    .GetAll()
                    .Include(x => x.Extension)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var uploaderFilesDto = uploaderFiles.Adapt<List<Model.UploaderFile>>(UploaderMappings.UploaderFileAdapterConfig);

                var paginatedUploaderFiles = new Model.PaginatedList<Model.UploaderFile>
                {
                    Items = uploaderFilesDto,
                    ItemsCount = uploaderFilesDto.Count,
                    TotalCount = totalCount,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return new SuccessResult<Model.PaginatedList<Model.UploaderFile>>(paginatedUploaderFiles);
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.PaginatedList<Model.UploaderFile>>(ex.Message);
            }
        }

        public async Task<Result<Model.UploaderFile>> RemoveFileAsync(int id)
        {
            try
            {
                var uploaderFile = await uploaderFileRepository
                    .GetByWhere(x => x.Id == id)
                    .Include(x => x.Extension)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (uploaderFile is null)
                {
                    return new NotFoundResult<Model.UploaderFile>($"File with id {id} could not be found");
                }

                var blob = azureStorageBlobClient.Client.GetBlobClient($"{uploaderFile.BlobName}.{uploaderFile.Extension}");

                await blob.DeleteIfExistsAsync().ConfigureAwait(false);

                await uploaderFileRepository
                    .DeleteAsync(uploaderFile)
                    .ConfigureAwait(false);

                return new SuccessResult<Model.UploaderFile>();
            }
            catch (Exception ex)
            {
                return new UnexpectedResult<Model.UploaderFile>(ex.Message);
            }
        }
    }
}
