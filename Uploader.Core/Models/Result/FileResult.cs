using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class FileResult : Result<FileResponse>
    {
        private readonly FileResponse fileResponse;

        public FileResult(FileResponse fileResponse)
        {
            this.fileResponse = fileResponse;
        }

        public override ResultType ResultType => ResultType.File;

        public override List<string> Errors => new();

        public override FileResponse Data => fileResponse;
    }
}
