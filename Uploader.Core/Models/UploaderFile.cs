namespace Uploader.Core.Models
{
    public class UploaderFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string BlobName { get; set; }

        public int ExtensionId { get; set; }

        public string Extension { get; set; }

        public string Comments { get; set; }
    }
}
