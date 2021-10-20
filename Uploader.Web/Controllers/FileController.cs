using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using Uploader.Core.Models;
using Uploader.Web.HttpClients;

namespace Uploader.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        [HttpGet("page/{pageNumber}/size/{pageSize}")]
        public async Task<IActionResult> GetFiles([FromHeader(Name = "authorization")] string accessToken, int pageNumber, int pageSize)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest("api/file", DataFormat.Json);

            request.AddParameter("pageNumber", pageNumber);
            request.AddParameter("pageSize", pageSize);

            try
            {
                var response = await client.GetAsync<PaginatedList<UploaderFile>>(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile([FromHeader(Name = "authorization")] string accessToken, int id)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/file/{id}", DataFormat.Json);

            try
            {
                var result = await client.ExecuteGetAsync(request);

                if (result.IsSuccessful)
                {
                    return File(result.RawBytes, result.ContentType);
                }

                return BadRequest(result.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromHeader(Name = "authorization")] string accessToken, [FromForm] UploadUploaderFile uploaderFile)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/file") { AlwaysMultipartFormData = true };

            try
            {
                var memoryStream = new MemoryStream();

                await uploaderFile.File.CopyToAsync(memoryStream);

                request.AddParameter("FileName", uploaderFile.FileName, ParameterType.GetOrPost);
                request.AddParameter("Comment", uploaderFile.Comment, ParameterType.GetOrPost);

                request.Files.Add(new FileParameter
                {
                    Name = "File",
                    Writer = (s) => {
                        var stream = uploaderFile.File.OpenReadStream();
                        stream.CopyTo(s);
                        stream.Dispose();
                    },
                    FileName = uploaderFile.File.FileName,
                    ContentType = uploaderFile.File.ContentType,
                    ContentLength = uploaderFile.File.Length
                });

                var result = await client.ExecutePostAsync<UploaderFile>(request);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile([FromHeader(Name = "authorization")] string accessToken, int id)
        {
            var client = RestSharpClient.Create(accessToken);

            var request = new RestRequest($"api/file/{id}", DataFormat.Json);

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
    }
}
