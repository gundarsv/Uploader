using Microsoft.AspNetCore.Http;

namespace Uploader.Core.Models
{
    public class UploadUploaderFile
    {
        public string FileName { get; set; }

        public string Comment { get; set; }

        public IFormFile File { get;set; }
    }
}
