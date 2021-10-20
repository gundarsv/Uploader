using System.IO;

namespace Uploader.Core.Models
{
    public class FileResponse
    {
        public Stream Stream { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
