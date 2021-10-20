using System;
using System.Linq;

namespace Uploader.Core.Helpers
{
    public static class FileHelper
    {
        private static readonly string[] validExtensions = { "jpg", "bmp", "gif", "png" };

        public static bool IsImage(string extension)
        {
            return validExtensions.Contains(extension.ToLower());
        }
    }
}
