using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Core.Models;
using Uploader.Core.Models.Result;

namespace Uploader.Core.Services.Interfaces
{
    public interface IFileService
    {
        Task<Result<UploaderFile>> AddFileAsync(UploadUploaderFile uploaderFile);
        Task<Result<FileResponse>> GetFileAsync(int uploaderFileId);
        Task<Result<List<UploaderFile>>> GetUploaderFilesAsync();
        Task<Result<PaginatedList<UploaderFile>>> GetUploaderFilesPaginatedAsync(int pageNumber, int pageSize);
        Task<Result<UploaderFile>> RemoveFileAsync(int id);
    }
}