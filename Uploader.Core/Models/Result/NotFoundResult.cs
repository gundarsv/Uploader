using System.Collections.Generic;
using Uploader.Core.Enums;

namespace Uploader.Core.Models.Result
{
    public class NotFoundResult<T> : Result<T>
    {
        private readonly string error;

        public NotFoundResult(string error)
        {
            this.error = error;
        }

        public override ResultType ResultType => ResultType.NotFound;

        public override List<string> Errors => new() { error ?? "The entity could not be found" };

        public override T Data => default;
    }
}
