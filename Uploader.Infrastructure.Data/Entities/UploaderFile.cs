using System.Collections.Generic;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Infrastructure.Data.Entities
{
    public class UploaderFile : Entity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string BlobName { get; set; }

        public int ExtensionId { get; set; }

        public UploaderFileExtension Extension { get; set; }

        public string Comments { get; set; }
    }
}
