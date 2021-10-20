using System;
using System.Collections.Generic;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Infrastructure.Data.Entities
{
    public class UploaderSettings : Entity
    {
        public int Id { get; set; }
      
        /// <summary>
        /// Max file size in MB
        /// </summary>
        public long MaxFileSize {  get; set; }

        /// <summary>
        /// Max height in px
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// Max width in px
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Min height in px
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Min width in px
        /// </summary>
        public int MinWidth { get; set; }

        public EnabledUploaderSettings EnabledUserSettings { get; set; }

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<UploaderFileExtension> AllowedFileExtensions { get; set; } = new List<UploaderFileExtension>();
    }
}
