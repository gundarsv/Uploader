using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uploader.Core.Models;
using Uploader.Web.HttpClients;

namespace Uploader.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSettings([FromHeader(Name = "authorization")] string accessToken)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest("api/settings", DataFormat.Json);

            try
            {
                var response = await client.GetAsync<List<UploaderSettings>>(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/enable")]
        public async Task<IActionResult> EnableSettings([FromHeader(Name = "authorization")] string accessToken, int id)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings/{id}/enable", DataFormat.Json);

            try
            {
                var response = await client.PutAsync<UploaderSettings>(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FileExtensions")]
        public async Task<IActionResult> GetFileExtensions([FromHeader(Name = "authorization")] string accessToken)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest("api/settings/fileExtensions", DataFormat.Json);

            try
            {
                var response = await client.GetAsync<List<UploaderFileExtension>>(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettings([FromHeader(Name = "authorization")] string accessToken, int id)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings/{id}", DataFormat.Json);

            try
            {
                var result = await client.ExecuteAsync(request, Method.DELETE);

                if (result.IsSuccessful)
                {
                    return Ok();
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSettings([FromHeader(Name = "authorization")] string accessToken, UploaderSettings uploaderSettings)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings", DataFormat.Json);
            request.AddJsonBody(uploaderSettings);

            try
            {
                var result = await client.ExecuteAsync(request, Method.POST);

                if (result.IsSuccessful)
                {
                    return Ok();
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{settingsId}/FileExtensions/{fileExtensionId}")]
        public async Task<IActionResult> AddFileExtensionToSettings([FromHeader(Name = "authorization")] string accessToken, int settingsId, int fileExtensionId)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings/{settingsId}/FileExtensions/{fileExtensionId}", DataFormat.Json);

            try
            {
                var result = await client.ExecuteAsync(request, Method.POST);

                if (result.IsSuccessful)
                {
                    return Ok();
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{settingsId}/FileExtensions/{fileExtensionId}")]
        public async Task<IActionResult> RemoveFileExtensionFromSettings([FromHeader(Name = "authorization")] string accessToken, int settingsId, int fileExtensionId)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings/{settingsId}/FileExtensions/{fileExtensionId}", DataFormat.Json);

            try
            {
                var result = await client.ExecuteAsync(request, Method.DELETE);

                if (result.IsSuccessful)
                {
                    return Ok();
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("FileExtensions")]
        public async Task<IActionResult> AddFileExtension([FromHeader(Name = "authorization")] string accessToken, UploaderFileExtension fileExtension)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/settings/FileExtensions", DataFormat.Json);
            request.AddJsonBody(new UploaderFileExtension { FileExtension = fileExtension.FileExtension });

            try
            {
                var result = await client.ExecuteAsync(request, Method.POST);

                if (result.IsSuccessful)
                {
                    return Ok(result.Content);
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
