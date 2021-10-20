using System.Collections.Generic;

namespace Uploader.Core.Models
{
    public class UploaderSettings
    {
        public int Id { get; set; }

        /// <summary>
        /// Max file size in MB
        /// </summary>
        public long MaxFileSize { get; set; }

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

        public bool IsEnabled { get; set; } = false;

        public List<UploaderFileExtension> AllowedFileExtensions { get; set; }
    }
}
