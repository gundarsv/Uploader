using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Core.Extensions;
using Uploader.Core.Models;
using Uploader.Core.Services.Interfaces;

namespace Uploader.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UploaderFile>), 200)]
        [ProducesResponseType(typeof(PaginatedList<UploaderFile>), 200)]
        public async Task<IActionResult> GetUploaderFiles(int? pageNumber, int? pageSize)
        {
            if (pageSize is null || pageNumber is null)
            {
                return this.FromResult(await fileService.GetUploaderFilesAsync());
            }

            var result = await fileService.GetUploaderFilesPaginatedAsync(pageNumber.Value, pageSize.Value);

            return this.FromResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UploaderFile), 200)]
        public async Task<IActionResult> Upload([FromForm] UploadUploaderFile uploaderFile, [FromForm] string comment)
        {
            var result = await fileService.AddFileAsync(uploaderFile);

            return this.FromResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var result = await fileService.GetFileAsync(id);

            return this.FromResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UploaderFile), 200)]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await fileService.RemoveFileAsync(id);

            return this.FromResult(result);
        }
    }
}
