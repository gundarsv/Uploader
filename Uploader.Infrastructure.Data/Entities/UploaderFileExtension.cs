using System;
using System.Collections.Generic;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Infrastructure.Data.Entities
{
    public class UploaderFileExtension : Entity
    {
        public int Id { get; set; }

        public string FileExtension { get; set; }

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<UploaderSettings> UploaderSettings { get; set; } = new List<UploaderSettings>();
    }
}
