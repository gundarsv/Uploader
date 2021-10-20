using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Core.Extensions;
using Uploader.Core.Models;
using Uploader.Core.Services.Interfaces;

namespace Uploader.Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UploaderSettings>), 200)]
        public async Task<IActionResult> GetSettings()
        {
            var result = await settingsService.GetUploaderSettingsAsync();

            return this.FromResult(result);
        }

        [HttpGet("FileExtensions")]
        [ProducesResponseType(typeof(List<UploaderFileExtension>), 200)]
        public async Task<IActionResult> GetFileExtensions()
        {
            var result = await settingsService.GetUploaderFileExtensionAsync();

            return this.FromResult(result);
        }

        [HttpPut("{id:int}/enable")]
        [ProducesResponseType(typeof(UploaderSettings), 200)]
        public async Task<IActionResult> EnableSettings(int id)
        {
            var result = await settingsService.EnableUploaderSettingsAsync(id);

            return this.FromResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UploaderSettings), 200)]
        public async Task<IActionResult> AddSettings(UploaderSettings settings)
        {
            var result = await settingsService.AddSettingsAsync(settings);

            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(UploaderSettings), 200)]
        public async Task<IActionResult> RemoveSettings(int id)
        {
            var result = await settingsService.RemoveSettingsAsync(id);

            return this.FromResult(result);
        }

        [HttpPost("FileExtensions")]
        [ProducesResponseType(typeof(UploaderFileExtension), 200)]
        public async Task<IActionResult> AddFileExtension(UploaderFileExtension fileExtension)
        {
            var result = await settingsService.AddFileExtesionAsync(fileExtension);

            return this.FromResult(result);
        }

        [HttpPost("{settingsId:int}/FileExtensions/{fileExtensionId:int}")]
        [ProducesResponseType(typeof(UploaderSettings), 200)]
        public async Task<IActionResult> AddFileExtensionToSettings(int settingsId, int fileExtensionId)
        {
            var result = await settingsService.AddFileExtensionToSettingAsync(settingsId, fileExtensionId);

            return this.FromResult(result);
        }

        [HttpDelete("{settingsId:int}/FileExtensions/{fileExtensionId:int}")]
        [ProducesResponseType(typeof(UploaderSettings), 200)]
        public async Task<IActionResult> RemoveFileExtensionFromSettings(int settingsId, int fileExtensionId)
        {
            var result = await settingsService.RemoveFileExtensionFromSettings(settingsId, fileExtensionId);

            return this.FromResult(result);
        }
    }
}
